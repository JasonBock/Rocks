# Rocks

A mocking library based on the Compiler APIs (Roslyn + Mocks)

## Overview

There are great mocking libraries out there, like [Moq](https://github.com/moq/moq) and [NSubstitute](http://nsubstitute.github.io/), so why did I decide to create YAML (yet another mocking library) in 2015? There are essentially two reasons.

The first reason relates to how code generation was done with mocking libraries. Most (if not all) used an approach that ends up using `System.Reflection.Emit`, which requires knowledge of IL. This is not a trivial endeavour. Furthermore, the generated code can't be stepped into during a debugging process. I wanted to write a mocking library with the new Compiler APIs (Roslyn) to see if I could make the code generation process for the mock much easier and allow a developer to step into that code if necessary.

The other reason was being able to pre-generate the mocks for a given assembly, rather than dynamically generate them in a test. This is what the [Fakes library](https://docs.microsoft.com/en-us/visualstudio/test/code-generation-compilation-and-naming-conventions-in-microsoft-fakes?view=vs-2019) can do, but I wanted to be able to do it where I could easily modify a project file and automatically generate those mocks.

This is what Rocks can do. Mocks are created by generating C# code on the fly and compiling it with the Compiler APIs. This makes it trivial to step into the mock code. Before the 5.0.0 version, this code generation step took place at runtime, but with source generators in C#9, this generation happens as soon as you state that you want to create a mock of a particular type. So, feel free to test Rocks out, and see what you think. Even if you don't use it as your primary mocking library, you may see just how easy it to generate code on the fly with the new Compiler APIs. Enjoy!

## Tutorial

To make a mock, you take an interface or an unsealed class that has virtual members:

```
public interface IAmSimple
{
  void TargetAction();
}
```

and you use Rocks to create a mock with expectations, along with verifying its usage:

```
var rock = Rock.Create<IAmSimple>();
rock.Methods().TargetAction();

var chunk = rock.Make();
chunk.TargetAction();

rock.Verify();
```

More details can be found on the [Quickstart page](https://github.com/JasonBock/Rocks/blob/main/docs/Quickstart.md). Note that if you build the code locally, you'll need to build in `Release` mode for the package reference in `Rocks.NuGetHost` to resolve correctly (or unload that project from the solution as it's optional and delete `nuget.config`).
