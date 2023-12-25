using Microsoft.SemanticKernel;
using System.Text.Json;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticValidation.Models;

namespace SemanticValidation;

/// <summary>
/// This class is for semantic validations. These type of validations require AI to check
/// the validation semantically. Validations like similarity:
/// <code>
/// AreSimilarAsync("This automobile is red", "The car is red") // returns true
/// AreSimilarAsync("This tree is red", "The car is red") // returns false
/// </code>
/// or semantically condition checking like:
/// <code>
/// HasConditionAsync("This car is red", "I talks about cars") // returns true
/// HasConditionAsync("This car is red", "I talks about trees") // returns false
/// </code>
/// </summary>
public partial class Semantic
{
    private Kernel TestKernel { get; }

    public KernelFunction AreSimilarFunc { get; set; } = default!;

    public KernelFunction HasConditionFunc { get; set; } = default!;

    /// <summary>
    /// The Semantic library needs a SemanticKernel kernel to work.
    /// Using this constructor you can use an AzureOpenAI subscription to configure it.
    /// If you want to connect using an OpenAI client, you can configure your kernel
    /// as you like and pass your pre-configured kernel using the other constructor.
    /// </summary>
    /// <param name="deploymentName"></param>
    /// <param name="endpoint"></param>
    /// <param name="apiKey"></param>
    public Semantic(string deploymentName, string endpoint, string apiKey)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey);

        TestKernel = builder.Build();
        InitializeKernel();
    }

    /// <summary>
    /// The Semantic library needs a SemanticKernel kernel to work.
    /// Pass your pre-configured kernel to this constructor.
    /// </summary>
    /// <param name="kernel"></param>
    public Semantic(Kernel kernel)
    {
        TestKernel = kernel;
        InitializeKernel();
    }

    private void InitializeKernel()
    {
        AreSimilarFunc = TestKernel.CreateFunctionFromPrompt("""
            Check if the first text and the second text are semantically equivalent:

            [[[First Text]]]

            {{$first_text}}

            [[[End of First Text]]]


            [[[Second Text]]]

            {{$second_text}}

            [[[End of Second Text]]]

            The result should be a valid json like:

            {
                "success": true or false,
                "message": "If they are not equivalent, explain here the difference"
            }


            RESULT: 
            """);

        HasConditionFunc = TestKernel.CreateFunctionFromPrompt("""
            Check if the text has the condition semantically:

            [[[Input Text]]]

            {{$text}}

            [[[End of Input Text]]]


            [[[Condition]]]

            {{$condition}}

            [[[End of Condition]]]

            The result should be a valid json like:

            {
                "success": true or false,
                "message": "If success is false, explain (in the same language with text) here the difference"
            }


            RESULT: 
            """);

    }
}