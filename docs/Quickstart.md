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
    - [Mocking Generic Methods](#mocking-generic-methods)
    - [Mocking Methods with `ref/out/in` Parameters or `ref readonly` Return Values](#mocking-methods-with-refoutin-parameters-or-ref-readonly-return-values)
    - [Optional Arguments](#optional-arguments)
    - [Mocking Properties](#mocking-properties)
    - [Mocking Indexers](#mocking-indexers)
    - [Setting `required` and `init` Properties](#settin-required-and-init-properties)
    - [Mocking Events](#mocking-events)
  - [Using Makes](#using-makes)
  - [Handling Asynchronous Code](#handling-asynchronous-code)
  - [Managing Multiple Mocks](#managing-multiple-mocks)
  - [`dynamic` Types](#dynamic-types)
  - ["Special" Types](#special-types)
- [Conclusion](#conclusion)
  
# Introduction

New to Rocks? In this page, we'll cover the essentials of what Rocks can do so you can get up to speed on the API with little effort. We'll go through creating mocks and how you handle methods, properties and events. We'll show what "makes" are and where they're useful. We'll illustrate how you can use options with your mocks to debug the generated code. We'll also demonstrate how you can test asynchronous code.

Remember that this is just a quickstart. You can always browse the tests in source to see specific examples of a case that may not be covered in detail here.

## Background and History

There are great mocking libraries out there, like [Moq](https://github.com/moq/moq "Moq mocking framework on GitHub") and [NSubstitute](http://nsubstitute.github.io/ "NSubstitute: A friendly substitute for .NET mocking libraries"), so why did I decide to create YAML (yet another mocking library) in 2015? There are essentially two reasons.

The first reason relates to how code generation was done with mocking libraries. Most (if not all) used an approach that ends up using `System.Reflection.Emit`, which requires knowledge of IL. This is not a trivial endeavour. Furthermore, the generated code can't be stepped into during a debugging process. I wanted to write a mocking library with the new Compiler APIs (Roslyn) to see if I could make the code generation process for the mock much easier and allow a developer to step into that code if necessary.

The other reason was being able to pre-generate the mocks for a given assembly, rather than dynamically generate them in a test. This is what the [Microsoft Fakes Library](https://docs.microsoft.com/en-us/visualstudio/test/code-generation-compilation-and-naming-conventions-in-microsoft-fakes?view=vs-2019 "Microsoft Fakes: Generate & compile code; naming conventions - Visual Studio (Windows) | Microsoft Docs") can do, but I wanted to be able to do it where I could easily modify a project file and automatically generate those mocks.

This is what Rocks can do. Mocks are created by generating C# code on the fly and compiling it with the Compiler APIs. This makes it trivial to step into the mock code. Before the 5.0.0 version, this code generation step took place at runtime, but with source generators in C# 9, this generation happens as soon as you state that you want to create a mock of a particular type. So, feel free to test Rocks out, and see what you think. Even if you don't use it as your primary mocking library, you may see just how easy it to generate code on the fly with the new Compiler APIs. Enjoy!

## Creating Mocks

There's a lot of scenarios that can be encountered when you want to create a mock. In this section we'll cover all of them.

### API Generation

With Rocks 5.0.0 and above, all of the mocks are created using [C# 9.0's source generation feature](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/ "Introducing C# Source Generators - Microsoft"). This means that you must be targeting .NET 5.0 to use Rocks. With source generation, you have a lot of freedom to generate what you want, and this is exactly what Rocks takes advantage of. When you want to create a mock, a number of extension methods are generated that group the members you can set expectations on. You'll see in this document references in code to `.Methods()`, `.Properties()`, and `.Indexers()`. You use these extension methods to set expectations on specific members. This should become clear as you read on and look at the examples.

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
var expectations = Rock.Create<IAmSimple>();
expectations.Methods().TargetAction();
expectations.Methods().TargetFunc().Returns(44);

var mock = expectations.Instance();
mock.TargetAction();
var result = mock.TargetFunc();

expectations.Verify();
```

Note that all mocks generated with Rocks are strict. That is, if you didn't set up an expectation for the `TargetAction()` call, the `Verify()` call would fail.

### Parameter Verification

If your method has parameters, there are a couple of ways you can set up expectations on what should be passed into the method. Here's an interface with a method that takes an integer:

```csharp
public interface IHaveParameterExpectations
{
  void Target(int a);
}
```

You can verify that `Target(`) will be called with an exact value by passing in that value when you set up the expectation:

```csharp
var expectations = Rock.Create<IHaveParameterExpectations>();
expectations.Methods().Target(44);
```

You create an instance of the mock with the `Instance()` method:

```csharp
var mock = expectations.Instance();
mock.Target(44);

expectations.Verify();
```

If you don't care what the value is, you use `Arg.Any<>()`:

```csharp
var expectations = Rock.Create<IHaveParameterExpectations>();
expectations.Methods().Target(Arg.IsAny<int>());

var mock = expectations.Instance();
mock.Target(44);

expectations.Verify();
```

If you want to specify logic to validate the given value, you use `Arg.Validate<>()`:

```csharp
var expectations = Rock.Create<IHaveParameterExpectations>();
expectations.Methods().Target(Arg.Validate<int>(a => a > 20 && a < 50));

var mock = expectations.Instance();
mock.Target(44);

expectations.Verify();
```

You can also specify multiple expectations:

```csharp
var expectations = Rock.Create<IHaveParameterExpectations>();
expectations.Methods().Target(Arg.Validate<int>(a => a > 20 && a < 50));
expectations.Methods().Target(10);

var mock = expectations.Instance();
mock.Target(44);
mock.Target(10);

expectations.Verify();
```

### Method Call Counts

You may want to verify that code under test calls a method a specific number of times. You can do that by specifying an expected call count:

```csharp
var expectations = Rock.Create<IAmSimple>();
expectations.Methods().TargetAction().CallCount(2);

var mock = expectations.Instance();
mock.TargetAction();
mock.TargetAction();

expectations.Verify();
```

This also works with multiple expectations:

```csharp
var expectations = Rock.Create<IHaveParameterExpectations>();
expectations.Methods().Target(44).CallCount(2);
expectations.Methods().Target(22).CallCount(3);

var mock = expectations.Instance();
mock.Target(22);
mock.Target(44);
mock.Target(22);
mock.Target(44);
mock.Target(22);

expectations.Verify();
```

We haven't covered properties yet, but this works with them as well.

### Implementing Handled Methods

You can provide a lambda that will be called when a method is invoked so you can do things like capture method argument values:

```csharp
var value = 0;

var expectations = Rock.Create<IHaveParameterExpectations>();
expectations.Methods().Target(Arg.Validate<int>(i => i > 10))
  .Callback(a => value = a);

var mock = expectations.Instance();
mock.Target(44);

// value would be equal to 44 here.

expectations.Verify();
```

### Returning Values

If a method returns a value, you can use `Returns()`:

```csharp
var expectations = Rock.Create<IAmSimple>();
expectations.Methods().TargetFunc().Returns(44);

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

var expectations = Rock.Create<MockedClass>();
expectations.Methods().Target();

var mock = expectations.Instance(44);
mock.Target();

expectations.Verify();
```

### Mocking Generic Methods

If generics are in play, Rocks can handle that:

```csharp
public interface IHaveGenerics<T>
{
  void Target<Q>(T a, Q b);
}

// ...

var expectations = Rock.Create<IHaveGenerics<string>>();
expectations.Methods().Target<int>("a", 44));

var mock = expectations.Instance();
mock.Target("a", 44);

expectations.Verify();
```

### Mocking Methods with `ref/out/in` Parameters or `ref readonly` Return Values

Methods with either `ref`, `out`, or `in` parameters are supported. A delegate is created for you so you can easily create a callback method to handle the parameters if you want:

```csharp
public interface IHaveRefs
{
  void Target(ref int a);
}

// ...
public void MyTestMethod()
{
  static void TargetCallback(ref int a) => a = 4;

  var expectations = Rock.Create<IHaveRefs>();
  expectations.Methods().Target(3).Callback(TargetCallback);

  var mock = expectations.Instance();
  var value = 3;
  mock.Target(ref value);

  // value is 4 here.

  expectations.Verify();
}
```

Since `Action` and `Func` do not support `ref` and `out`, you have to declare the callback method yourself, but with local functions this is straightforward to do. You can use any method declaration technique you'd like - the callback method just needs to match the signature of the target method.

`in` parameters do not require any special handling as Rocks doesn't change the value of parameters. `ref readonly` also work in Rocks and require no extra work on your behalf. You can return your own values from methods if you want, and Rocks handles the requirement of having a field available to return by reference.

### Optional Arguments

If your method has optional arguments, you can handle them just like other arguments. You can also use `Arg.IsDefault()` if you want to use the default argument no matter what it is (or if it changes in the future). Here's what that looks like:

```csharp
public interface IHaveOptionalArguments
{
  void Target(int a, string b = "b", long c = 44);
}

// ...

var expectations = Rock.Create<IHaveOptionalArguments>();
expectations.Methods().Target(22, Arg.IsDefault<string>(), Arg.IsDefault<long>()));

var mock = expectations.Instance();
mock.Target(22);

expectations.Verify();
```

### Mocking Properties

Mocking properties is a breeze in Rocks. `Get` and `Set` extension methods are generated for you that are available from the `Properties()` extension method:

```csharp
public interface IHaveAProperty
{
  string GetterAndSetter { get; set; }
}
```

Here's how you set up the expectations:

```csharp
var expectations = Rock.Create<IHaveAProperty>();
expectations.Properties().Getters().GetterAndSetter();
expectations.Properties().Setters().GetterAndSetter();

var mock = expectations.Instance();
mock.GetterAndSetter = Guid.NewGuid().ToString();
var value = mock.GetterAndSetter;

expectations.Verify();
```

Note that you can also set up callbacks and expected call counts just like you can with methods.

If you have a property that uses `init`, you can create a mock with that, but you can't set any expectations with it.

### Mocking Indexers

Indexers are not something a lot of .NET developers use, but if you do, you can mock them in Rocks:

```csharp
public interface IHaveIndexer
{
  string this[string a] { get; set; }
}

// ...

var expectations = Rock.Create<IHaveIndexer>();
expectations.Indexers().Getters().This(4);
expectations.Indexers().Setters().This("b", 4);

var mock = expectations.Instance();
var propertyValue = mock[indexer1];
mock[indexer1] = indexer1SetValue;

expectations.Verify();
```

Note that the setter looks like it's taking an extra parameter - that's because the value is passed in as the last argument.

### Setting `required` and `init` Properties

The `init` feature was added with C# 9, and `required` properties were added with C# 11. Starting with Rocks `7.0.0`, there's a way to set these properties when the mock is created. A type called `ConstructorProperties` is created that contains all of the `required` and `init` properties, and an instance of this type can be given as the first argument to the generated `Instance()` methods:

```csharp
public class RequiredAndInit
{
  public virtual void Foo() { }
  
  public string InitData { get; init; }
  public required string RequiredData { get; set; }
}

var expectations = Rock.Create<RequiredAndInit>();
expectations.Methods().Foo();

var mock = expectations.Instance(new() { InitData = "a", RequiredData = "b" });
```

This will set `InitData` to `"a"` and `RequiredData` to `"b"` on the mock instance.

`ConstructorProperties` will contain properties that are both virtual and non-virtual. If there are no `required` properties, the `constructorProperties` parameter will be nullable.

Note that this will work with `init` indexers as well. `required` indexers is currently not possible in C#.

### Mocking Events

If there's an event on the mock, you can raise it as part of a member's usage:

```csharp
public interface IHaveAnEvent
{
  void Target(int a);

  event EventHandler TargetEvent;
}

// ...

var expectations = Rock.Create<IHaveAnEvent>();
expectations.Methods().Target(1).RaisesTargetEvent(EventArgs.Empty);

var wasEventRaised = false;
var mock = expectations.Instance();
mock.TargetEvent += (s, e) => wasEventRaised = true;

// wasEventRaised is still false
mock.Target(1);
// wasEventRaised is now equal to true

expectations.Verify();
```

## Using Makes

There are times where your mock needs to return a value where you want to ensure that the return value is a specific instance. As you've seen with these Rocks examples, you always do `Rock.Create()`, set up expectations, and then call `Instance()`. If you need a mock with no expectations, you create a "make" via a call to `Rock.Make()`:

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

var valueMock = Rock.Make<IValue>().Instance();

var produceExpectations = Rock.Create<IProduceValue>();
produceExpectations.Methods().Produce().Returns(valueMock);

var uses = new UsesProducer(produceExpectations.Instance());

var producedValue = uses.GetValue();

// producedValue and valueMock are the same references.
```

Note that makes do no have any expectations set up on them so they can't be verified. If you call a method on a make that returns a value, it'll return the default value of the return type (same thing applies for getters on properties and indexers).

## Handling Asynchronous Code

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

var expectations = Rock.Create<IAmAsync>();
expectations.Methods().GoAsync().Returns(Task.FromResult(44));

var uses = new UsesAsync(expectations.Instance());
await uses.RunGoAsync().ConfigureAwait(false);

expectations.Verify();
```

## Managing Multiple Mocks

If you need to create a number of mocks within a test, you can use `RockRepository` to call `Verify()` on all of them once the test completes. Here's an example of how it works:

```csharp
public interface IFirstRepository
{
  void Foo();
}

public interface ISecondRepository
{
  void Bar();
}

public void MyTestMethod()
{
  using var repository = new RockRepository();

  var firstExpectations = repository.Add(Rock.Create<IFirstRepository>());
  firstExpectations.Methods().Foo();

  var secondExpectations = repository.Add(Rock.Create<ISecondRepository>());
  secondExpectations.Methods().Bar();

  var firstMock = firstExpectations.Instance();
  firstMock.Foo();

  var secondMock = secondExpectations.Instance();
  secondMock.Bar();
}
```

Before 5.0.0, the repository worked by calling `repository.Create<MyTypeToMock>()`. However, the source generation has to happen within the assembly that needs the mock. Therefore, the call to `Rock.Create()` can't exist within `RockRepository` itself. The "workaround" is just to call `Rock.Create()` and pass the result into `Add()`. Internally, the repository will add that return value to a list and then passes that back as the return value. This list is used in its `Dispose()` implementation to verify all the mocks.

## `dynamic` Types

It's not a common thing to see `dynamic` used, but Rocks supports it just fine:

```csharp
public interface IHaveDynamic
{
  void Foo(dynamic d);
}

var expectations = Rock.Create<IHaveDynamic>();
expectations.Methods().Foo(Arg.Is<dynamic>("b"));

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

var expectations = Rock.Create<IHavePointers>();
expectations.Methods().PointerParameter(new()).Callback(_ => *_ = 20);

var mock = expectations.Instance();
mock.PointerParameter(&value);

expectations.Verify();
```

Note that `value` would be equal to 20 after `PointerParameter()` is called.

# Conclusion

You've now seen the majority of cases that Rocks can handle. Remember to peruse the tests within `Rocks.IntegrationTests` in case you get stuck. If you'd like, feel free to submit a PR to update this document to improve its contents. If you run into any issues, please submit an issue. Happy coding!