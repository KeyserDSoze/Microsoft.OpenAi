﻿using System;
using System.Threading.Tasks;
using OpenSource.OpenAi.Models;
using Xunit;

namespace Azure.OpenAi.Test
{
    public class ModelEndpointTests
    {
        [Fact]
        public async ValueTask GetAllModelsAsync()
        {
            var api = DiUtility.GetOpenAi();

            Assert.NotNull(api.Model);

            var results = await api.Model.ListAsync();
            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Contains(results, c => c.Id.ToLower().StartsWith("text-davinci"));
        }

        [Fact]
        public async ValueTask GetModelDetailsAsync()
        {
            var api = DiUtility.GetOpenAi();

            Assert.NotNull(api.Model);

            var result = await api.Model.RetrieveAsync(TextModelType.DavinciText3.ToModel().Id);
            Assert.NotNull(result);

            Assert.NotNull(result.CreatedUnixTime);
            Assert.True(result.CreatedUnixTime.Value != 0);
            Assert.NotNull(result.Created);
            Assert.True(result.Created.Value > new DateTime(2018, 1, 1));
            Assert.True(result.Created.Value < DateTime.Now.AddDays(1));

            Assert.NotNull(result.Id);
            Assert.NotNull(result.OwnedBy);
            Assert.Equal(TextModelType.DavinciText3.ToModel().Id, result.Id.ToLower());
        }


        [Fact]
        public async ValueTask GetEnginesAsync_ShouldReturnTheEngineList()
        {
            var api = DiUtility.GetOpenAi();
            var models = await api.Model.ListAsync();
            Assert.True(models.Count > 5);
        }

        [Theory]
        [InlineData("ada")]
        [InlineData("babbage")]
        [InlineData("curie")]
        [InlineData("davinci")]
        public async ValueTask RetrieveEngineDetailsAsync_ShouldRetrieveEngineDetails(string modelId)
        {
            var api = DiUtility.GetOpenAi();
            var modelData = await api.Model.RetrieveAsync(modelId);
            Assert.Equal(modelId, modelData.Id);
            Assert.True(modelData.Created > new DateTime(2018, 1, 1));
            Assert.True(modelData.Created < DateTime.UtcNow.AddDays(1));
        }
    }
}