using Microsoft.SemanticKernel;
using System.Text.Json;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticValidation.Models;

namespace SemanticValidation;

public partial class Semantic
{
    private Kernel TestKernel { get; }

    public KernelFunction AreSameSkFunc { get; set; }

    public KernelFunction HasConditionFunc { get; set; }

    public Semantic(string endpoint, string apiKey)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion("gpt-35-turbo-test", endpoint, apiKey);

        TestKernel = builder.Build();

        var dir = Environment.CurrentDirectory;
        var testPluginDir = Path.Combine(dir, "Plugins", "TestPlugin");
        if (!Directory.Exists(testPluginDir))
        {
            throw new InvalidOperationException($"TestPlugin directory does not exist: {testPluginDir}");
        }

        //var testPlugin = TestKernel.CreatePluginFromPromptDirectory(testPluginDir);

        //AreSameSkFunc = testPlugin["AreSame"];
        //HasConditionFunc = testPlugin["HasCondition"];

        AreSameSkFunc = TestKernel.CreateFunctionFromPrompt("""
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