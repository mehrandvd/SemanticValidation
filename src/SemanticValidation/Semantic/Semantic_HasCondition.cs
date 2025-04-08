using System.Text.Json;
using Microsoft.Extensions.AI;
using SemanticValidation.Models;
using SemanticValidation.Utils;

namespace SemanticValidation
{
    public partial class Semantic
    {
        /// <summary>
        /// Checks whether <paramref name="text"/> has the <paramref name="condition"/> semantically.
        /// It uses the kernel and OpenAI to check this semantically.
        /// <example>
        /// <code>
        /// HasConditionAsync("This car is red", "It talks about cars") // returns true
        /// HasConditionAsync("This car is red", "It talks about trees") // returns false
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="text">The text that the condition should be checked on</param>
        /// <param name="condition">The condition</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the OpenAI was unable to generate a valid response.</exception>
        public async Task<SemanticValidationResult> HasConditionAsync(string text, string condition)
        {
            var prompt =
                $$"""
                Check if the text has the condition semantically:

                [[[Input Text]]]

                {{text}}

                [[[End of Input Text]]]


                [[[Condition]]]

                {{condition}}

                [[[End of Condition]]]

                The result should be a valid json like:

                {
                    "success": true or false,
                    "message": "If success is false, explain (in the same language with text) here the difference"
                }


                RESULT: 
                """;

            var response = 
                await ChatClient.GetResponseAsync(
                    [
                        new ChatMessage(ChatRole.User, prompt)
                    ]);

            var answer = response.Text ?? throw new InvalidOperationException("Can not assert the condition");

            var result = SemanticUtils.PowerParseJson<SemanticValidationResult>(answer);

            if (result is null)
                throw new InvalidOperationException("Can not assert the condition");

            return result;
        }

        /// <summary>
        /// Checks whether <paramref name="text"/> has the <paramref name="condition"/> semantically.
        /// It uses the kernel and OpenAI to check this semantically.
        /// <example>
        /// <code>
        /// HasConditionAsync("This car is red", "It talks about cars") // returns true
        /// HasConditionAsync("This car is red", "It talks about trees") // returns false
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
