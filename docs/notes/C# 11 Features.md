# C# 11 Features

This document contains notes on how I'll handle C# 11 features in Rocks. This doesn't cover all the C# 11 features; I'm only focused on those that I think will have significant impact on how Rocks works. For example, static abstract members in interfaces will require me to do **something** with those members - otherwise, I'll generate invalid code.

## Static Abstract Members in Interfaces

This feature is pretty much self-defining. You can do this:
```
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

You can define methods, properties, **and** operators in interfaces like this. The operator example come from the [experimental generic math](https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/) work that's been done for C# 11.

So, how do we handle this?

Right now, Rocks returns an instance of a mock type:

```
var mock = Rock.Create<IThing>();
//...
var mockInstance = mock.Instance();
```

But static members can't follow the same approach. They **have** to be implemented, but a static member can't access the expectations that are stored in the instance field in the mock object. If I come up with a way to allow a developer to specify an expectation, these are static members, and they are accessible anywhere in an `AppDomain`. I'd have to come up with a way to allow a set of expectations to be done that are **isolated** from any other set.

So, there's essentially two options. One is to handle static members as "makes" - meaning, a developer won't be able to set any expectations for them, similar to what `Rock.Make<>()` does. This is a simple thing to do, and it ... "works", but it feels weak. Is there a way to make expectations on static members?

Well, this has already been done by some libraries (commercial and free), like [TypeMock Isolator](https://www.typemock.com/TypemockIsolatorExamples#staticMethod) and [Fakes](https://docs.microsoft.com/en-us/visualstudio/test/using-shims-to-isolate-your-application-from-other-assemblies-for-unit-testing). In both cases, there is a "context" that is set up where the static member is switched. Under the scenes, they do things with Profiler API to change out the implementation. Rocks doesn't need to do anything that extreme, but the general context/scope content is valid.

We also need to be concerned about parallel execution. If tests are running in parallel. Because these are static members, we need to use a static field to store those expectations such that the static members can look them up and evaluate them. If tests are running in a synchronous manner, switching them in and out keeps them isolated, but across multiple threads, we can easily run into race conditions. I think the key is to use `AsyncLocal<>` for the static field. I did a POC, and it seems like having a static `AsyncLocal<>` field will do the trick. Note that this isn't just for `async` methods. It's to keep the context correct, so for a static member, this should be seamless.

I think I can create a context on the fly (or maybe I predefine this in Rocks), `ExpectationsContext<TTargetType>` that implements `IDisposable` and has an `.Instance()` method to get an instance **if** the target type has instance members. Once `Instance()` is called, it cannot be called again for those set of expectations, just like Rocks works now. The static expectations are housed in a private static `AsyncLocal<>` if any exist. If it is already "set", that means a previous set of expectations were not verified. When `Dispose()` is called, it sets the static `AsyncLocal<>` to `null`, and it calls `Verify()` on the expectations. If `ExpectationsContext<TTargetType>` is defined in Rocks, I can make `Verify()` `internal`. This would be a breaking change, but it would force uses to change to getting the context. This means that the developer no longer needs to explicitly call `Verify()`

Code analysis will help here to make sure you get the context and dispose it correctly:

```
var expectations = Rock.Create<IThing>();
expectations.Methods().Test();
using var mockContext = expectations.Context();
var instance = mockContext.Instance();
```

It's one more line to get the context, but a call to `Verify()` is no longer needed.

I'll need to add AddStatic() methods to ExpectationsWrapper<>. Members that are static, the gen'd code will need to call this method instead of Add(). So this:

	internal static MethodAdornments<IInterfaceMethodVoidWithEvents, Action> NoParameters(this MethodExpectations<IInterfaceMethodVoidWithEvents> self) =>
	 new MethodAdornments<IInterfaceMethodVoidWithEvents, Action>(self.Add(0, new List<Argument>()));
	 
Will change to this:

	internal static MethodAdornments<IInterfaceMethodVoidWithEvents, Action> NoParameters(this MethodExpectations<IInterfaceMethodVoidWithEvents> self) =>
	 new MethodAdornments<IInterfaceMethodVoidWithEvents, Action>(self.AddStatic(0, new List<Argument>()));

When I generate the mock, I'll know if I ever generated static members, so in mock constructors, I can have a static AsyncLocal<> field, "staticHandlers", generated and set. In the static member, I just do

	if (ClassName.staticHandlers.Value.TryGetValue(0, out var methodHandlers))
	
RaiseEvents...that's taking an object instance, so we may need something that allows you to raise static events (?)

IMockWithEvents...

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

I can explicitly implement one or both, and I shouldn't have a name collision. Then I can call on the handler RaiseEvents(), this overload doesn't take a "this" reference. That will end up calling the IMockWithStaticEvents.Raise() that's implemented to raise a static event.

## Generic Attributes

In C# 11, generic attributes will be a thing. For Rocks, the main concern is that I'm generating the right type information when an attribute has to be included on a member. My hope is that it'll be simple to verify:

```
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
```
public interface IUseOpenGenericAttribute<Q>
{
	[MyGeneric<Q>]
	void TestClassGeneric();
	
	[MyGeneric<T>]
	void TestMethodGeneric<T>();
}
```

The first declaration gives this:

```
error CS8968: 'Q': an attribute type argument cannot use type parameters
```

The second one has this error:

```
error CS0246: The type or namespace name 'T' could not be found (are you missing a using directive or an assembly reference?)
```

## Required Members

These are some random ideas around both `required` and `init` properties and constructors. There are similarities with these, so that's why I'm including both.

### `init` Properties

Right now, I have `init` properties unmockable. I'd like to rectify that.

For `init` properties, would this work?

```
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

```
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

```
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

```
A = 4, B = , C = 00000000-0000-0000-0000-000000000000
A = 0, B = B value, C = cbdf9c87-881f-41a0-8db6-2920b06436fd
```

So...there's hope. But this would mean I would have to put a bunch of stuff back in from issue #158 :|. Well, maybe. We may not need to have expectations on its initialization, but we may need to have some of the props (esp. ones that can't be mocked) set to initial values.

### `required` Properties

Required properties have been proposed for C#, and there's talk of them showing up in C# 11, but nothing is committed yet. In fact, at this point, this can't be used as a preview feature just yet (though [sharplab.io](https://sharplab.io/) allows you to play with it).

We need to be concerned with this, because, while properties on interfaces cannot be marked with `required`, an `abstract` class, or a class with `virtual` properties can be `required`, and then an override **must** be `required` as well:

```
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

```
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

If a property is both `init` and `required`, it would be `required`. In other words, I wouldn't make an optional parameter in the `Instance()` methods for this property; it would be handled by the generated `RequiredPropertyValues` class.