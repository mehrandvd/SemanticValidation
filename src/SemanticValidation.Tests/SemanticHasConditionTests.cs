using Xunit.Abstractions;

namespace SemanticValidation.Tests
{
    public class SemanticHasConditionTests(ITestOutputHelper output) : TestBase(output)
    {
        [Theory]
        [MemberData(nameof(GetHasConditionData))]
        public async Task HasCondition_True_MustWork(string text, string condition)
        {
            var result = await Semantic.HasConditionAsync(text, condition);
            Assert.True(result.IsValid, result.Reason);
        }

        [Theory]
        [MemberData(nameof(GetHasNotConditionData))]
        public async Task HasCondition_False_MustWork(string text, string condition)
        {
            var result = await Semantic.HasConditionAsync(text, condition);
            Assert.False(result.IsValid);
            Output.WriteLine($"""
                [Explanation]
                {result.Reason}
                """);
        }

        public static IEnumerable<object[]> GetHasNotConditionData()
        {
            yield return new object[]
            {
                "Such a beautiful day",
                "talks about night"
            };
            yield return new object[]
            {
                "You fucking bastard",
                "shows kindness"
            };
            yield return new object[]
            {
                "This car is red",
                "It talks about trees"
            };
            yield return new object[]
            {
                "This automobile is red",
                "It talks about blue"
            };
        }

        public static IEnumerable<object[]> GetHasConditionData()
        {
            yield return new object[]
            {
                "Such a beautiful day",
                "talks about a good day"
            };
            yield return new object[]
            {
                "You fucking bastard",
                "shows anger"
            };
            yield return new object[]
            {
                "This car is red",
                "It talks about cars"
            };
            yield return new object[]
            {
                "This car is red",
                "It talks about red"
            };

        }
    }
}