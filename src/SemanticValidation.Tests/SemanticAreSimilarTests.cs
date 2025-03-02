using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace SemanticValidation.Tests
{
    public class SemanticAreSimilarTests
    {
        private Semantic Semantic { get; set; }

        private ITestOutputHelper Output { get; set; }
        public SemanticAreSimilarTests(ITestOutputHelper output)
        {
            Output = output;

            var builder = new ConfigurationBuilder()
                .AddUserSecrets<SemanticAreSimilarTests>();

            IConfiguration configuration = builder.Build();

            var apiKey =
                configuration["AzureOpenAI_ApiKey"] ??
                throw new Exception("No ApiKey is provided.");
            var endpoint =
                configuration["AzureOpenAI_Endpoint"] ??
                throw new Exception("No Endpoint is provided.");
            var deploymentName =
                configuration["AzureOpenAI_Deployment"] ??
                throw new Exception("No Deployment is provided.");

            Semantic = new Semantic(
                new AzureOpenAIClient(
                    new Uri(endpoint),
                    new System.ClientModel.ApiKeyCredential(apiKey)
                ).AsChatClient(deploymentName));
        }

        [Theory]
        [MemberData(nameof(GetSimilarData))]
        public async Task AreSimilar_True_MustWork(string first, string second)
        {
            var result = await Semantic.AreSimilarAsync(first, second);
            Assert.True(result.IsValid, result.Reason);
        }

        [Theory]
        [MemberData(nameof(GetNonSimilarData))]
        public async Task AreSimilar_False_MustWork(string first, string second)
        {
            var result = await Semantic.AreSimilarAsync(first, second);
            Assert.False(result.IsValid);
            Output.WriteLine($"""
                [Explanation]
                {result.Reason}
                """);
        }

        public static IEnumerable<object[]> GetNonSimilarData()
        {
            yield return new object[]
            {
                "This car is red",
                "The car is blue"
            };
            yield return new object[]
            {
                "This bicycle is red",
                "The car is red"
            };
            yield return new object[]
            {
                "بفرما بشین!",
                "بشین بتمرگ!"
            };
        }

        public static IEnumerable<object[]> GetSimilarData()
        {
            yield return new object[]
            {
                "This car is red",
                "The car is red"
            };
            yield return new object[]
            {
                "This automobile is red",
                "The car is red"
            };
        }
    }
}