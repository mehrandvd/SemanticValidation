using Microsoft.SemanticKernel;
using System.Text.Json;
using SemanticValidation.Models;

namespace SemanticValidation;

public partial class Semantic
{
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