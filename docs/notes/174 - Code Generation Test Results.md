So, after creating the console app and adding some other packages, I have some work to do.

## Fixes
* For makes, any `Task<>` or `ValueTask<>` return types are marked with the null-forgiving operator when the return type is not nullable.
* For makes, `[AllowNull]` now shows up on properties when the base property has it defined.
* Constraint ordering has been fixed in certain cases (it was failing when there were `struct` and interface constraints)
* Method matching now correctly handles cases where the either parameters or return types use generic type parameters.
* If an interface has static abstract members, Rocks will raise the `InterfaceHasStaticAbstractMembersDiagnostic` (`ROCK7`).

## Unsolved Issues

### Type Names in Multiple Interfaces

In CSLA, if `BusinessBase<>` is mocked, an error occurs where `SerializationInfo` is used, and both `Csla.Serialization.Mobile.SerializationInfo` and `System.Runtime.Serialization.SerializationInfo` show up in `using` statements. This is essentially ["the type problem"](https://github.com/JasonBock/Rocks/issues/129), where I need to decide if I stop doing `using` and use fully-qualified type names everywhere.

To reproduce this:

```csharp
namespace Namespace1
{
  public class Thing { }
  public class Stuff { }
}

namespace Namespace2
{
  public class Thing { }
}

public interface IUsesThing
{
  void Use(Namespace2.Thing thing, Namespace1.Stuff stuff);
}

var expectations = Rock.Create<IUsesThing>();
```

### Carrying Constraints to Extension Methods

In Moq, mocking `Mock<>` creates this error:

```
error CS0452: The type 'TInterface' must be a reference type in order to use it as parameter 'T' in the generic type or method 'Mock<T>'
```

This is because the `As<TInterface>` method has a constraint on it, but when the `MethodExpectations<>` extension method is made for `As<TInterface>`, the constraints aren't there. I need to copy the constraints from methods into the extension methods.

To reproduce this:

```csharp
public class Thing { }

public abstract class Thing<T> : Thing where T : class
{
    public abstract Thing<TTarget> As<TTarget>() where TTarget : class;
}

var expectations = Rock.Create<Thing<object>>
```

### Delegate Creation for Ref Structs

Doing this with ImageSharp:

```csharp
using SixLabors.ImageSharp.PixelFormats;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<PixelOperations<A8>>();
	}
}
```

Creates a delegate like this:

```csharp
public delegate bool ArgEvaluationForReadOnlySpanOfTSourcePixel(ReadOnlySpan<TSourcePixel> value);
```

The issue is, `TSourcePixel` is defined as a generic parameter anywhere.

### Including Types From Constraints

Doing this with ImageSharp:

```csharp
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<IPixelSamplingStrategy>();
	}
}
```

Generates the `IPixel<TPixel>` constraint in `EnumeratePixelRegions<TPixel>`. However, `IPixel<TPixel>` is in the `SixLabors.ImageSharp.PixelFormats` namespace, and right now Rocks isn't looking at types in constraint to include their namespaces in `NamespaceGatherer`. Whether I decide to fully-qualify all type names to eliminate `NamespaceGatherer` or not, I need to ensure these types are resolved.

### Creating Too Many Constraints

Doing this with ImageSharp:

```csharp
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<IPixelSamplingStrategy>();
	}
}
```

Creates the `where TPixel : unmanaged, struct, IPixel<TPixel>` constraint syntax, but this is illegal. `EnumeratePixelRegions<TPixel>` has `where TPixel : unmanaged, IPixel<TPixel>` in the source code. The `ITypeParameterSymbol` for `TPixel` ends up having `HasUnmanagedTypeConstraint` and `HasValueTypeConstraint`, but I'm guessing that if the `unmanaged` constraint exists, you don't need to look at `HasValueTypeConstraint`.

I thought I had fixed this with an example that was `unmanaged` and an interface type. So I'm not sure why this is happening right now.

To reproduce this:

```csharp
public interface IValue<TValue>
  where TValue : IValue<TValue> { }

public class Value<TValue>
  where TValue : unmanaged, IValue<TValue> { }

public interface IUnmanagedValue
{
    void Use<TValue>(Value<TValue> value)
        where TValue : unmanaged, IValue<TValue>;
}
```