using System.Text.Json;
using Microsoft.Extensions.AI;
using SemanticValidation.Models;
using SemanticValidation.Utils;

namespace SemanticValidation;

public partial class Semantic
{
    /// <summary>
    /// Checks whether <paramref name="first"/> and <paramref name="second"/> string are semantically similar.
    /// It uses the kernel and OpenAI to check this semantically.
    /// <example>
    /// <code>
    /// AreSimilarAsync("This automobile is red", "The car is red") // returns true
    /// AreSimilarAsync("This tree is red", "The car is red") // returns false
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If the OpenAI was unable to generate a valid response.</exception>
    public async Task<SemanticValidationResult> AreSimilarAsync(string first, string second)
    {
        var prompt =
            $$"""
            Check if the first text and the second text are semantically equivalent:

            [[[First Text]]]

            {{first}}

            [[[End of First Text]]]


            [[[Second Text]]]

            {{second}}

            [[[End of Second Text]]]

            The result should be a valid json like:

            {
                "success": true or false,
                "message": "If they are not equivalent, explain here the difference"
            }


            RESULT: 
            """;

        var response =
            await ChatClient.GetResponseAsync(
                [
                    new ChatMessage(ChatRole.User, prompt)
                ],
                options: new ChatOptions
                {

                });

        var answer = response.Text ?? throw new InvalidOperationException("Can not assert the similarity");

        var result = SemanticUtils.PowerParseJson<SemanticValidationResult>(answer);

        if (result is null)
            throw new InvalidOperationException("Can not assert the similarity");

        return result;
    }

    /// <summary>
    /// Checks whether <paramref name="first"/> and <paramref name="second"/> string are semantically similar.
    /// It uses the kernel and OpenAI to check this semantically.
    /// <example>
    /// <code>
    /// AreSimilarAsync("This automobile is red", "The car is red") // returns true
    /// AreSimilarAsync("This tree is red", "The car is red") // returns false
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If the OpenAI was unable to generate a valid response.</exception>
    public SemanticValidationResult AreSimilar(string first, string second)
    {
        return AreSimilarAsync(first, second).GetAwaiter().GetResult();
    }
}