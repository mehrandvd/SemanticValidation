using System.Text.Json;
using Microsoft.Extensions.AI;
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
    private IChatClient ChatClient { get; }

    /// <summary>
    /// The Semantic library needs a ChatClient to work.
    /// </summary>
    public Semantic(IChatClient chatClient)
    {
        ChatClient = chatClient;
        InitializeKernel();
    }

    private void InitializeKernel()
    {
    }
}