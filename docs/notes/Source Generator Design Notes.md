# Overview
C#9 is (potentially) getting a new feature called [source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/). I think Rocks could greatly benefit from this and do some amazing things by "in-lining" the mock generation as well as provide clear extension methods to set up expectations for a particular type. This document are my thoughts on what could be done here.
## Brief Review
Here's the general idea. Let's say I have an interface like this:
```
public interface IService
{
  void DoSomething(int value);
}
```
I want to mock out `IService`, so this is how it's done in Rocks:
```
var rock = Rock.Create<IService>();
rock.Handle(_ => _.DoSomething(Arg.IsAny<int>()));
var chunk = rock.Make();
chunk.DoSomething(44);
rock.Verify();
```
There's a couple of points to mention here:
* The mock isn't made until runtime. When `Rock.Create<>()` is called, the mock is generated. The thing is, for a given type, the mock type is the same thing. Only the expectations may be different.
* The `Handle...()` methods are vast to handle all `Func` and `Action` types, as well as other cases for properties and events. Because Rocks relies upon these out-of-the-box delegate types, we're a bit limited in what can be mocked, and what can't be. For the vast majority of method calls, this is sufficient, but it is a (small) limitation.
* The type passed into `Handle()` is actually an expression that is parsed to determine what the expectations are.
## Source Generator Ideas
So, here's what I propose that can be done.
### Custom Handler Extension Methods
What I'd like to see is something like this:
```
rock.HandleDoSomething(Arg.IsAny<int>());
```
In other words, for all the members that are mockable, Rocks will generate an extension method that lets you pass in expectations, or literal values if that's the exact value you expect. For example, you should be allowed to do this:
```
rock.HandleDoSomething(44);
```
The extension method would be something like this:
```
public static MethodAdornments HandleDoSomething(this IRock<IService> @this, ArgumentExpectation<int> value) { ... }
```
To do this, the `ArgumentExpectation<>` class will need to support conversion:
```
public class ArgumentExpectation<T>
{
  public static implicit operator Argument<T>(T value) => new Argument<T>(value);

  public Argument(T value) => this.Value = value;

  public T Value { get; }
}
```
This means you could still set an expectation like this:
```
rock.HandleDoSomething(Arg.Is<int>(a => a > 20 && a < 50));
```
I think this is much cleaner. You have no expressions to parse (well, unless you want to use one to verify the value with a specific piece of code as an expression, and even then, *maybe* I could do something here with generated delegates), the `Handle` extension methods will use the names of the members along with their parameters, so there should be no collisions either. I think this also gets around some issues that I've seen with certain types not being supported with `Action` and `Func`. The generated extension methods would need to be in the "right" namespace such that the user could see them right away, and at this point, I'm not sure if the source generator infrastructure will be invoked at specific points in the code/IDE experience, or *only* during compilation.
### Generator Mock Type
I already know how to generate the mock. What I'd like to do is generate it as soon as someone types this:
```
var rock = Rock.Create<IService>();
```
Again, I'm not sure I'll be able to detect that, and I may also need to detect if any changes occur to `IService` as that would require mock regeneration. I'd also have to be smart enough to not generate the mock type if it's needed in other tests.

I think the mocked type could also be generated as a private class within the class that hosts the generated extension methods. Then someone can't inadvertently use the mock type. Furthermore, `Make()` would end up being an extension method that would have access to that private class.

### Summary
With source generators, Rocks has the potential to generate all mocks at compile time (if not sooner) and provider a better interface to express expectations. If this can be done, it'll potentially address a number of Rocks issues. Here's a (probably incomplete) list of those issues:
* https://github.com/JasonBock/Rocks/issues/126
* https://github.com/JasonBock/Rocks/issues/114
* https://github.com/JasonBock/Rocks/issues/109
* https://github.com/JasonBock/Rocks/issues/107
* https://github.com/JasonBock/Rocks/issues/83
* https://github.com/JasonBock/Rocks/issues/60
* https://github.com/JasonBock/Rocks/issues/34
* https://github.com/JasonBock/Rocks/issues/27

The main issue is being tracked [here](https://github.com/JasonBock/Rocks/issues/121).

Also, see [this issue](https://github.com/JasonBock/Rocks/issues/123) to look into something that may be useful in the Roslyn API to determine if a type is nullable.

### References:
* [Introducing C# Source Generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)
* [SourceGenerators Samples](https://github.com/dotnet/roslyn-sdk/tree/master/samples/CSharp/SourceGenerators)
* [INotifyPropertyChanged with C# 9.0 Source Generators](https://jaylee.org/archive/2020/04/29/notify-property-changed-with-rosyln-generators.html)
* [C# Source Generators: Less Boilerplate Code, More Productivity](https://dontcodetired.com/blog/post/C-Source-Generators-Less-Boilerplate-Code-More-Productivity)
* [Using C# Source Generators with Microsoft Feature Management Feature Flags](https://dontcodetired.com/blog/post/Using-C-Source-Generators-with-Microsoft-Feature-Management-Feature-Flags)
* [StackOnlyJsonParser](https://github.com/TomaszRewak/C-sharp-stack-only-json-parser)
* [First Look: C# Source Generators](https://daveabrock.com/2020/05/08/first-look-c-sharp-generators)
* [Source Generators in C#](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Source-Generators-in-CSharp)