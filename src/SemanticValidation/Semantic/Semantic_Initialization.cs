using Microsoft.SemanticKernel;
using System.Text.Json;
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

        var testPlugin = TestKernel.CreatePluginFromPromptDirectory(testPluginDir);

        AreSameSkFunc = testPlugin["AreSame"];
        HasConditionFunc = testPlugin["HasCondition"];

    }
}