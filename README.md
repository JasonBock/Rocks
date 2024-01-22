# Rocks

A mocking library based on the Compiler APIs (Roslyn + Mocks)

## Getting Started

Reference the `Rocks` [NuGet package](https://www.nuget.org/packages/Rocks) - that's it.

### Prerequisites

The Rocks package targets .NET Standard 2.0 for host flexibility. Note that Rocks looks for, and generates, code that targets .NET 8.

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
[assembly: RockCreate<IAmSimple>]

var expectations = new IAmSimpleCreateExpectations();
expectations.Methods.TargetAction();

var mock = expectations.Instance();
mock.TargetAction();

expectations.Verify();
```

More details can be found on the [Overview page](https://github.com/JasonBock/Rocks/blob/main/docs/Overview.md).

## Additional Documentation

* [Discord Server](https://discord.gg/ZXMhkKsMRb)
* [Changelog](https://github.com/JasonBock/Rocks/blob/main/changelog.md)
* [Unit testing best practices with .NET Core and .NET Standard](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
* [BenchmarkMockNet](https://github.com/ecoAPM/BenchmarkMockNet)

## Feedback

If you run into any issues, please add them [here](https://github.com/JasonBock/Rocks/issues).
