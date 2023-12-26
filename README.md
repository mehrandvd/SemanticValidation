# Semantic Validation
[![Build and Deploy](https://github.com/mehrandvd/SemanticValidation/actions/workflows/build.yml/badge.svg)](https://github.com/mehrandvd/SemanticValidation/actions/workflows/build.yml)
[![NuGet version (SemanticValidation)](https://img.shields.io/nuget/v/SemanticValidation.svg?style=flat)](https://www.nuget.org/packages/SemanticValidation/)
[![NuGet downloads](https://img.shields.io/nuget/dt/SemanticValidation.svg?style=flat)](https://www.nuget.org/packages/SemanticValidation)

SemanticValidation is a library that integrates OpenAI’s powerful language models with validation systems. It allows you to perform semantic checks on your data and queries using natural language understanding.

It **brings the power of OpenAI into the validation systems** as easily as this:
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
Under the hood, it uses OpenAI and [SemanticKernel](https://github.com/microsoft/semantic-kernel/) to do all the semantic stuff.

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

## Features
- **Brings the power of OpenAI into the validation systems**: You can use OpenAI's language models to perform semantic validation on your data and queries with a simple and intuitive syntax.
- **Provides explanatory feedback**: When a semantic check fails, it explains why, thanks to OpenAI's natural language generation capabilities.
- **Uses OpenAI and SemanticKernel under the hood**: Semantic Validation relies on OpenAI and SemanticKernel to do all the semantic stuff. SemanticKernel is a library that provides a unified interface to interact with OpenAI's language models.

## Requirements
- .NET 8.0 or higher
- An OpenAI API key


## Installation
Semantic Validation is available as a [NuGet](https://www.nuget.org/packages/SemanticValidation) package that you can easily install in your project. To do so, run the following command in your terminal:
```bash
dotnet add package SemanticValidation
```

Next, you need to create an instance of the `Semantic` class and pass your OpenAI subscription details as parameters:
```csharp
var semantic = new Semantic(deployment, endpoint, apiKey);
```
That's it! You are ready to use Semantic Validation in your code. 😊
