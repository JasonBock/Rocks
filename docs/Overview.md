## Table of Contents
- [Introduction](#introduction)
  - [Background and History](#background-and-history)
  - [Creating Mocks](#creating-mocks)
    - [API Generation](#api-generation)
    - [Mocking Simple Methods](#mocking-simple-methods)
    - [Parameter Verification](#parameter-verification)
    - [Method Call Counts](#method-call-counts)
    - [Implementing Handled Methods](#implementing-handled-methods)
    - [Returning Values](#returning-values)
    - [Passing Constructor Arguments to a Mock](#passing-constructor-arguments-to-a-mock)
    - [Mocking Generic Methods and Types](#mocking-generic-methods-and-types)
    - [Mocking Methods with `ref/out/in` Parameters or `ref readonly` Return Values](#mocking-methods-with-refoutin-parameters-or-ref-readonly-return-values)
    - [Mocking Properties](#mocking-properties)
    - [Mocking Indexers](#mocking-indexers)
    - [Mocking Events](#mocking-events)
    - [Optional Arguments](#optional-arguments)
    - [Handling Asynchronous Code](#handling-asynchronous-code)
    - [`dynamic` Types](#dynamic-types)
    - ["Special" Types](#special-types)
  - [Using Makes](#using-makes)
  - [Managing Multiple Mocks](#managing-multiple-mocks)
- [Conclusion](#conclusion)
  
# Introduction

New to Rocks? In this page, we'll cover the essentials of what Rocks can do so you can get up to speed on the API with little effort. We'll go through creating mocks and how you handle methods, properties and events.  We'll demonstrate how you can test asynchronous code, and use `dynamic` and "special" types. We'll show what "makes" are and where they're useful.

If something is unclear after reading the documentation, you can always browse the tests in source to see specific examples of a case that may not be covered in detail here. If you are still unable to determine how something works, feel free to drop a message in the [Discord channel](https://discord.gg/ZXMhkKsMRb), or add an issue [here](https://github.com/JasonBock/Rocks/issues).

## Background and History

There are great mocking libraries out there, like [Moq](https://github.com/moq/moq "Moq mocking framework on GitHub") and [NSubstitute](http://nsubstitute.github.io/ "NSubstitute: A friendly substitute for .NET mocking libraries"), so why did I decide to create YAML (yet another mocking library) in 2015? There are essentially two reasons.

The first reason relates to how code generation was done with mocking libraries. Most (if not all) used an approach that ends up using `System.Reflection.Emit` to create a mock type on the fly, which requires knowledge of IL, or a library like [`Castle.DynamicProxy`](https://github.com/castleproject/Core) to facilitate the mock generation with IL. This is not a trivial endeavour. Furthermore, the generated code can't be stepped into during a debugging process. I wanted to write a mocking library with the new Compiler APIs (Roslyn) to see if I could make the code generation process for the mock much easier and allow a developer to step into that code if necessary.

The other reason was being able to pre-generate the mocks for a given assembly, rather than dynamically generate them in a test. This is what the [Microsoft Fakes Library](https://docs.microsoft.com/en-us/visualstudio/test/code-generation-compilation-and-naming-conventions-in-microsoft-fakes?view=vs-2019 "Microsoft Fakes: Generate & compile code; naming conventions - Visual Studio (Windows) | Microsoft Docs") can do, but I wanted to be able to do it where I could easily modify a project file and automatically generate those mocks.

This is what Rocks can do. Mocks are created by generating C# code on the fly and compiling it with the Compiler APIs. This makes it trivial to step into the mock code. Before the 5.0.0 version, this code generation step took place at runtime, but with source generators in C# 9, this generation happens as soon as you state that you want to create a mock of a particular type. Moreover, since the mock is generated C# code, **any** language feature in C# can be supported, such as optional parameters and pointer values. So, feel free to test Rocks out, and see what you think. Even if you don't use it as your primary mocking library, you may see just how easy it to generate code on the fly with the new Compiler APIs. Enjoy!

## Creating Mocks

There's a lot of scenarios that can be encountered when you want to create a mock. In this section we'll cover all of them.

### API Generation

With Rocks 5.0.0 and above, all of the mocks are created using [C# 9.0's source generation feature](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/ "Introducing C# Source Generators - Microsoft"). This means that you must be targeting .NET 5.0 to use Rocks. With source generation, you have a lot of freedom to generate what you want, and this is exactly what Rocks takes advantage of. When you want to create a mock, a number of properties are generated that group the members you can set expectations on. You'll see in this document references in code like `.Methods`, `.Properties`, and `.Indexers`. For example, if the type you want to mock has methods that can have expectations set on them, you'll see `.Methods` show up. Similarly, mockable properties can be found from the `.Properties` property. You use these properties to set expectations on specific members. This should become clear as you read on and look at the examples.

### Mocking Simple Methods

Creating a mock in Rocks is pretty easy. Let's say you have an interface defined like this:

```csharp
public interface IAmSimple
{
  void TargetAction();
  int TargetFunc();
}
```

Here's how you create the mock, define its expected interactions, use the mock, and verify the expectations:

```csharp
[assembly: RockCreate<IAmSimple>]

var expectations = new IAmSimpleCreateExpectations();
expectations.Methods.TargetAction();
expectations.Methods.TargetFunc().ReturnValue(44);

var mock = expectations.Instance();
mock.TargetAction();
var result = mock.TargetFunc();
// result is equal to 44
expectations.Verify();
```

The mocking process starts by instantiating an `IAmSimpleCreateExpectations` object. This is done by adding the `RockCreateAttribute` in code. When this attribute is added, Rocks will interrogate the type provided in the generic parameter. It will then create a number of properties for each mockable member. In the example, you can call `.Methods.TargetAction()` and `.Methods.TargetFunc()` to state that you expect these members to be called. For `TargetFunc()`, you can also specify the return value via the `ReturnValue()` method.

There are two attributes you can use: `RockCreateAttribute<>` and `RockMakeAttribute<>`. Makes will be covered later on in this document. These attributes can exist at the assembly, class, or method level. If you have multiple instances of these attributes defined in a project targeting the same type to mock, only one expectations class will be generated. The names of the generated expectation types follow these patterns:

* `RockCreate<MyType>` generates `MyTypeCreateExpectations`
* `RockMake<MyType>` generates `MyTypeMakeExpectations`

In other words, the naming pattern is `{Mock type name}{Mock kind}Expectations`. The expectation type is generated within the same namespace that the mock type resides within (if one exists).

Once you set your expectations, you can create an instance of the mock from the generated `Instance()` method. If the target type has multiple constructors with different parameters, `Instace()` methods will generated for each constructor - constructors will be covered in detail in [this section](#passing-constructor-arguments-to-a-mock).

Note that all mocks generated with Rocks are strict. That is, if you didn't set up an expectation for the `TargetAction()` call, the `Verify()` call would fail with a `VerificationException`. This exception type provided details on why verification failed.

### Parameter Verification

If your method has parameters, there are a couple of ways you can set up expectations on what should be passed into the method. Here's an interface with a method that takes an integer:

```csharp
public interface IHaveParameterExpectations
{
  void Target(int a);
}
```

You can verify that `Target()` will be called with an exact value by passing in that value when you set up the expectation:

```csharp
[assembly: RockCreate<IHaveParameterExpectations>]

var expectations = new IHaveParameterExpectationsCreateExpectations();
expectations.Methods.Target(44);

var mock = expectations.Instance();
mock.Target(44);

expectations.Verify();
```

If you don't care what the value is, you use `Arg.Any<>()`:

```csharp
[assembly: RockCreate<IHaveParameterExpectations>]

var expectations = new IHaveParameterExpectationsCreateExpectations();
expectations.Methods.Target(Arg.Any<int>());

var mock = expectations.Instance();
mock.Target(44);

expectations.Verify();
```

If you want to specify logic to validate the given value, you use `Arg.Validate<>()`:

```csharp
[assembly: RockCreate<IHaveParameterExpectations>]

var expectations = new IHaveParameterExpectationsCreateExpectations();
expectations.Methods.Target(Arg.Validate<int>(a => a > 20 && a < 50));

var mock = expectations.Instance();
mock.Target(44);

expectations.Verify();
```

You can also specify multiple expectations:

```csharp
[assembly: RockCreate<IHaveParameterExpectations>]

var expectations = new IHaveParameterExpectationsCreateExpectations();
expectations.Methods.Target(Arg.Validate<int>(a => a > 20 && a < 50));
expectations.Methods.Target(10);

var mock = expectations.Instance();
mock.Target(44);
mock.Target(10);

expectations.Verify();
```

### Method Call Counts

You may want to verify that code under test calls a method a specific number of times. You can do that by specifying an expected call count via `ExpectedCallCount()`:

```csharp
[assembly: RockCreate<IAmSimple>]

var expectations = new IAmSimpleCreateExpectations();
expectations.Methods.TargetAction().ExpectedCallCount(2);

var mock = expectations.Instance();
mock.TargetAction();
mock.TargetAction();

expectations.Verify();
```

This also works with multiple expectations:

```csharp
[assembly: RockCreate<IHaveParameterExpectations>]

var expectations = new IHaveParameterExpectationsCreateExpectations();
expectations.Methods.Target(44).ExpectedCallCount(2);
expectations.Methods.Target(22).ExpectedCallCount(3);

var mock = expectations.Instance();
mock.Target(22);
mock.Target(44);
mock.Target(22);
mock.Target(44);
mock.Target(22);

expectations.Verify();
```

We haven't covered properties yet, but the same process is in place with them as well.

### Implementing Handled Methods

You can provide a lambda via the `Callback()` method that will be called when a method is invoked so you can do things like capture method argument values:

```csharp
[assembly: RockCreate<IHaveParameterExpectations>]

var value = 0;

var expectations = new IHaveParameterExpectationsCreateExpectations();
expectations.Methods.Target(Arg.Validate<int>(i => i > 10))
  .Callback(a => value = a);

var mock = expectations.Instance();
mock.Target(44);

// value would be equal to 44 here.

expectations.Verify();
```

A callback can also be used if the member usage should throw an exception as expected behavior:

```csharp
expectations.Methods.Target(Arg.Validate<int>(i => i > 10))
  .Callback(_ => throw new NotSupportedException());
```

### Returning Values

If a method returns a value, you can use `ReturnValue()`:

```csharp
[assembly: RockCreate<IAmSimple>]

var expectations = new IAmSimpleCreateExpectations();
expectations.Methods.TargetFunc().ReturnValue(44);

var mock = expectations.Instance();
var x = mock.TargetFunc();

// x is equal to 44.

expectations.Verify();
```

### Passing Constructor Arguments to a Mock

If you want to mock a class with virtual members where the class only has constructors with multiple arguments, you can do it. You pass the constructor arguments to `Instance()`. Rocks will generate an override of `Instance()` for each constructor that exists on the target type. Here's an example:

```csharp
public class MockedClass
{
  public MockedClass(int value) { }

  public virtual void Target() { }
}

// ...

[assembly: RockCreate<MockedClass>]

var expectations = new MockedClassCreateExpectations();
expectations.Methods.Target();

var mock = expectations.Instance(44);
mock.Target();

expectations.Verify();
```

### Mocking Generic Methods and Types

If generics are in play, Rocks can handle that:

```csharp
public interface IHaveGenerics<T>
{
  void Target<Q>(T a, Q b);
}

// ...

[assembly: RockCreate<IHaveGenerics<string>>]

var expectations = new IHaveGenericsOfstringCreateExpectations();
expectations.Methods.Target<int>("a", 44));

var mock = expectations.Instance();
mock.Target("a", 44);

expectations.Verify();
```

Note that the generated expectations class name "flattens" the generic by inserting `Of` in the name before the generic parameters are specified.

Starting with Rocks 8.1.0, generic types can be mocked as an open generic:

```c#
[assembly: RockCreate(typeof(IHaveGenerics<>))]

var expectations = new IHaveGenericsCreateExpectations<string>();
expectations.Methods.Target<int>("a", 44));

var mock = expectations.Instance();
mock.Target("a", 44);

expectations.Verify();
```

This allows for more flexibility, in that only one set of supporting types are generated for the target type. With closed generics, Rocks must generate all of the supporting types for each closed generic.

### Mocking Methods with `ref/out/in` Parameters or `ref readonly` Return Values

Methods with either `ref`, `out`, or `in` parameters are supported. A delegate is created for you so you can easily create a callback method to handle the parameters if you want:

```csharp
public interface IHaveRefs
{
  void Target(ref int a);
}

// ...

[RockCreate<IHaveRefs>]
public void MyTestMethod()
{
  static void TargetCallback(ref int a) => a = 4;

  var expectations = new IHaveRefsCreateExpectations();
  expectations.Methods.Target(3).Callback(TargetCallback);

  var mock = expectations.Instance();
  var value = 3;
  mock.Target(ref value);

  // value is 4 here.

  expectations.Verify();
}
```

Since the `Action` and `Func` delegate types do not support `ref` and `out`, you have to declare the callback method yourself, but with local functions this is straightforward to do. You can use any method declaration technique you'd like - the callback method just needs to match the signature of the target method.

`in` parameters do not require any special handling as Rocks doesn't change the value of parameters. `ref readonly` also work in Rocks and require no extra work on your behalf. You can return your own values from methods if you want, and Rocks handles the requirement of having a field available to return by reference.

### Mocking Properties

Mocking properties is a breeze in Rocks. `Getters`, `Setters`, and/or `Initializers` properties are generated for you off of the `Properties` property:

```csharp
public interface IHaveAProperty
{
  string GetterAndSetter { get; set; }
}
```

Here's how you set up the expectations:

```csharp
[assembly: RockCreate<IHaveAProperty>]

var setupValue = Guid.NewGuid().ToString();

var expectations = new IHaveAPropertyCreateExpectations();
expectations.Properties.Getters.GetterAndSetter();
expectations.Properties.Setters.GetterAndSetter(setupValue);

var mock = expectations.Instance();
mock.GetterAndSetter = setupValue;
var value = mock.GetterAndSetter;

expectations.Verify();
```

Note that you can also set up callbacks and expected call counts just like you can with methods.

The `init` feature was added with C# 9, and `required` properties were added with C# 11. Starting with Rocks `7.0.0`, there's a way to set these properties when the mock is created. A type called `ConstructorProperties` is created that contains all of the `required` and `init` properties, and an instance of this type can be given as the first argument to the generated `Instance()` methods:

```csharp
public class RequiredAndInit
{
  public virtual void Foo() { }
  
  public string InitData { get; init; }
  public required string RequiredData { get; set; }
}

[assembly: RockCreate<RequiredAndInit>]

var expectations = new RequiredAndInitCreateExpectations();
expectations.Methods.Foo();

var mock = expectations.Instance(new() { InitData = "a", RequiredData = "b" });
```

This will set `InitData` to `"a"` and `RequiredData` to `"b"` on the mock instance.

`ConstructorProperties` will contain properties that are both virtual and non-virtual. If there are no `required` properties, the `constructorProperties` parameter will be nullable.

Note that this will work with `init` indexers as well. `required` indexers is currently not possible in C#.

### Mocking Indexers

Indexers are not something a lot of .NET developers use, but if you do, you can mock them in Rocks. An `Indexers` property is created to set indexer get, set, and/or init expectations. Following the naming convention of an indexer in C#, a method named `This()` is generated that allows you to set the expectations:

```csharp
public interface IHaveIndexer
{
  int this[int a] { get; set; }
}

// ...

[assembly: RockCreate<IHaveIndexer>]

var expectations = new IHaveIndexerCreateExpectations();
expectations.Indexers.Getters.This(3);
expectations.Indexers.Setters.This(4, 3);

var mock = expectations.Instance();
var propertyValue = mock[3];
mock[3] = 4;

expectations.Verify();
```

Note that the setter looks like it's taking an extra parameter - that's because the value is passed in as the first argument. If you're wondering why the value is the first argument, read the [Optional Arguments](#optional-arguments) section for the explanation.

### Mocking Events

If there's an event on the mock, you can raise it as part of a member's usage:

```csharp
public interface IHaveAnEvent
{
  void Target(int a);

  event EventHandler TargetEvent;
}

// ...

[assembly: RockCreate<IHaveAnEvent>]

var expectations = new IHaveAnEventCreateExpectations();
expectations.Methods.Target(1).RaiseTargetEvent(EventArgs.Empty);

var wasEventRaised = false;
var mock = expectations.Instance();
mock.TargetEvent += (s, e) => wasEventRaised = true;

// wasEventRaised is still false
mock.Target(1);
// wasEventRaised is now equal to true

expectations.Verify();
```

Rocks generates extension methods for every adornments object (which is returned when an expectation is set). The naming pattern is `Raise{Event name}`. There is an `AddRaiseEvent()` method you can call directly, but the extension methods makes it convenient to pass in the right event name and its' corresponding event argument value.

Note that these extension methods won't be created if the mock type is generic, an open generic is requested, and there are events on the mock type. This is due to the way the extension methods are created. This situation should be relative rare, and `AddRaiseEvent()` can still be used in this case. (This limitation is being tracked with [this issue](https://github.com/JasonBock/Rocks/issues/309) - hopefully this will be resolved in the future.)

### Optional Arguments

If your method or indexer has optional arguments, you can handle them just like other arguments. You can also use `Arg.IsDefault()` if you want to use the default argument no matter what it is (or if it changes in the future). Here's what that looks like:

```csharp
public interface IHaveOptionalArguments
{
  void Foo(int a, string b = "b", double c = 3.2);
  int this[int a, string b = "b"] { get; set; }
}

// ...

[assembly: RockCreate<IHaveOptionalArguments>]

var returnValue = 3;
var expectations = new IHaveOptionalArgumentsCreateExpectations();
// In this case, we're assuming b will be set to "b",
// and c will be set to 3.2
expectations.Methods.Foo(1);
// With the indexer getter, we assume b will be set to "b"
expectations.Indexers.Getters.This(2).ReturnValue(returnValue);
// Read the explanation after this code snippet ;)
expectations.Indexers.Setters.This(value: 52, a: 3);

var mock = expectations.Instance();
mock.Foo(1);
var value = mock[2];
mock[3] = 52;

expectations.Verify();

Assert.That(value, Is.EqualTo(returnValue));
```

There is a little bit of an oddity if an indexer's setter has optional or `params` arguments. Properties and indexers have methods that are mapped to the `get`, `set`, or `init` accessors. In this case of an indexer's setter, the `set_Item()` mapped method has the `value` parameter **after** the optional parameters. While C#'s compiler makes this happen, you can't declare this in C#:

```csharp
void set_Item(int a, string b = "b", int value) { /* ... */ }
```

What Rocks does is generate the `This()` extension method with the `value` as the **first** parameter. That way, any parameters with default values or declared as `params` can show up after `value`.

### Handling Asynchronous Code

If your mock returns a `Task`, `Task<T>`, `ValueTask`, or a `ValueTask<T>`, or you're using that returned value with `async/await`, you can create mocks that will allow you to test it:

```csharp
public interface IAmAsync
{
  Task<int> GoAsync();
}

public class UsesAsync
{
  private readonly IAmAsync amAsync;

  public UsesAsync(IAmAsync amAsync)
  {
    this.amAsync = amAsync;
  }

  public async Task<int> RunGoAsync()
  {
    return await this.amAsync.GoAsync();
  }
}

[assembly: RockCreate<IAmAsync>]

var expectations = new IAmAsyncCreateExpectations();
expectations.Methods.GoAsync().ReturnValue(Task.FromResult(44));

var uses = new UsesAsync(expectations.Instance());
await uses.RunGoAsync().ConfigureAwait(false);

expectations.Verify();
```

### `dynamic` Types

It's not a common thing to see `dynamic` used, but Rocks supports it just fine:

```csharp
public interface IHaveDynamic
{
  void Foo(dynamic d);
}

[assembly: RockCreate<IHaveDynamic>]

var expectations = new IHaveDynamicCreateExpectations();
expectations.Methods.Foo(Arg.Is<dynamic>("b"));

var mock = expectations.Instance();
mock.Foo("b");

expectations.Verify();
```

## "Special" Types

Rocks can mock members with certain kinds of types, such as pointer types and ref structs like `Span<T>`. So if you have an interface like this:

```csharp
public unsafe interface IHavePointers
{
  void PointerParameter(int* value);
}
```

You can mock it like this:

```csharp
var value = 10;

[assembly: RockCreate<IHavePointers>]

var expectations = new IHavePointersCreateExpectations();
expectations.Methods.PointerParameter(new()).Callback(_ => *_ = 20);

var mock = expectations.Instance();
mock.PointerParameter(&value);

expectations.Verify();
```

Note that `value` would be equal to 20 after `PointerParameter()` is called.

### Using Makes

There are times when your mock needs to return a value where you want to ensure that the return value is a specific instance. As you've seen with these Rocks examples, you define a `[RockCreate<>]` attribute instance, create a new expectations instance, set up expectations, and then call `Instance()`. If you need a mock with no expectations, you create a "make" by adding a `[RockMake<>]` attribute instance:

```csharp
public interface IValue { }

public interface IProduceValue
{
  IValue Produce();
}

public class UsesProducer
{
  private readonly IProduceValue producer;

  public UsesProducer(IProduceValue producer)
  {
    this.producer = producer;
  }

  public IValue GetValue()
  {
    return this.producer.Produce();
  }
}

// ...

[assembly: RockCreate<IProduceValue>]
[assembly: RockMake<IValue>]

var valueMock = new IValueMakeExpectations().Instance();

var produceExpectations = new IProduceValueCreateExpectations();
produceExpectations.Methods.Produce().ReturnValue(valueMock);

var uses = new UsesProducer(produceExpectations.Instance());

var producedValue = uses.GetValue();

// producedValue and valueMock are the same references.
```

Note that makes do no have any expectations set up on them so they can't be verified. If you call a method on a make that returns a value, it'll return the default value of the return type - the same thing applies for getters on properties and indexers.

# Conclusion

You've now seen the majority of cases that Rocks can handle. Remember to peruse the tests within `Rocks.IntegrationTests` in case you get stuck. If you'd like, feel free to submit a PR to update this document to improve its contents. If you run into any issues, please submit an issue. Happy coding!
