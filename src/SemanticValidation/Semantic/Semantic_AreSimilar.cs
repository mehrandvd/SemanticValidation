using Microsoft.SemanticKernel;
using System.Text.Json;
using SemanticValidation.Models;

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
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<SemanticValidationResult> AreSimilarAsync(string first, string second)
    {
        var skResult = (
            await AreSimilarFunc.InvokeAsync(TestKernel, new KernelArguments()
            {
                ["first_text"] = first,
                ["second_text"] = second
            })
        ).GetValue<string>() ?? "";

        var result = JsonSerializer.Deserialize<SemanticValidationResult>(skResult);

        if (result is null)
            throw new InvalidOperationException("Can not assert Similarity");

        return result;
    }
}