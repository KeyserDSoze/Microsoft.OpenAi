using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Microsoft.OpenAi.Api
{
    public static class HttpClientExtensions
    {
        internal static async Task<HttpResponseMessage> PrivatedExecuteAsync(this HttpClient client, string url, object? message, bool isStreaming, bool isDelete)
        {
            var request = new HttpRequestMessage(isDelete ? HttpMethod.Delete : (message != null ? HttpMethod.Post : HttpMethod.Get), url);
            if (message != null)
            {
                if (message is HttpContent httpContent)
                {
                    request.Content = httpContent;
                }
                else
                {
                    var jsonContent = JsonSerializer.Serialize(message);
                    var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    request.Content = stringContent;
                }
            }
            var response = await client.SendAsync(request, isStreaming ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }
        internal static async ValueTask<TResponse> DeleteAsync<TResponse>(this HttpClient client, string url, object? message)
        {
            var response = await client.PrivatedExecuteAsync(url, message, false, true);
            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseAsString)!;
        }
        internal static async ValueTask<TResponse> ExecuteAsync<TResponse>(this HttpClient client, string url, object? message)
        {
            var response = await client.PrivatedExecuteAsync(url, message, false, false);
            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseAsString)!;
        }
        private const string StartingWith = "data: ";
        private const string Done = "[DONE]";
        internal static async IAsyncEnumerable<TResponse> ExecuteStreamAsync<TResponse>(this HttpClient client, string url, object? message)
        {
            var response = await client.PrivatedExecuteAsync(url, message, true, false);
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line.StartsWith(StartingWith))
                    line = line.Substring(StartingWith.Length);
                if (line == Done)
                {
                    yield break;
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    var res = JsonSerializer.Deserialize<TResponse>(line);
                    if (res is ApiResultBase apiResult)
                        apiResult.SetHeaders(response);
                    yield return res!;
                }
            }
        }
        private static void SetHeaders<TResponse>(this TResponse result, HttpResponseMessage response)
            where TResponse : ApiResultBase
        {
            try
            {
                result.Organization = response.Headers.GetValues("Openai-Organization").FirstOrDefault();
                result.RequestId = response.Headers.GetValues("X-Request-ID").FirstOrDefault();
                result.ProcessingTime = TimeSpan.FromMilliseconds(int.Parse(response.Headers.GetValues("Openai-Processing-Ms").First()));
                result.OpenaiVersion = response.Headers.GetValues("Openai-Version").FirstOrDefault();
                result.ModelId = response.Headers.GetValues("Openai-Model").FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.Print($"Issue parsing metadata of OpenAi Response.  Error: {e.ToString()}.  This is probably ignorable.");
            }
        }
    }
}
