# Rocks

[![NuGet](https://img.shields.io/nuget/v/Rocks.svg)](https://www.nuget.org/packages/Rocks)

[![Bugs](https://img.shields.io/github/issues/JasonBock/Rocks/bug)](https://github.com/JasonBock/Rocks/issues?q=is%3Aissue%20state%3Aopen%20label%3Abug)

[![Issues](https://img.shields.io/github/issues/JasonBock/Rocks)](https://github.com/JasonBock/Rocks/issues)

![Rocks logo](https://raw.github.com/JasonBock/Rocks/main/src/Images/Banner-Small.png)

A mocking library based on the Compiler APIs (Roslyn + Mocks).

## Getting Started

Reference the `Rocks` [NuGet package](https://www.nuget.org/packages/Rocks) - that's it.

### Prerequisites

The Rocks package targets .NET Standard 2.0 for host flexibility. That said, Rocks (as of `9.0.0`) will generate code that requires .NET 9.

## Usage

To make a mock, you take an interface or an unsealed class that has virtual members:

```csharp
public interface IAmSimple
{
  void TargetAction();
}
```

and you use Rocks to create a mock with expectations, along with verifying its usage:

```csharp
[assembly: Rock(typeof(IAmSimple), BuildType.Create)]

var expectations = new IAmSimpleCreateExpectations();
expectations.Methods.TargetAction();

var mock = expectations.Instance();
mock.TargetAction();

expectations.Verify();
```

More details can be found on the [Overview page](https://github.com/JasonBock/Rocks/blob/main/docs/Overview.md).

## Additional Documentation

* [Discord Server](https://discord.gg/TZ62ftF7YT)
* [Changelog](https://github.com/JasonBock/Rocks/blob/main/changelog.md)
* [Unit testing best practices with .NET Core and .NET Standard](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
* [BenchmarkMockNet](https://github.com/ecoAPM/BenchmarkMockNet)
* [Unit Testing CSLA Rules With Rocks](https://blog.lhotka.net/2025/10/02/Unit-Testing-CSLA-Rules-With-Rocks)

## Feedback

If you run into any issues, please add them [here](https://github.com/JasonBock/Rocks/issues).
