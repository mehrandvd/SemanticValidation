using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace SemanticValidation.Tests;

public class TestBase
{

    protected ITestOutputHelper Output { get; set; }
    protected Semantic Semantic { get; set; }
    protected IConfiguration Configuration { get; set; }

    protected TestBase(ITestOutputHelper output)
    {
        Output = output;

        var builder = new ConfigurationBuilder()
                      .AddUserSecrets<SemanticAreSimilarTests>()
                      .AddEnvironmentVariables();

        Configuration = builder.Build();

        var apiKey =
            Configuration["AZUREOPENAI_APIKEY"] ??
            throw new Exception("No ApiKey is provided: AZUREOPENAI_APIKEY");
        var endpoint =
            Configuration["AzureOpenAI_Endpoint"] ??
            throw new Exception("No Endpoint is provided.");
        var deploymentName =
            Configuration["AzureOpenAI_Deployment"] ??
            throw new Exception("No Deployment is provided.");

        Semantic = new Semantic(
            new AzureOpenAIClient(
                new Uri(endpoint),
                new System.ClientModel.ApiKeyCredential(apiKey)
            ).GetChatClient(deploymentName).AsIChatClient());
    }
}