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

            var apiKey =
                Environment.GetEnvironmentVariable("openai-api-key", EnvironmentVariableTarget.User) ??
                throw new Exception("No ApiKey in environment variables.");
            var endpoint =
                Environment.GetEnvironmentVariable("openai-endpoint", EnvironmentVariableTarget.User) ??
                throw new Exception("No Endpoint in environment variables.");
            var deploymentName =
                Environment.GetEnvironmentVariable("openai-deployment-name", EnvironmentVariableTarget.User) ??
                throw new Exception("No DeploymentName in environment variables.");

            Semantic = new Semantic(deploymentName, endpoint, apiKey);
        }

        [Theory]
        [MemberData(nameof(GetSimilarData))]
        public async Task AreSimilar_True_MustWork(string first, string second)
        {
            var result = await Semantic.AreSimilarAsync(first, second);
            Assert.True(result.Success, result.Message);
        }

        [Theory]
        [MemberData(nameof(GetNonSimilarData))]
        public async Task AreSimilar_False_MustWork(string first, string second)
        {
            var result = await Semantic.AreSimilarAsync(first, second);
            Assert.False(result.Success);
            Output.WriteLine($"""
                [Explanation]
                {result.Message}
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