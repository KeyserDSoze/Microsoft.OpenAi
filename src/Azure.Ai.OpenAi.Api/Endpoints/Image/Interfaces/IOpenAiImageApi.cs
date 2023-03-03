using System.IO;

namespace Azure.Ai.OpenAi.Image
{
    public interface IOpenAiImageApi
    {
        ImageCreateRequestBuilder Generate(string prompt);
        ImageVariationRequestBuilder Variate(Stream image, string imageName = "image.png");
    }
}
