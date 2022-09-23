So, after creating the console app and adding some other packages, I have some work to do.

## Fixes
* For makes, any `Task<>` or `ValueTask<>` return types are marked with the null-forgiving operator when the return type is not nullable.
* For makes, `[AllowNull]` now shows up on properties when the base property has it defined.
* Constraint ordering has been fixed in certain cases (it was failing when there were `struct` and interface constraints)
* Method matching now correctly handles cases where the either parameters or return types use generic type parameters.
* If an interface has static abstract members, Rocks will raise the `InterfaceHasStaticAbstractMembersDiagnostic` (`ROCK7`).

## Unsolved Issues

### ADDED - Type Names in Multiple Interfaces

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

### ADDED - Including Types From Constraints

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

### ADDED - Nested Types and Properties

Doing this with Microsoft.CodeAnalysis:

```csharp
using Microsoft.CodeAnalysis.FlowAnalysis;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<ICaughtExceptionOperation>();
	}
}
```

Gives an error when the mock implements the `ChildOperations` property. The type is `IOperation.OperationList`, but the code just generated `IOperation`. I thought I handled nested type names, but...maybe I didn't with property types?

### ADDED - Missing Namespaces From Types From Properties

Doing this with AutoMapper:

```csharp
using AutoMapper;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<PathMap>();
	}
}
```

The `MethodInfo[]` property type from `SourceMembers`, the namespace isn't included. I just need to make sure when I use fully-qualified names to ensure I'm including property types. Though the `GetNamespaces()` extension method on `IPropertySymbol` **should** be including the namespace. Looks like this:

```csharp
namespaces.Add(self.ContainingNamespace);
```

in `GetNamespaces()` for `ITypeSymbol`, that returns `null`. Though the type name is recognized as `System.Reflection.MethodInfo`.

Ah, it's because it's an array:

```csharp
?self.TypeKind == TypeKind.Array
true

?(self as IArrayTypeSymbol).ElementType.ContainingNamespace
{System.Reflection}
```

This feels like when I need to get the element type from a `Type` in Reflection. But I don't know of a way to get to the "root" element type with symbols. I'd want to include this with "the type problem" story so that I get the right name for arrays.

To reproduce this:

```csharp
using Rocks;
using System;
using System.Reflection;

public interface IUseMethodInfo
{
	MethodInfo[] Methods { get; }
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<IUseMethodInfo>();
	}
}
```

### ADDED - Missing Namespace For Parameter Types

Doing this with Serilog:

```csharp
using Serilog.Formatting.Json;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<JsonFormatter>();
	}
}
```

A couple of types from `WriteRenderings()`, like `IGrouping<string, PropertyToken>[]`, the namespaces aren't being generated. I'm guessing this is the problem I mentioned above with "Missing Namespaces From Types From Properties". This should be included with "The Type Problem" issue.

### ADDED - Constraints are Not Carried to Extension Methods

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

The issue is, `TSourcePixel` isn't defined as a generic parameter anywhere.

### ADDED - Creating Too Many Constraints

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

### ADDED - Internal Virtual Members Prevent a Mock From Being Created

Doing this with Microsoft.CodeAnalysis:

```csharp
using Microsoft.CodeAnalysis;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<Diagnostic>();
	}
}
```

The resulting mock misses three members: `WithSeverity()`, `WithIsSuppressed()`, and `WithLocation()`. It can't do this because they're `internal`, so they're not accessible. But...this means that if we find inaccessible members that are abstract, it can't be mocked. Trying to do this in VS:

```csharp
public class MyDiagnostic : Microsoft.CodeAnalysis.Diagnostic
```

and tell the "Implement Abstract Class" code fix will create these three methods, but the resulting code is incorrect.

To reproduce this:

```csharp
// Note that this has to be compiled into its' own assembly
public abstract class InternalTargets
{
	public abstract void VisibleWork();
	internal abstract void Work();
}

// This class has to be in a different assembly.
public static class Test
{
	public static void Test() => Rock.Create<InternalTargets>();
}
```

### ADDED - Obsolete Types in Member Definitions and Extension Methods

Doing this with Microsoft.CodeAnalysis:

```csharp
using Microsoft.CodeAnalysis.Operations;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<OperationWalker>();
	}
}
```

Rocks overrides `VisitCollectionElementInitializer()`, which is fine as it correctly adds the `[Obsolete]` attribute. However, the extension method for `MethodExpectations<OperationWalker>` doesn't include the attribute, so we get a compiler error (`CS0619`). Seems like the way to fix this is similar to what I reported in "Including Types From Constraints" in that the attributes have to come along with the ride as well. That, or if the member is `virtual` but not `abstract`, just ignore it.

To reproduce this:

```csharp
[Obsolete("Don't use", error: true)]
public class DoNotUse { }

// Rocks should ignore Foo
public interface IHaveObsolete
{
    void Foo(DoNotUse doNotUse);
    void Bar();
}
```

### ADDED - Conflict With Variable Names and Member Parameters

Doing this with Castle.Core:

```csharp
using Castle.Components.DictionaryAdapter;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<DynamicDictionary>();
	}
}
```

Creates overrides for methods like `TryInvokeMember()` that have a parameter called `result`. This conflicts with Rocks generating a variable called `result` to house the return value. I need to ensure any variables generated in a member do not conflict with any parameters or return values (not sure if a tuple return with named values are included or not, I don't think so, but want to check to make sure).

I think this would also have to be done for the `self` parameter used in extension methods, as well as the `constructorProperties` named used in `Instance()` for `required` and `init` properties.

To reproduce this:

```csharp
public interface IHaveNamingConflicts
{
	// If there are any other variables that Rocks create, include them as parameters.
	int Foo(string methodHandlers, string methodHandler, string result);
}
```

### ADDED - Target Type Uses Reserved Language Keywords

Doing this with Mono.Cecil:

```csharp
using Mono.Cecil;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<TypeReference>();
	}
}
```

One of the constructors for `TypeReference` has a parameter with the name `namespace`. This is legal, but to declare in C# requires the `@` token in front of the name. Rocks isn't looking at the parameter names to see if they are a reserved keyword like `namespace` or `event`, and if they are, the `@` sign needs to be put in front.

To reproduce this:

```csharp
public interface IUseKeyword
{
    void Foo(string @namespace, string @event, string @property);   
}
```

### ADDED - Emitting "Invalid" Attribute

Doing this with System.Threading.Channels:

```csharp
using System.Threading.Channels;
using Rocks;
using System;

public interface IUseMethodInfo
{
	MethodInfo[] Methods { get; }
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<ChannelReader<object>>();
	}
}
```

The `ReadAllAsync()` has an `[AsyncIteratorStateMachine]` attribute, but this shouldn't be emitted in the mock, similar to other attributes that are ignored in `AttributeDataExtensions.GetDescription()`, like `IteratorStateMachineAttribute`.

To reproduce this:

```csharp
#nullable enable

public class HasAsyncIteratorStateMachine<T>
{
    private ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(true);
	
    private bool TryRead([MaybeNullWhen(false)] out T item)
    {
        item = default!;
        return true;
    }
    
    public virtual async IAsyncEnumerable<T> ReadAllAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await WaitToReadAsync(cancellationToken).ConfigureAwait(false))
        {
            while (TryRead(out T? item))
            {
                yield return item;
            }
        }
    }
}
```

### ADDED - Overgenerating Constraints

Doing this with EntityFramework:

```csharp
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<PropertyBuilder<object>>();
	}
}
```

This overrides methods like `HasValueGenerator()` as follows:

```csharp
public override PropertyBuilder HasValueGenerator<TGenerator>()
	where TGenerator : ValueGenerator
```

But according to `CS0460`, the constraints shouldn't be included: "Constraints for override and explicit interface implementation methods are inherited from the base method, so they cannot be specified directly, except for either a 'class', or a 'struct' constraint."

To reproduce this:

```csharp
public class PropertyBuilder { }
public class ValueGenerator { }

public class HasConstraints<T>
{
	public virtual PropertyBuilder HasValueGenerator<TGenerator>()
		 where TGenerator : ValueGenerator => default!;
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<HasConstraints<object>>();
	}
}
```

### ADDED - Duplicating Overrides From `new` Methods

Doing this with EntityFramework:

```csharp
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<PropertyBuilder<object>>();
	}
}
```

Ends up generating "duplicate" methods, like this:

```csharp
[MemberIdentifier(3, "PropertyBuilder HasAnnotation(string annotation, object? value)")]
	public override PropertyBuilder HasAnnotation(string annotation,  object? value)
	
[MemberIdentifier(34, "PropertyBuilder<object> HasAnnotation(string annotation, object? value)")]
	public override PropertyBuilder<object> HasAnnotation(string annotation,  object? value)
```

I think this is because the base type, `PropertyBuilder`, defines this method:

```csharp
public virtual PropertyBuilder HasAnnotation(string annotation, object? value)
```

The inheriting type, `PropertyBuilder<TProperty>`, creates a `new` method:

```csharp
public new virtual PropertyBuilder<TProperty> HasAnnotation(string annotation, object? value)
```

What the mock type should generate is just the one method from the generic class, not both of them. So I'm guessing I need logic to see if a method is "shadowing" a base one via `new`.

To reproduce this:

```csharp
#nullable enable

public class PropertyBuilder
{
    public virtual PropertyBuilder HasAnnotation(string annotation, object? value) => default!;
}

public class PropertyBuilder<TProperty> : PropertyBuilder
{
    public new virtual PropertyBuilder<TProperty> HasAnnotation(string annotation, object? value) => default!;
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<PropertyBuilder<object>>();
	}
}
```

### ADDED - Odd Issue with `Microsoft.EntityFrameworkCore.Metadata.INavigation`

Doing this with EntityFramework:

```csharp
using Microsoft.EntityFrameworkCore.Metadata;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<INavigation>();
	}
}
```

Results in some very wrong mock code. I don't know what the exact problem is right now, but this interface is definitely giving Rocks fits. Note that `IConventionProperty` yields similar results.

### ADDED - Types Within Delegates Do Not Show Up in Usings

Doing this with RestSharp:

```csharp
using RestSharp;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<RestRequest>();
	}
}
```

Gives this error:

```
error CS0029: Cannot implicitly convert type 'System.Func<Stream, Stream?>' to 'System.Func<System.IO.Stream, System.IO.Stream?>'
```

It's when the `ConstructorProperties` object tries to do this in `Instance()`:

```csharp
ResponseWriter = constructorProperties.ResponseWriter,
```

It's happening because the `ResponseWriter` property isn't virtual; it's only being included because it's an `init` and therefore it becomes a part of `ConstructorProperties`. But then, Rocks isn't analyzing it enough to include the "usings" within the delegate.

To reproduce this:

```csharp
using Rocks;
using System;
using System.IO;

public class IHaveDelegate
{
	public Func<Stream, Stream> Processor { get; init; }   
	public virtual void Foo() { }
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<IHaveDelegate>();
	}
}
```

### ADDED - Emitting the `[Dynamic]` Attribute

Doing this with CsvHelper:

```csharp
using CsvHelper.Expressions;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<DynamicRecordCreator>();
	}
}
```

`CreateDynamicRecord()` returns `dynamic`, and Rocks end up generating `[return: Dynamic]` on the method, which gives this error:

```
error CS1970: Do not use 'System.Runtime.CompilerServices.DynamicAttribute'. Use the 'dynamic' keyword instead.
```

For some reason, even though it doesn't look like the source code uses the attribute, the method ends up with the attribute on it. We shouldn't generate it in Rocks.

To reproduce this:

```csharp
using Rocks;
using System;
using System.Runtime.CompilerServices;

public class HaveDynamic
{
	[return: Dynamic]
	protected virtual dynamic CreateDynamicRecord() => default;
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<HaveDynamic>();
	}
}
```

### ADDED - Async Iterators Aren't Implemented Correctly

Doing this with CsvHelper:

```csharp
using CsvHelper;
using Rocks;
using System;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<CsvReader>();
	}
}
```

There are methods that return `IAsyncEnumerable<>`, like `GetRecordsAsync<T>`, and essentially this means the implementing method must do the async iteration "correctly". Something like:

* The method must be `async`.
* Since it's `async`, there must be an `await` in the method body. The simplest way to do this is emit `await Task.CompletedTask;` as the first line.
* The `return` statements (even if the code calls `base`) need to change to `yield return`.

To reproduce this:

```csharp
using Rocks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

public class AsyncEnumeration
{
	public virtual async IAsyncEnumerable<string> GetRecordsAsync([EnumeratorCancellation]CancellationToken cancellationToken = default(CancellationToken))
	{
		await Task.CompletedTask;
		yield return "x";
	}
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<AsyncEnumeration>();
	}
}
```

### Missing Null Annotation With Null Default Value For Parameter

Doing this with IdentityModel.Client:

```csharp
using Rocks;
using System;
using IdentityModel.Client;

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<JsonWebKeySetResponse>();
	}
}
```

`InitializeAsync()` has a parameter, `object initializationData = null`. Doing a "Go To Definition" doesn't show the parameter with a "?", though it really should be. It's OK in an override to change it to have `?`. So, the fix seems to be that if a parameter is optional, and the default value is `null`, it should be annotated with `?`.

To reproduce this:

```csharp
using Rocks;
using System;

public class NeedsNullable
{
	protected virtual void Initialize(object initializationData = null) { }
}

public static class Test
{
	public static void Go()
	{
		var expectations = Rock.Create<NeedsNullable>();
	}
}
```