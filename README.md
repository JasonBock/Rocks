# Rocks

A mocking library based on the Compiler APIs (Roslyn + Mocks)

## Getting Started

Reference the `Rocks` [NuGet package](https://www.nuget.org/packages/Rocks) - that's it.

### Prerequisites

The Rocks package targets .NET Standard 2.0 for host flexibility. Note that Rocks looks for, and generates, code that targets .NET 7.

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
var expectations = Rock.Create<IAmSimple>();
expectations.Methods().TargetAction();

var mock = expectations.Instance();
mock.TargetAction();

expectations.Verify();
```

More details can be found on the [Quickstart page](https://github.com/JasonBock/Rocks/blob/main/docs/Quickstart.md).

## Additional Documentation

* [Discord Server](https://discord.com/channels/1035376645864955974/1035376646326321194)
* [Changelog](https://github.com/JasonBock/Rocks/blob/main/changelog.md)
* [Unit testing best practices with .NET Core and .NET Standard](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
* [BenchmarkMockNet](https://github.com/ecoAPM/BenchmarkMockNet)

## Feedback

If you run into any issues, please add them [here](https://github.com/JasonBock/Rocks/issues).