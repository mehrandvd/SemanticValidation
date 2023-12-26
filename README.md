# Semantic Validation
Semantic Validation is a library to **bring OpenAI into the validation systems** like:
```csharp
var result = Semantic.AreSimilar("This automobile is red", "The car is red");

Console.WriteLine(result.IsValid);
// true
```
The interesting part is that: **When it fails, it explains why!** (thanks to OpenAI)
```csharp
var result = Semantic.AreSimilar("This bicycle is red", "The car is red");

Console.WriteLine(result.IsValid);
// false

Console.WriteLine(result.Reason);
// The first text describes a red bicycle, while the second text describes a red car. They are not semantically equivalent.
```
Under the hood, it uses OpenAI and SemanticKernel to do all the semantic stuff.

There are other semantic methods available too. The `HasConditoin` checks if a `text` meets a special `condition`. And again watch how 
great it describes the reason for semantic validation failure.
```csharp
var result = Semantic.HasCondition(
                text: "This car is red",
                condition: "It talks about trees"

Console.WriteLine(result.IsValid);
// false

Console.WriteLine(result.Reason);
// The input text does not talk about trees
```

## How to use?
Install the [SemanticValidation NuGet package](https://www.nuget.org/packages/SemanticValidation) using this command:
```command
dotnet add package SemanticValidation
```

And then instantiate it using your OpenAI subscription:
```csharp
var semantic = new Semantic(deployment, endpoint, apiKey);
```
All done, enjoy using it!
