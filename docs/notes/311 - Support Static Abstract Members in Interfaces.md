https://github.com/JasonBock/Rocks/issues/311

A decent amount of "generic math" interfaces use the Curiously Recurring Generic Pattern, or CRGP (sometimes "template" is replaced for "generic" to make it CRTP, but it's the same thing). Here are a couple of articles on the topic:

* https://blog.stephencleary.com/2022/09/modern-csharp-techniques-1-curiously-recurring-generic-pattern.html
* https://zpbappi.com/curiously-recurring-template-pattern-in-csharp/
* https://stackoverflow.com/questions/1327568/curiously-recurring-template-pattern-and-generics-constraints-c

I have in the docs that the way to support this is to make an intermediary type definition, which is literally a one-liner. So this may not be a big deal, and interfaces like `IParseable` don't do this. It's not a requirement :). But, it may get in the way a lot doing SAMIs, requiring users to create intermediary interfaces, which is somewhat annoying.

Let's say we had a simple interface with SAMIs:

```c#
public interface IAddable<T>
    where T : IAddable<T>
{
    static abstract T Zero { get; }
    static abstract T operator +(T t1, T t2);
}
```

Because of the `+` operator, `T` has to be constrained with a CRGP - otherwise, the following error occurs:

```
CS8924 - One of the parameters of a binary operator must be the containing type, or its type parameter constrained to it.
```

Before I do any more work, I think I need to figure out why CRGP was causing issues in the first place. Here's a simple interface:

```c#
public interface IProcessor<TProcessor>
    where TProcessor : IProcessor<TProcessor>
{
    void Process(TProcessor processor);
}
```

Let's say I did this:

```c#
[assembly: Rock(typeof(IProcessor<>), BuildType.Create)]

using var context = new RockContext();
var expectations = new IProcessorCreateExpectations<???>();
```

I'll address the `???` in a moment. CSLA has done CRGP for decades and Rocks works with it:

```c#
[assembly: Rocks.Rock(typeof(Customer), Rocks.BuildType.Create | Rocks.BuildType.Make)]
[assembly: Rocks.Rock(typeof(BusinessBase<>), Rocks.BuildType.Create | Rocks.BuildType.Make)]

public class Customer
    : BusinessBase<Customer>
{
    public virtual void Do() { }
}

using var context = new RockContext();
var customerExpectations = new CustomerCreateExpectations();
customerExpectations.Methods.Do();

var businessBaseExpectations = new BusinessBaseCreateExpectations<Customer>();
businessBaseExpectations.Methods.ToString();
```

Note that with the 2nd one, I can't mock `Do()`, but that makes sense as there's no `Do()` definition on `BusinessBase`, and `Customer` is technically that "intermediary" type that Rocks needs.

The main problem is that I need to be able to do this if there is no intermediary type:

```c#
var expectations = new IProcessorCreateExpectations<IProcessorCreateExpectations<???>.Mock>();
```

But I **can't** do that, because `Mock` is a nested type within a generic type, so I run into an infinite series of generic definitions, which can't be done.

I think we're still stuck with the user providing an intermediary type. There's just no way to get around this that I can find. Maybe what I can do is generate that type if the user requests it, and then the mock would use that as the deriving type:

```c#
[assembly: Rocks.Rock(typeof(IProcessor<>), Rocks.BuildType.Create | Rocks.BuildType.Make, Rocks.GenerateIntermediateType.Yes)]
```

The default would be `No`. If `Yes`, then Rocks would do something like this:

```c#
public sealed class IProcessorCreateExpectations<TProcessor>
    where TProcessor : IProcessor<TProcessor>
{
    // ...
    internal interface IProcessorIntermediate
        : IProcessor<IProcessorIntermediate> { }
    
    private sealed class Mock
        : IProcessorIntermediate
    {
        // ...
    }
}
```

Then the user could do this:

```c#
using var context = new RockContext();
var expectations = context.Create<IProcessorCreateExpectations<IProcessorCreateExpectations.IProcessorIntermediate>>();
```

There are problems here though. The main one is, how does Rocks figure out where to "plug the holes" within the intermediary type? That seems like that could end up in edge-case hell, and I feel like I really should not go there. But is that true? If the constraints refer to a type parameter, that's where the hole would be plugged. That's it. So maybe this wouldn't be that hard.

At the end of the day, it's a one-liner for a user to create the intermediary type, and they'll do it correctly.

So, back to SAMIs. One thing is that the mock type has to become visible to use the static members on that type. Maybe we can provide a flag to specify the visibility, with the default being `Internal`. Using the `IAddable<>` with an intermediary, here's what that would do:

```c#
public enum MockVisibility
{
    Internal, Public
}

public interface IAddableIntermediate
    : IAddable<IAddableIntermediate> { }

[assembly: Rocks.Rock(typeof(IAddableIntermediate), Rocks.BuildType.Create | Rocks.BuildType.Make, Rocks.MockVisibility.Public)]

using var context = new RockContext();
var expectations = context.Create<IAddableIntermediate>();
expectations.Properties.Getters.Zero().ReturnValue(new IAddableIntermediateMakeExpectations().Instance());
expectations.Operators.Addition(Arg.Any<IAddableIntermediate>(), Arg.Any<IAddableIntermediate>()).ReturnValue(???);
```

But, this is exposing yet another issue with trying to handle SAMIs. What do we return from `Addition()`? How do we create an instance of the mock type? This feels like it gets incredibly gross. The getter for `Zero()`, I had it return a "make", but we don't want to do that everywhere.

It feels like the only way to support SAMIs is to have the user handle it themselves. That feels very defeatist, but I can't think of another way to do this. **Maybe** we introduce the concept of returning "self":

```c#
expectations.Properties.Getters.Zero().ReturnSelf();
expectations.Operators.Addition(Arg.Any<IAddableIntermediate>(), Arg.Any<IAddableIntermediate>()).ReturnSelf();
```

Actually...this has promise. If this is set, then code that Rocks currently generates for the return value:

```c#
var @result = @handler.Callback is not null ?
    @handler.Callback() : @handler.ReturnValue;
```

Now turns into this:

```c#
var @result = @handler.Callback is not null ?
    @handler.Callback() :
    @handler.ReturnSelf ?
        this : @handler.ReturnValue;
```

`ReturnSelf()` could only work if the target type for the mock is assignable to the return type. Maybe we create a custom `Expectations` type that is offered when a member has a return value and the target type is assignable to it. This could be useful even if I don't do SAMIs, because this gives a way for the developer to return what will become the mock instance.

Tasks:
* Remove the check for SAMIs to create a diagnostic. In `MockModel.Create()`, there's this: `methods.HasStaticAbstractMembers || properties.HasStaticAbstractMembers`, and if true, creates `InterfaceHasStaticAbstractMembersDiagnostic`. This needs to go, we can remove that diagnostic. We **might** want to keep those `HasStaticAbstractMembers` properties. They may be useful, but we can probably remove them.
* When finding members on an interface, need to remove checks that would filter out  