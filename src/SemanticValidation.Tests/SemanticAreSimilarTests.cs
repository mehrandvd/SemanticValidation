using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace SemanticValidation.Tests
{
    public class SemanticAreSimilarTests(ITestOutputHelper output) : TestBase(output)
    {
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