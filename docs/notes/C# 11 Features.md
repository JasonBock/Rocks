# C# 11 Features

This document contains notes on how I'll handle C# 11 features in Rocks. This doesn't cover all the C# 11 features; I'm only focused on those that I think will have significant impact on how Rocks works. For example, static abstract members in interfaces will require me to do **something** with those members - otherwise, I'll generate invalid code.

## Static Abstract Members in Interfaces

This is probably the feature that has the most impact, so I should handle this one first.

This feature is pretty much self-defining. You can do this:
```csharp
public interface ICreatableAsync<TSelf>
{
  public static abstract ValueTask<TSelf> CreateAsync();
}

public interface IEmpty<TSelf>
{
  public static abstract TSelf Empty { get; }
}

public interface IAdditionOperators<TSelf, TOther, TResult> 
  where TSelf : IAdditionOperators<TSelf, TOther, TResult>
{
  public static abstract TResult operator +(TSelf left, TOther right);
}
```

You can define methods, properties, **and** operators in interfaces like this. The operator example come from the [experimental generic math](https://devblogs.microsoft.com/dotnet/dotnet-7-generic-math/) work that's been done for C# 11.

So, how do we handle this?

Right now, Rocks returns an instance of a mock type:

```csharp
var expectations = Rock.Create<IThing>();
//...
var mock = expectations.Instance();
```

But static members can't follow the same approach. They **have** to be implemented, but a static member can't access the expectations that are stored in the instance field in the mock object. If I come up with a way to allow a developer to specify an expectation, these are static members, and they are accessible anywhere in an `AppDomain`. I'd have to come up with a way to allow a set of expectations to be done that are **isolated** from any other set.

So, there's essentially two options. One is to handle static members as "makes" - meaning, a developer won't be able to set any expectations for them, similar to what `Rock.Make<>()` does. This is a simple thing to do, and it ... "works", but it feels weak. Is there a way to make expectations on static members?

Well, this has already been done by some libraries (commercial and free), like [TypeMock Isolator](https://www.typemock.com/TypemockIsolatorExamples#staticMethod) and [Fakes](https://docs.microsoft.com/en-us/visualstudio/test/using-shims-to-isolate-your-application-from-other-assemblies-for-unit-testing). In both cases, there is a "context" that is set up where the static member is switched. Under the scenes, they do things with Profiler API to change out the implementation. Rocks doesn't need to do anything that extreme, but the general context/scope content is valid.

We also need to be concerned about parallel execution, if tests are running in parallel. Because these are static members, we need to use a static field to store those expectations such that the static members can look them up and evaluate them. If tests are running in a synchronous manner, switching them in and out keeps them isolated, but across multiple threads, we can easily run into race conditions. I think the key is to use `AsyncLocal<>` for the static field. I did a POC, and it seems like having a static `AsyncLocal<>` field will do the trick. Note that this isn't just for `async` methods. It's to keep the context correct, so for a static member, this should be seamless.

If there are any static abstract members, we need to create a `Statics()` method, similar to `Instance()`. It doesn't return anything; it just sets an  `AsyncLocal<>` value in the mock to the static set of expectations. If it is already "set", that means a previous set of expectations were not verified, and an exception should be thrown. When `Verify()` is called, it will set that field to `null`, along with checking all expectations, both static and instance. 

```csharp
public interface IThing
{
	static abstract void StaticTest();
	void InstanceTest();	
}

var expectations = Rock.Create<IThing>();
expectations.Methods().InstanceTest();
expectations.Methods().StaticTest();

expectations.Statics();
var mock = mockStatics.Instance();

RockIThing.StaticTest();
mock.InstanceTest();

expectations.Verify();
```

This makes it even more important to call `Verify()` in a test.

Maybe create an `Initialize()` method (or use a "better" word, the concept is what matters), instead of having both `Statics()` and `Instance()` methods. If the target type has instance members, this would return an instance of the mock, otherwise, it returns `void`. This would be a breaking change, in that there would no longer be an `Instance()` method. But this "feels" better. However, for makes, this doesn't make sense. I'm not sure there's a need to have a make for statics, but an interface can have statics and instance methods, so a make would have to do something with it anyway. Maybe `Rock.Make<>()` does the same thing as `Initialize()` and I remove the `Instance()` method altogether.

Static members shouldn't need different handling with adornments, but I'll need to add `AddStatic()` methods to `ExpectationsWrapper<>` to differentiate expectations between static and instance members. Members that are static, the gen'd code will need to call this method instead of `Add()`. So this:

```csharp
internal static MethodAdornments<IInterfaceMethodVoidWithEvents, Action> NoParameters(this MethodExpectations<IInterfaceMethodVoidWithEvents> self) =>
	new MethodAdornments<IInterfaceMethodVoidWithEvents, Action>(self.Add(0, new List<Argument>()));
```

Will change to this:

```csharp
internal static MethodAdornments<IInterfaceMethodVoidWithEvents, Action> NoParameters(this MethodExpectations<IInterfaceMethodVoidWithEvents> self) =>
	new MethodAdornments<IInterfaceMethodVoidWithEvents, Action>(self.AddStatic(0, new List<Argument>()));
```

When I generate the mock, I'll know if I ever generated static members, so in mock constructors, I can have a static `AsyncLocal<>` field, `staticHandlers`, generated and set. In the static member, I just do

```csharp
if (MockTypeName.staticHandlers.Value.TryGetValue(0, out var methodHandlers))
```

Note that now an interface can only have static abstract members. In that case, we should not generate any `Instance()` methods.

Raising events...right now, `IMockWithEvents` is for instance events, so we may need something that allows you to raise static events like this:

```csharp
public interface IMockWithEvents
	: IMock
{
	void Raise(string eventName, EventArgs args);
}

public interface IMockWithStaticEvents
	: IMock
{
	static abstract void Raise(string eventName, EventArgs args);
}
```

I can explicitly implement one or both, and I shouldn't have a name collision. Then I can call on the handler `RaiseEvents()`, this overload doesn't take a "this" reference. That will end up calling the `IMockWithStaticEvents.Raise()` that's implemented to raise a static event.

Operators are another issue. I'm thinking I'd have an `Operators()` name like I do with `Methods()` and `Properties()`. However, I can't use the name of the operator, because they're not valid names (e.g. `+`). I'll probably have to keep a map of operators to well-understood names (e.g. `+` would be `Add`). I'm not sure how the generics will be resovled either - should I just set them to the name of the mock, or let the user pick a type?

## Generic Attributes

In C# 11, generic attributes will be a thing. For Rocks, the main concern is that I'm generating the right type information when an attribute has to be included on a member. My hope is that it'll be simple to verify:

```csharp
public sealed class MyGenericAttribute<T>
	: Attribute { }
	
public interface IUseClosedGenericAttribute
{
	[MyGeneric<int>]
	void Test();
}

// Hopefully this just works.
var expectations = Rock.Create<IUseClosedGenericAttribute>();
```

Note that you can't have open generic attributes like this:
```csharp
public interface IUseOpenGenericAttribute<Q>
{
	[MyGeneric<Q>]
	void TestClassGeneric();
	
	[MyGeneric<T>]
	void TestMethodGeneric<T>();
}
```

The first declaration gives this:

```csharp
error CS8968: 'Q': an attribute type argument cannot use type parameters
```

The second one has this error:

```csharp
error CS0246: The type or namespace name 'T' could not be found (are you missing a using directive or an assembly reference?)
```

I don't see Rocks needing to **use** generic attributes itself right now. It just needs to emit the name of these attributes correctly.

## Required Members

These are some random ideas around both `required` and `init` properties and constructors. There are similarities with these, so that's why I'm including both.

### `init` Properties

`init` is a C# 10 feature, but because of `required`, I'm revisiting the approach. Right now, I have `init` properties unmockable. I'd like to rectify that.

For `init` properties, would this work?

```csharp
public interface ITest
{
	int A { get; init; }
	string B { get; init; }
	Guid C { get; init; }
}

public ITest Instance(int AProperty = default, string BProperty = default, Guid CProperty = default) =>
	new MockITest() { A = AProperty, B = BProperty, C = CProperty };

var chunk = rock.Instance(BProperty: "value");
```

This way you can choose which properties to initialize, if the caller doesn't provide a value, they'll just be the default anyway.

I think this is legal syntax. Now, if constructors are involved:

```csharp
public class Test
{
	public Test(string value) { }
	
	public virtual int A { get; init; }
	public virtual string B { get; init; }
	public virtual Guid C { get; init; }
}

public Test Instance(string value, int AProperty = default, string BProperty = default, Guid CProperty = default) =>
	new MockTest(value) { A = AProperty, B = BProperty, C = CProperty };
```

I can see all sorts of issues popping up with name collisions, and what about properties that aren't mockable but do have inits that are accessible? Would we want to include those as well in the parameter list?

I tried a POC:

```csharp
static Test Instance(string value, int AProperty = default, string? BProperty = default, Guid CProperty = default) =>
	new Test(value) { A = AProperty, B = BProperty, C = CProperty };

var test1 = Instance("a", AProperty: 4);
Console.WriteLine(test1);

var test2 = Instance("a", BProperty: "B value", CProperty: Guid.NewGuid());
Console.WriteLine(test2);

public class Test
{
	public Test(string value) { }

	public virtual int A { get; init; }
	public virtual string? B { get; init; }
	public virtual Guid C { get; init; }

	public override string ToString() =>
		$"A = {this.A}, B = {this.B}, C = {this.C}";
}
```

And this gives:

```csharp
A = 4, B = , C = 00000000-0000-0000-0000-000000000000
A = 0, B = B value, C = cbdf9c87-881f-41a0-8db6-2920b06436fd
```

So...there's hope. But this would mean I would have to put a bunch of stuff back in from issue #158 :|. Well, maybe. We may not need to have expectations on its initialization, but we may need to have some of the props (esp. ones that can't be mocked) set to initial values.

But...I came up with an idea with `required` properties, and I think it can be applied to `init` properties. Read on...

### `required` Properties

Required properties have been proposed for C# 11 and it looks like it's going to make it as a feature. We need to be concerned with this, because, while properties on interfaces cannot be marked with `required`, an `abstract` class, or a class with `virtual` properties can be `required`, and then an override **must** be `required` as well:

```csharp
using System;

public abstract class HaveRequiredProperties 
{
    public abstract string Data { get; set; }    
    public abstract required Guid Id { get; set; }
}

public class RequiredProperties
    : HaveRequiredProperties
{
    public override string Data { get; set; }    
    public required override Guid Id { get; set; }
}

var rp = new RequiredProperties { Id = Guid.NewGuid() };
```

This means I need a way to set all these properties in the `Instance()` calls.

Here's the idea. I create a nested class called `RequiredPropertyValues`:

```csharp
namespace Rocks.IntegrationTests
{
	internal static class CreateExpectationsOfRequiredPropertiesExtensions
	{
		internal sealed class RequiredPropertyValues
		{
			internal required Guid Id { get; set; }		
		}

		internal static RequiredProperties Instance(this Expectations<RequiredProperties> self, RequiredPropertyValues requiredPropertyValues)
		{
			if (self.Mock is null)
			{
				var mock = new RockRequiredProperties(self) { Id = requiredPropertyValues.Id };
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException("Can only create a new mock once.");
			}
		}
	}
	
	// More mock stuff is generated here...
```

This type name would always be the same, and the parameter name would always be camel-cased of it. The odds of this running into a name collision would be really small. If this **does** happen, I can always create some kind of configuration for Rocks where one of the values could be `RequiredPropertyValuesClassName`. But again, I don't see the need for this right now.

The generated code simply includes all the required property definitions into `RequiredPropertyValues`, and then adds a last parameter to every `Instance()` method. This will force the user to specify the required parameters. In turn, these values would be mapped to the mock instance.

If a constructor has any optional parameters, this `RequiredPropertyValues` parameter has to go first. This is a corner-case, but it has to be done that way. In fact, we can put the required properties parameter after the `this` value, and then put the constructor parameters. That eliminates the issue.

I'm thinking that maybe the "right" thing to do is to always put the required property class right after `self`, and then put any constructor parameters after that. That's consistent and eliminates any issue with `params` parameters.

This idea could also be done for `init` properties. Create an `InitPropertyValues`, which allows a user to set these properties on construction. I'm still keeping `init` properties from being verifiable, as you know it has to be called, but I'm open to changing my mind on this.

If a property is both `init` and `required`, it would be `required`. In other words, it would be handled by the generated `RequiredPropertyValues` class.

## Raw String Literals

This feature won't affect mock execution, but it can be extremely useful for code generation. I've used it for [StaticCast](https://github.com/jasonbock/staticcast) and it works really well. I create a `WriteLines()` extension method for `IndentedTextWriter` there, and it make it easy to put snippets of multi-line text into an `IndentedTextWriter` (though it may not be entirely efficient). Definitely should look at using raw string literals as much as possible.

## File-Local Types

[This feature](https://github.com/dotnet/csharplang/issues/6011) wasn't one that was on my radar until recently. What's interesting about this one is that it may make it easier for projected types. Right now I'm creating a namespace for all these types. If I make them `file` types, then there should not be a reason to make that namespace. Not sure that this will be in for C# 11.