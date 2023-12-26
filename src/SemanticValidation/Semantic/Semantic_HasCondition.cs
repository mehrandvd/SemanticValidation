using Microsoft.SemanticKernel;
using System.Text.Json;
using SemanticValidation.Models;

namespace SemanticValidation
{
    public partial class Semantic
    {
        /// <summary>
        /// Checks whether <paramref name="text"/> has the <paramref name="condition"/> semantically.
        /// It uses the kernel and OpenAI to check this semantically.
        /// <example>
        /// <code>
        /// HasConditionAsync("This car is red", "I talks about cars") // returns true
        /// HasConditionAsync("This car is red", "I talks about trees") // returns false
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="text">The text that the condition should be checked on</param>
        /// <param name="condition">The condition</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the OpenAI was unable to generate a valid response.</exception>
        public async Task<SemanticValidationResult> HasConditionAsync(string text, string condition)
        {
            var skResult = (
                await HasConditionFunc.InvokeAsync(TestKernel, new KernelArguments()
                {
                    ["text"] = text,
                    ["condition"] = condition
                })
            ).GetValue<string>() ?? "";

            var result = JsonSerializer.Deserialize<SemanticValidationResult>(skResult);

            if (result is null)
                throw new InvalidOperationException("Can not assert Similarity");

            return result;
        }

        /// <summary>
        /// Checks whether <paramref name="text"/> has the <paramref name="condition"/> semantically.
        /// It uses the kernel and OpenAI to check this semantically.
        /// <example>
        /// <code>
        /// HasConditionAsync("This car is red", "I talks about cars") // returns true
        /// HasConditionAsync("This car is red", "I talks about trees") // returns false
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="text">The text that the condition should be checked on</param>
        /// <param name="condition">The condition</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the OpenAI was unable to generate a valid response.</exception>
        public SemanticValidationResult HasCondition(string text, string condition)
        {
            return HasConditionAsync(text, condition).GetAwaiter().GetResult();
        }
    }
}
