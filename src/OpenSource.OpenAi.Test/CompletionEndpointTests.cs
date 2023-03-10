using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenSource.OpenAi;
using OpenSource.OpenAi.Completion;
using OpenSource.OpenAi.Models;
using Xunit;

namespace Azure.OpenAi.Test
{
    public class CompletionEndpointTests
    {
        private readonly IOpenAiApi _openAiApi;
        public CompletionEndpointTests(IOpenAiApi openAiApi)
        {
            _openAiApi = openAiApi;
        }
        [Fact]
        public async ValueTask GetBasicCompletionAsync()
        {
            Assert.NotNull(_openAiApi.Completion);

            var results = await _openAiApi.Completion
                .Request("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight")
                .WithModel(TextModelType.CurieText)
                .WithTemperature(0.1)
                .SetMaxTokens(5)
                .ExecuteAsync();

            Assert.NotNull(results);
            Assert.NotNull(results.CreatedUnixTime);
            Assert.True(results.CreatedUnixTime.Value != 0);
            Assert.NotNull(results.Created);
            Assert.True(results.Created.Value > DateTime.UtcNow.AddSeconds(-30));
            Assert.True(results.Created.Value < DateTime.UtcNow.AddSeconds(30));
            Assert.NotNull(results.Completions);
            Assert.True(results.Completions.Count != 0);
            Assert.Contains(results.Completions, c => c.Text.Trim().ToLower().StartsWith("nine"));
        }


        [Fact]
        public async ValueTask GetSimpleCompletionAsync()
        {
            Assert.NotNull(_openAiApi.Completion);

            var results = await _openAiApi.Completion
                .Request("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight")
                .WithTemperature(0.1)
                .SetMaxTokens(5)
                .ExecuteAsync();
            Assert.NotNull(results);
            Assert.True(results.Completions.Count > 0);
            Assert.Contains(results.Completions, c => c.Text.Trim().ToLower().StartsWith("nine"));
        }


        [Fact]
        public async ValueTask CompletionUsageDataWorksAsync()
        {
            Assert.NotNull(_openAiApi.Completion);

            var results = await _openAiApi.Completion
               .Request("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight")
               .WithModel(TextModelType.CurieText)
               .WithTemperature(0.1)
               .SetMaxTokens(5)
               .ExecuteAsync();
            Assert.NotNull(results);
            Assert.NotNull(results.Usage);
            Assert.True(results.Usage.PromptTokens > 15);
            Assert.True(results.Usage.CompletionTokens > 0);
            Assert.True(results.Usage.TotalTokens >= results.Usage.PromptTokens + results.Usage.CompletionTokens);
        }


        [Fact]
        public async Task CreateCompletionAsync_MultiplePrompts_ShouldReturnResult()
        {
            Assert.NotNull(_openAiApi.Completion);
            var results = new List<CompletionResult>();
            await foreach (var x in _openAiApi.Completion
               .Request("Today is Monday, tomorrow is", "10 11 12 13 14")
               .WithTemperature(0)
               .SetMaxTokens(3)
               .ExecuteAsStreamAsync())
            {
                results.Add(x);
            }

            Assert.NotEmpty(results);
            Assert.Contains(results.First().Completions, c => c.Text.Trim().ToLower().StartsWith("tuesday"));
            Assert.Contains(results.Last().Completions, c => c.Text.Trim().ToLower().StartsWith("15"));
        }


        //    [Theory]
        //    [InlineData(-0.2)]
        //    [InlineData(3)]
        //    public void CreateCompletionAsync_ShouldNotAllowTemperatureOutside01(double temperature)
        //    {
        //        var api = DiUtility.GetOpenAi();
        //        Assert.NotNull(api.Completion);

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = temperature,
        //            MaxTokens = 10
        //        };

        //        Func<Task> act = () => api.Completions.CreateCompletionsAsync(completionReq, 1);
        //        act.Should()
        //            .ThrowAsync<HttpRequestException>()
        //            .Where(exc => exc.Message.Contains("temperature"));
        //    }

        //    [Theory]
        //    [InlineData(1.8)]
        //    [InlineData(1.9)]
        //    [InlineData(2.0)]
        //    public async Task ShouldBeMoreCreativeWithHighTemperature(double temperature)
        //    {
        //        var api = DiUtility.GetOpenAi();
        //        Assert.NotNull(api.Completion);

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = temperature,
        //            MaxTokens = 5
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Count.Should().Be(5, "completion count should be the default");
        //        results.Completions.Distinct().Count().Should().Be(results.Completions.Count);
        //    }
        //    [Theory]
        //    [InlineData(0.05)]
        //    [InlineData(0.1)]
        //    public async Task CreateCompletionAsync_ShouldGetSomeResultsWithVariousTopPValues(double topP)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = 0,
        //            MaxTokens = 5,
        //            TopP = topP
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Count.Should().Be(5, "completion count should be the default");
        //    }

        //    [Theory]
        //    [InlineData(-0.5)]
        //    [InlineData(0.0)]
        //    [InlineData(0.5)]
        //    [InlineData(1.0)]
        //    public async Task CreateCompletionAsync_ShouldReturnSomeResultsForPresencePenalty(double presencePenalty)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = 0,
        //            MaxTokens = 5,
        //            PresencePenalty = presencePenalty
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Count.Should().Be(5, "completion count should be the default");
        //    }

        //    [Theory]
        //    [InlineData(-0.5)]
        //    [InlineData(0.0)]
        //    [InlineData(0.5)]
        //    [InlineData(1.0)]
        //    public async Task CreateCompletionAsync_ShouldReturnSomeResultsForFrequencyPenalty(double frequencyPenalty)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = 0,
        //            MaxTokens = 5,
        //            FrequencyPenalty = frequencyPenalty
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Count.Should().Be(5, "completion count should be the default");
        //    }

        //    [Fact]
        //    public async Task CreateCompletionAsync_ShouldWorkForBiggerNumberOfCompletions()
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = 0,
        //            MaxTokens = 5,
        //            NumChoicesPerPrompt = 2
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Count.Should().Be(5, "completion count should be the default");
        //    }
        //    [Theory]
        //    [InlineData(1)]
        //    [InlineData(2)]
        //    [InlineData(5)]
        //    public async Task CreateCompletionAsync_ShouldAlsoReturnLogProps(int logProps)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = 0,
        //            MaxTokens = 5,
        //            Logprobs = logProps
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Count.Should().Be(5, "completion count should be the default");
        //        results.Completions[0].Logprobs.TopLogprobs.Count.Should()
        //            .Be(5, "logprobs should be returned for each completion");
        //        results.Completions[0].Logprobs.TopLogprobs[0].Keys.Count.Should().Be(logProps,
        //            "because logprops count should be the same as requested");
        //    }

        //    [Fact]
        //    public async Task CreateCompletionAsync_Echo_ShouldReturnTheInput()
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            Temperature = 0,
        //            MaxTokens = 5,
        //            Echo = true
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Should().OnlyContain(c => c.Text.StartsWith(completionReq.Prompt), "Echo should get the prompt back");
        //    }
        //    [Theory]
        //    [InlineData("Thursday")]
        //    [InlineData("Friday")]
        //    public async Task CreateCompletionAsync_ShouldStopOnStopSequence(string stopSeq)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionReq = new CompletionRequest
        //        {
        //            Prompt = "Monday Tuesday Wednesday",
        //            Temperature = 0,
        //            MaxTokens = 5,
        //            Echo = true,
        //            StopSequence = stopSeq
        //        };

        //        var results = await api.Completions.CreateCompletionsAsync(completionReq);
        //        results.ShouldNotBeEmpty();
        //        results.Completions.Should().OnlyContain(c => !c.Text.Contains(stopSeq), "Stop sequence must not be returned");
        //        results.Completions.Should().OnlyContain(c => c.FinishReason == "stop", "must end due to stop sequence");
        //    }

        //    [Fact]
        //    public async Task CreateCompletionAsync_MultipleParamShouldReturnTheSameDataAsSingleParamVersion()
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var r = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            MaxTokens = 5,
        //            Temperature = 0,
        //            TopP = 0.1,
        //            PresencePenalty = 0.5,
        //            FrequencyPenalty = 0.3,
        //            NumChoicesPerPrompt = 2,
        //            Echo = true
        //        };

        //        var resultOneParam = await api.Completions.CreateCompletionsAsync(r);
        //        resultOneParam.ShouldNotBeEmpty();

        //        var resultsMultipleParams = await api.Completions.CreateCompletionAsync(
        //            r.Prompt, Model.DefaultModel, r.MaxTokens, r.Temperature, r.TopP, r.NumChoicesPerPrompt, r.PresencePenalty,
        //            r.FrequencyPenalty,
        //            null, r.Echo);
        //        resultsMultipleParams.ShouldNotBeEmpty();

        //        resultOneParam.Should().BeEquivalentTo(resultsMultipleParams, opt => opt
        //                .Excluding(o => o.Id)
        //                .Excluding(o => o.CreatedUnixTime)
        //                .Excluding(o => o.Created)
        //                .Excluding(o => o.ProcessingTime)
        //                .Excluding(o => o.RequestId)
        //        );
        //    }
        //    [Theory]
        //    [InlineData(5, 3)]
        //    [InlineData(7, 2)]
        //    public async Task StreamCompletionAsync_ShouldStreamIndexAndData(int maxTokens, int numOutputs)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionRequest = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            MaxTokens = maxTokens,
        //            NumChoicesPerPrompt = numOutputs,
        //            Temperature = 0,
        //            TopP = 0.1,
        //            PresencePenalty = 0.5,
        //            FrequencyPenalty = 0.3,
        //            Logprobs = 3,
        //            Echo = true,
        //        };

        //        var streamIndexes = new List<int>();
        //        var completionResults = new List<CompletionResult>();
        //        await api.Completions.StreamCompletionAsync(completionRequest, (index, result) =>
        //        {
        //            streamIndexes.Add(index);
        //            completionResults.Add(result);
        //        });

        //        int expectedCount = maxTokens * numOutputs;
        //        streamIndexes.Count.Should().Be(expectedCount);
        //        completionResults.Count.Should().Be(expectedCount);
        //    }
        //    [Theory]
        //    [InlineData(5, 3)]
        //    [InlineData(7, 2)]
        //    public async Task StreamCompletionAsync_ShouldStreamData(int maxTokens, int numOutputs)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionRequest = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            MaxTokens = maxTokens,
        //            NumChoicesPerPrompt = numOutputs,
        //            Temperature = 0,
        //            TopP = 0.1,
        //            PresencePenalty = 0.5,
        //            FrequencyPenalty = 0.3,
        //            Logprobs = 3,
        //            Echo = true,
        //        };

        //        var completionResults = new List<CompletionResult>();
        //        await api.Completions.StreamCompletionAsync(completionRequest, result =>
        //        {
        //            completionResults.Add(result);
        //        });

        //        int expectedCount = maxTokens * numOutputs;
        //        completionResults.Count.Should().Be(expectedCount);
        //    }
        //    [Theory]
        //    [InlineData(5, 3)]
        //    [InlineData(7, 2)]
        //    public async Task StreamCompletionEnumerableAsync_ShouldStreamData(int maxTokens, int numOutputs)
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var completionRequest = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            MaxTokens = maxTokens,
        //            NumChoicesPerPrompt = numOutputs,
        //            Temperature = 0,
        //            TopP = 0.1,
        //            PresencePenalty = 0.5,
        //            FrequencyPenalty = 0.3,
        //            Logprobs = 3,
        //            Echo = true,
        //        };

        //        var completionResults = new List<CompletionResult>();
        //        await foreach (var res in api.Completions.StreamCompletionEnumerableAsync(completionRequest))
        //        {
        //            completionResults.Add(res);
        //        }

        //        int expectedCount = maxTokens * numOutputs;
        //        completionResults.Count.Should().Be(expectedCount);
        //    }
        //    [Theory]
        //    [Fact]
        //    public async Task StreamCompletionEnumerableAsync_MultipleParamShouldReturnTheSameDataAsSingleParamVersion()
        //    {
        //        var api = DiUtility.GetOpenAi();

        //        var r = new CompletionRequest
        //        {
        //            Prompt = "three four five",
        //            MaxTokens = 5,
        //            Temperature = 0,
        //            TopP = 0.1,
        //            PresencePenalty = 0.5,
        //            FrequencyPenalty = 0.3,
        //            NumChoicesPerPrompt = 2,
        //            Logprobs = null,
        //            Echo = true
        //        };

        //        var resultsOneParam = new List<CompletionResult>();
        //        await foreach (var res in api.Completions.StreamCompletionEnumerableAsync(r))
        //        {
        //            resultsOneParam.Add(res);
        //        }

        //        resultsOneParam.Should().NotBeEmpty("At least one result should be fetched");

        //        var resultsMultipleParams = new List<CompletionResult>();
        //        await foreach (var res in api.Completions.StreamCompletionEnumerableAsync(
        //            r.Prompt, Model.DefaultModel, r.MaxTokens, r.Temperature, r.TopP, r.NumChoicesPerPrompt, r.PresencePenalty,
        //            r.FrequencyPenalty,
        //            null, r.Echo))
        //        {
        //            resultsMultipleParams.Add(res);
        //        }
        //        resultsMultipleParams.Should().NotBeEmpty();

        //        resultsOneParam.Should().BeEquivalentTo(resultsMultipleParams, opt => opt
        //            .Excluding(o => o.Id)
        //            .Excluding(o => o.CreatedUnixTime)
        //            .Excluding(o => o.Created)
        //            .Excluding(o => o.ProcessingTime)
        //            .Excluding(o => o.RequestId)
        //        );
        //    }
        //}

        //public static class CompletionTestingHelper
        //{
        //    public static void ShouldNotBeEmpty(this CompletionResult results)
        //    {
        //        results.Should().NotBeNull("a result must be received");
        //        results.Completions.Should().NotBeNull("completions must be received");
        //        results.Completions.Should().NotBeEmpty("completions must be non-empty");
        //    }

        //    public static void ShouldContainAStringStartingWith(this CompletionResult results, string startToken, string because = "")
        //    {
        //        results.Completions.Should().Contain(c => c.Text.Trim().ToLower().StartsWith(startToken), because);
        //    }
    }
}
