using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.OpenAi.Test
{
    public class EmbeddingEndpointTests
    {
        [Fact]
        public async ValueTask GetBasicEmbeddingAsync()
        {
            var api = DiUtility.GetOpenAi();

            Assert.NotNull(api.Embedding);

            var results = await api.Embedding.Request("A test text for embedding").ExecuteAsync();
            Assert.NotNull(results);
            if (results.CreatedUnixTime.HasValue)
            {
                Assert.True(results.CreatedUnixTime.Value != 0);
                Assert.NotNull(results.Created);
                Assert.True(results.Created.Value > new DateTime(2018, 1, 1));
                Assert.True(results.Created.Value < DateTime.Now.AddDays(1));
            }
            else
            {
                Assert.Null(results.Created);
            }
            Assert.NotNull(results.Object);
            Assert.True(results.Data.Count != 0);
            Assert.True(results.Data.First().Embedding.Length == 1536);
        }

        [Fact]
        public async ValueTask ReturnedUsageAsync()
        {
            var api = DiUtility.GetOpenAi();

            Assert.NotNull(api.Embedding);

            var results = await api.Embedding.Request("A test text for embedding").ExecuteAsync();
            Assert.NotNull(results);

            Assert.NotNull(results.Usage);
            Assert.True(results.Usage.PromptTokens >= 5);
            Assert.True(results.Usage.TotalTokens >= results.Usage.PromptTokens);
        }

        [Fact]
        public async ValueTask GetSimpleEmbeddingAsync()
        {
            var api = DiUtility.GetOpenAi();

            Assert.NotNull(api.Embedding);

            var results = await api.Embedding.Request("A test text for embedding").ExecuteAsync();
            Assert.NotNull(results);
            Assert.True(results.Data.Count == 1536);
        }
    }
}
