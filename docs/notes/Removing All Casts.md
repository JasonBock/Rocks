Random thought that I keep coming back to for Rocks...

Right now expectations is basically a holder of object references to validate, invoke callbacks, and get return values.

What if all of that could be gen'd as well so that there are no casts?

For example, with this interface:

```csharp
public interface IInterfaceMethodReturn
{
  int NoParameters();
  int OneParameter(int a);
  int MultipleParameters(int a, string b);
}
```

Here's what the implementation of `OneParameter()` in the mock looks like:

```csharp
private readonly Dictionary<int, List<HandlerInformation>> handlers;

[MemberIdentifier(1, "int OneParameter(int @a)")]
public int OneParameter(int @a)
{
  if (this.handlers.TryGetValue(1, out var @methodHandlers))
  {
    foreach (var @methodHandler in @methodHandlers)
    {
      if (((Argument<int>)@methodHandler.Expectations[0]).IsValid(@a))
      {
        @methodHandler.IncrementCallCount();
        var @result = @methodHandler.Method is not null ?
            ((Func<int, int>)@methodHandler.Method)(@a) :
            ((HandlerInformation<int>)@methodHandler).ReturnValue;
        return @result!;
      }
    }
        
    throw new ExpectationException("No handlers match for int OneParameter(int @a)");
  }
    
  throw new ExpectationException("No handlers were found for int OneParameter(int @a)");
}
```

There's a cast for the argument, and two different casts for the return value, depending if a callback was invoked.

What if Rocks gen'd something like this?

```csharp
// Maybe in C# 12 we use frozen lists, or an immutable list.
// I'll just use List<> for the document.
private readonly List<(HandlerInformation handler, Argument<int> a, Func<int, int>? callback, int returnValue)> validations1;

[MemberIdentifier(1, "int OneParameter(int @a)")]
public int OneParameter(int @a)
{
  if (this.validations1.Count > 0)
  {
    foreach (var validation in this.validations1)
    {
      if (validation.a.IsValid(@a))
      {
        validation.handler.IncrementCallCount();
        return validation.callback is not null ?
          validation.callback(@a) :
          validation.returnValue;
      }
    }
        
    throw new ExpectationException("No handlers match for int OneParameter(int @a)");
  }

  throw new ExpectationException("No handlers were found for int OneParameter(int @a)");
}
```

The key takeaway here is that, all of the arguments, the callback, and the return value (if the method has a return) is all strongly typed. There are no casts taking place.

For each member, there would be a `List<(...)> validations{n}` field, where `n` is the member identifier. When `Instance()` is called, it would need to pull all those from. This is where it would get tricky. I'd probably need to create something like this:

```csharp
// Maybe we really strip down Expectations<T> so
// it only has a Verify() implementation

// Maybe this is a file-scoped type.
internal sealed class MockExpectations
    : Expectations<MockType>
{

}
```