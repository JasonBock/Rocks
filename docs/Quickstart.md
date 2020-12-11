# Introduction

New to Rocks? In this page, we'll cover the essentials of what Rocks can do so you can get up to speed on the API with little effort. We'll go through creating mocks and how you handle methods, properties and events. We'll show what "makes" are and where they're useful. We'll illustrate how you can use options with your mocks to debug the generated code. We'll also demonstrate how you can test asynchronous code.

Remember that this is just a quickstart. You can always browse the tests in source to see specific examples of a case that may not be covered in detail here.

## Creating Mocks

There's a lot of scenarios that can be encountered when you want to create a mock. In this section we'll cover all of them.

### API Generation

With Rocks 5.0.0 and above, all of the mocks are created using [C# 9.0's source generation feature](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/). This means that you must be targeting .NET 5.0 to use Rocks. With source generation, you have a lot of freedom to generate what you want, and this is exactly what Rocks takes advantage of. When you want to create a mock, a number of extension methods are generated that group the members you can set expectations on. You'll see in this document references in code to `.Methods()`, `.Properties()`, and `.Indexers()`. You use these extension methods to set expectations on specific members. This should become clear as you read on and look at the examples.

### Mocking Simple Methods

Creating a mock in Rocks is pretty easy. Let's say you have an interface defined like this:

```
public interface IAmSimple
{
  void TargetAction();
  int TargetFunc();
}
```

Here's how you create the mock, define its expected interactions, use the mock, and verify the expectations:

```
var rock = Rock.Create<IAmSimple>();
rock.Methods().TargetAction();
rock.Methods().TargetFunc().Returns(44);

var chunk = rock.Instance();
chunk.TargetAction();
var result = chunk.TargetFunc();

rock.Verify();
```

Note that all mocks generated with Rocks are strict. That is, if you didn't set up an expectation for the `TargetAction()` call, the `Verify()` call would fail.

### Parameter Verification

If your method has parameters, there are a couple of ways you can set up expectations on what should be passed into the method. Here's an interface with a method that takes an integer:

```
public interface IHaveParameterExpectations
{
  void Target(int a);
}
```

You can verify that `Target(`) will be called with an exact value by passing in that value when you set up the expectation:

```
var rock = Rock.Create<IHaveParameterExpectations>();
rock.Methods().Target(44);
```

You create an instance of the mock with the `Instance()` method:

```
var chunk = rock.Instance();
chunk.Target(44);

rock.Verify();
```

If you don't care what the value is, you use `Arg.Any<>()`:

```
var rock = Rock.Create<IHaveParameterExpectations>();
rock.Methods().Target(Arg.IsAny<int>());

var chunk = rock.Instance();
chunk.Target(44);

rock.Verify();
```

If you want to specify logic to validate the given value, you use `Arg.Validate<>()`:

```
var rock = Rock.Create<IHaveParameterExpectations>();
rock.Methods().Target(Arg.Validate<int>(a => a > 20 && a < 50));

var chunk = rock.Instance();
chunk.Target(44);

rock.Verify();
```

You can also specify multiple expectations:

```
var rock = Rock.Create<IHaveParameterExpectations>();
rock.Methods().Target(Arg.Validate<int>(a => a > 20 && a < 50));
rock.Methods().Target(10);

var chunk = rock.Instance();
chunk.Target(44);
chunk.Target(10);

rock.Verify();
```

### Method Call Counts

You may want to verify that code under test calls a method a specific number of times. You can do that by specifying an expected call count:

```
var rock = Rock.Create<IAmSimple>();
rock.Methods().TargetAction().CallCount(2);

var chunk = rock.Instance();
chunk.TargetAction();
chunk.TargetAction();

rock.Verify();
```

This also works with multiple expectations:

```
var rock = Rock.Create<IHaveParameterExpectations>();
rock.Methods().Target(44).CallCount(2);
rock.Methods().Target(22).CallCount(3);

var chunk = rock.Instance();
chunk.Target(22);
chunk.Target(44);
chunk.Target(22);
chunk.Target(44);
chunk.Target(22);

rock.Verify();
```

We haven't covered properties yet, but this works with them as well.

### Implementing Handled Methods

You can provide a lambda that will be called when a method is invoked so you can do things like capture method argument values:

```
var value = 0;

var rock = Rock.Create<IHaveParameterExpectations>();
rock.Methods().Target(Arg.Validate<int>(i => i > 10))
	.Callback(a => value = a);

var chunk = rock.Instance();
chunk.Target(44);

// value would be equal to 44 here.

rock.Verify();
```

### Returning Values

If a method returns a value, you can use `Returns()`:

```
var rock = Rock.Create<IAmSimple>();
rock.Methods().TargetFunc().Returns(44);

var chunk = rock.Instance();
var x = chunk.TargetFunc();

// x is equal to 44.

rock.Verify();
```

### Passing Constructor Arguments to a Mock

If you want to mock a class with virtual members where the class only has constructors with multiple arguments, you can do it. You pass the constructor arguments to `Instance()`. Rocks will generate an override of `Instance()` for each constructor that exists on the target type. Here's an example:

```
public class MockedClass
{
  public MockedClass(int value) { }

  public virtual void Target() { }
}

// ...

var rock = Rock.Create<MockedClass>();
rock.Methods().Target();

var chunk = rock.Instance(44);
chunk.Target();

rock.Verify();
```

### Mocking Generic Methods

If generics are in play, Rocks can handle that:

```
public interface IHaveGenerics<T>
{
  void Target<Q>(T a, Q b);
}

// ...

var rock = Rock.Create<IHaveGenerics<string>>();
rock.Methods().Target<int>("a", 44));

var chunk = rock.Instance();
chunk.Target("a", 44);

rock.Verify();
```

### Mocking Methods with `ref/out/in` Parameters or `ref readonly` Return Values

Methods with either `ref`, `out`, or `in` parameters are supported. A delegate is created for you so you can easily create a callback method to handle the parameters if you want:

```
public interface IHaveRefs
{
  void Target(ref int a);
}

// ...
public void MyTestMethod()
{
  static void TargetCallback(ref int a) => a = 4;

  var rock = Rock.Create<IHaveRefs>();
  rock.Methods().Target(3).Callback(TargetCallback);

  var chunk = rock.Instance();
  var value = 3;
  chunk.Target(ref value);

  // value is 4 here.

  rock.Verify();
}
```

Since `Action` and `Func` do not support `ref` and `out`, you have to declare the callback method yourself, but with local functions this is straightforward to do. You can use any method declaration technique you'd like - the callback method just needs to match the signature of the target method.

`in` parameters do not require any special handling as Rocks doesn't change the value of parameters. `ref readonly` also work in Rocks and require no extra work on your behalf. You can return your own values from methods if you want, and Rocks handles the requirement of having a field available to return by reference.

### Optional Arguments

If your method has optional arguments, you can handle them just like other arguments. You can also use `Arg.IsDefault()` if you want to use the default argument no matter what it is (or if it changes in the future). Here's what that looks like:

```
public interface IHaveOptionalArguments
{
  void Target(int a, string b = "b", long c = 44);
}

// ...

var rock = Rock.Create<IHaveOptionalArguments>();
rock.Methods().Target(22, Arg.IsDefault<string>(), Arg.IsDefault<long>()));

var chunk = rock.Instance();
chunk.Target(22);

rock.Verify();
```

### Mocking Properties

Mocking properties is a breeze in Rocks. `Get` and `Set` extension methods are generated for you that are available from the `Properties()` extension method:

```
public interface IHaveAProperty
{
  string GetterAndSetter { get; set; }
}
```

Here's how you set up the expectations:

```
var rock = Rock.Create<IHaveAProperty>();
rock.Properties().Getters().GetterAndSetter();
rock.Properties().Setters().GetterAndSetter();

var chunk = rock.Instance();
chunk.GetterAndSetter = Guid.NewGuid().ToString();
var value = chunk.GetterAndSetter;

rock.Verify();
```

Note that you can also set up callbacks and expected call counts just like you can with methods.

### Mocking Indexers

Indexers are not something a lot of .NET developers use, but if you do, you can mock them in Rocks:

```
public interface IHaveIndexer
{
  string this[string a] { get; set; }
}

// ...

var rock = Rock.Create<IHaveIndexer>();
rock.Indexers().Getters().This(4);
rock.Indexers().Setters().This("b", 4);

var chunk = rock.Instance();
var propertyValue = chunk[indexer1];
chunk[indexer1] = indexer1SetValue;

rock.Verify();
```

Note that the setter looks like it's taking an extra parameter - that's because the value is passed in as the last argument.

### Mocking Events

If there's an event on the mock, you can raise it as part of a member's usage:

```
public interface IHaveAnEvent
{
  void Target(int a);

  event EventHandler TargetEvent;
}

// ...

var rock = Rock.Create<IHaveAnEvent>();
rock.Methods().Target(1).RaisesTargetEvent(EventArgs.Empty);

var wasEventRaised = false;
var chunk = rock.Instance();
chunk.TargetEvent += (s, e) => wasEventRaised = true;

// wasEventRaised is still false
chunk.Target(1);
// wasEventRaised is now equal to true

rock.Verify();
```

## Using Makes

There are times where your mock needs to return a value where you want to ensure that the return value is a specific instance. As you've seen with these Rocks examples, you always do `Rock.Create()`, set up expectations, and then call `Instance()`. If you need a mock with no expectations, you create a "make" via a call to `Rock.Make()`:

```
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

var value = Rock.Make<IValue>().Instance();

var producer = Rock.Create<IProduceValue>();
producer.Methods().Produce().Returns(value);

var uses = new UsesProducer(producer.Instance());

var producedValue = uses.GetValue();

// producedValue and value are the same references.
```

Note that makes do no have any expectations set up on them so they can't be verified. If you call a method on a make that returns a value, it'll return the default value of the return type (same thing applies for getters on properties and indexers).

## Handling Asynchronous Code

If your mock returns a `Task`, `Task<T>`, `ValueTask`, or a `ValueTask<T>`, or you're using that returned value with `async/await`, you can create mocks that will allow you to test it:

```
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

var rock = Rock.Create<IAmAsync>();
rock.Methods().GoAsync().Returns(Task.FromResult(44));

var uses = new UsesAsync(rock.Instance());
await uses.RunGoAsync().ConfigureAwait(false);

rock.Verify();
```

## Managing Multiple Mocks

If you need to create a number of mocks within a test, you can use `RockRepository` to call `Verify()` on all of them once the test completes. Here's an example of how it works:

```
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

  var firstRock = repository.Add(Rock.Create<IFirstRepository>());
  firstRock.Methods().Foo();

  var secondRock = repository.Add(Rock.Create<ISecondRepository>());
  secondRock.Methods().Bar();

  var firstChunk = firstRock.Instance();
  firstChunk.Foo();

  var secondChunk = secondRock.Instance();
  secondChunk.Bar();
}
```

Before 5.0.0, the repository worked by calling `repository.Create<MyTypeToMock>()`. However, the source generation has to happen within the assembly that needs the mock. Therefore, the call to `Rock.Create()` can't exist within `RockRepository` itself. The "workaround" is just to call `Rock.Create()` and pass the result into `Add()`. Internally, the repository will add that return value to a list and then passes that back as the return value. This list is used in its `Dispose()` implementation to verify all the mocks.

## `dynamic` Types

It's not a common thing to see `dynamic` used, but Rocks supports it just fine:

```
public interface IHaveDynamic
{
  void Foo(dynamic d);
}

var dynamicRock = Rock.Create<IHaveDynamic>();
dynamicRock.Methods().Foo(Arg.Is<dynamic>("b"));

var dynamicChunk = dynamicRock.Instance();
dynamicChunk.Foo("b");

dynamicRock.Verify();
```

## "Special" Types

Rocks can't mock methods with certain types, such as pointer types and `Span<T>`. So if you have an interface like this:

```
public unsafe interface IHaveReturnPointers
{
  void TakeAPointerBecauseReasons(int* ptrToSomething);
  int* ReturnAPointerBecauseReasons();
}
```

It won't work. However, even though this is a rare situation, there are plans to add support for these types in a future version of Rocks.

# Conclusion

You've now seen the majority of cases that Rocks can handle. Remember to peruse the tests within `Rocks.IntegrationTests` in case you get stuck. If you'd like, feel free to submit a PR to update this document to improve its contents. If you run into any issues, please submit an issue. Happy coding!