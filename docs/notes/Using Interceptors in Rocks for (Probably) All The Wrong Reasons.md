# Using Interceptors in Rocks for (Probably) All The Wrong Reasons

In a future C# version, there will be a feature called interceptors (maybe, unless it gets pulled). Details about it can be found [here](https://github.com/dotnet/csharplang/issues/7009) and [here](https://github.com/dotnet/roslyn/blob/main/docs/features/interceptors.md#interceptslocationattribute). It's currently an experimental feature, and there is no guarantee that it'll ever ship. In the interim, let's see if it's useful in Rocks.

Currently, you can only mock instance members that are virtual or abstract:

```csharp
public class Target
{
    public virtual void Mockable() { }

    public void NotMockable() { }
}

var expectations = Rock.Create<Target>();
expectations.Methods().Mockable();

// You can't do this right now,
// as Rocks has no way to "override"
// a non-virtual method.
//expectations.Methods().NotMockable();

var mock = expectations.Instance();
mock.Mockable();
// Calling this has no expectations:
mock.NotMockable();

expectations.Verify();
```

With interceptors, it **might** be possible to provide a hook for `NotMockable()` (note that right now, only methods can be intercepted):

```csharp
internal static class Interceptors
{
    [InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 24, column: 6)]
    public static void NotMockable(this Target self) =>  /* now what?? */;
}
```

Rocks would have to look for any method invocation on a mock to determine if it needs interception. If so, it would generate a partial class with a unique name with a method that would wire up the interception. One way it can do this is to see, at the call site, if the `IMethodSymbol` is on a type that implements `IHandler` (more on that later on).

The problem is that, once the interceptor is fired, what do we do then? The intent would be to do something like this:

```csharp
internal static class Interceptors
{
    [InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 24, column: 6)]
    public static void NotMockable(this Target @self)
    {
        if (@self.handlers.TryGetValue(3, out var @methodHandlers))
        {
            var @methodHandler = @methodHandlers[0];
            @methodHandler.IncrementCallCount();
            if (@methodHandler.Method is not null)
            {
                ((global::System.Action)@methodHandler.Method)();
            }
        }
        else
        {
            @self.NotMockable();
        }
    }
}
```

In other words, we'd want to move the mock validation inside this interception method. Note that if an expectation wasn't set, we'd simply fall back to the "normal" `NotMockable()` invocation. Since that one isn't tracked with an interceptor, it'll execute as it typically would.

A more complex example with a method that takes a parameters and returns a value would look like this:

```csharp
public int GetLength(string value) => value.Length;

internal static class Interceptors
{
    [InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 24, column: 6)]
    public static int GetLength(this Target @self, string @value)
    {
        if (@self.handlers.TryGetValue(4, out var @methodHandlers))
        {
            foreach (var @methodHandler in @methodHandlers)
            {
                if (((global::Rocks.Argument<string>)@methodHandler.Expectations[0]).IsValid(@value))
                {
                    @methodHandler.IncrementCallCount();
                    var @result = @methodHandler.Method is not null ?
                        ((global::System.Func<string, int>)@methodHandler.Method)(@value) :
                        ((global::Rocks.HandlerInformation<bool>)@methodHandler).ReturnValue;
                    return @result!;
                }
            }
            
            throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int GetLength(string @value)");
        }
        else
        {
            return base.GetLength(@value);
        }
    }
}
```

The only problem is getting a reference to the `private` field, `handlers`. This is typed as `Dictionary<int, List<HandlerInformation>>`. One thought is to create an `IHandler` interfac in Rocks that every mock would explicitly implement:

```csharp
// Put all the attributes and comments on this to try
// and "warn" users not to use this interface or the
// `Handlers` property.
public interface IHandler
{
    Dictionary<int, List<HandlerInformation>> Handlers { get; }
}

private sealed class RockTarget
    : Target, IHandler
{
    public RockTarget(Dictionary<int, List<HandlerInformation>> handlers)
    {
        this.Handlers = handlers;
    }

    Dictionary<int, List<HandlerInformation>> IHandler.Handlers { get; }
}
```

 Then all the interceptor would need to do is cast to `IHandler`:

```csharp
if (((IHandler)@self).Handlers.TryGetValue(4, out var @methodHandlers))
```

If I do this, I should probably make the type an `ImmutableDictionary<int, ImmutableList<HandlerInformation>>`, just in case someone would try to do weird things with the information in the collection.