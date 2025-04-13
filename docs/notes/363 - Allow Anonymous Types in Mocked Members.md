Let's say we have this:

```c#
public interface IGeneric
{
    void DoStuff<T0, T1, T2>(T0 a, T1 b, T2 c);
}

DoStuff("a", new { Name = "Jason" }, Guid.NewGuid());
```

In this case, `T0` and `T2` are speakable types. `T1` is anonymous. So the expectation should be set up something like this:

```c#
expectations.Methods.DoStuff(Arg.Any<string>(), Arg.Any<AnonymousType>(), Arg.Any<Guid>());
```

Our `else if` check should look like this:

```c#
else if (
    (
        (typeof(T0).IsAnonymousType() && @genericHandler.GetType().GenericTypeArguments[0] == typeof(global::Rocks.AnonymousType)) ||
        (typeof(TO) == @genericHandler.GetType().GenericTypeArguments[0])
        &&
        (typeof(T1).IsAnonymousType() && @genericHandler.GetType().GenericTypeArguments[1] == typeof(global::Rocks.AnonymousType)) ||
        (typeof(T1) == @genericHandler.GetType().GenericTypeArguments[1])
        &&
        (typeof(T2).IsAnonymousType() && @genericHandler.GetType().GenericTypeArguments[2] == typeof(global::Rocks.AnonymousType)) ||
        (typeof(T2) == @genericHandler.GetType().GenericTypeArguments[2])
    ))
{
    var @handler = (dynamic)@genericHandler;

    if (@handler.@a.IsValid(typeof(TO).IsAnonymousType() ? new global::Rocks.AnonymousType(@a!) : @a) &&
        @handler.@b.IsValid(typeof(T1).IsAnonymousType() ? new global::Rocks.AnonymousType(@b!) : @b) &&
        @handler.@c.IsValid(typeof(T2).IsAnonymousType() ? new global::Rocks.AnonymousType(@c!) : @c))
    {
        @foundMatch = true;
        @handler.CallCount++;
        @handler.Callback?.Invoke(
            typeof(TO).IsAnonymousType() ? new global::Rocks.AnonymousType(@a!) : @a,
            typeof(T1).IsAnonymousType() ? new global::Rocks.AnonymousType(@b!) : @b
            typeof(T2).IsAnonymousType() ? new global::Rocks.AnonymousType(@c!) : @c);
        break;
    }
}
```

This is a **lot** of code that would be generated, not only in the mock, but all the gen-d code that `dynamic` would end up creating as well. Maybe I make this an opt-in feature, so it only occurs when you know an anonymous type would be in play.

Maybe the right call is to just create a one-line record that defines the shape of what you want from an anonymous type:

```c#
public sealed record NameContent(string Name);

DoStuff("a", new NameContent("Jason"), Guid.NewGuid());

// ...
expectations.Methods.DoStuff(Arg.Any<string>(), Arg.Any<NameContent>(), Arg.Any<Guid>());
```

There has been talk about letting anonymous types implement types ([this](https://github.com/dotnet/csharplang/issues/4301), [this](https://github.com/dotnet/csharplang/discussions/738), and [this](https://github.com/dotnet/csharplang/discussions/6049)), but there's no planned work to add this as a feature. Even if it was, you'd still have to create a type that the anonymous type would have to implement, and then you might as well just create a type at that point (there may still be cases where having this feature might be useful, but the feature isn't being planned anyway, so no use hoping for something that isn't on anyone's radar).
