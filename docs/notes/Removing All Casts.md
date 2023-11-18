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

For each member, there would be a `List<(...)> validations{n}` field, where `n` is the member identifier. When `Instance()` is called, it would need to pull all those from.

What about the extension methods for the expectations?

```csharp
internal static MethodAdornments<IInterfaceMethodReturn, Func<int, int>, int> OneParameter(
    this MethodExpectations<IInterfaceMethodReturn> @self, Argument<int> @a)
{
    ArgumentNullException.ThrowIfNull(@a);
    return new MethodAdornments<IInterfaceMethodReturn, Func<int, int>, int>(
        @self.Add<int>(1, new List<Argument>(1) { @a }));
}

```

This is where it would get tricky. I'd probably need to create something like this:

```csharp
// Maybe we really strip down Expectations<T> so
// it only has a Verify() implementation

// In Rocks...
public abstract class Expectations
{
  public void Verify()
  {
    if(this.WasInstanceInvoked)
    {
      var failures = new List<string>();

      foreach (var pair in this.Handlers)
      {
        foreach (var handler in pair.Value)
        {
          foreach (var failure in handler.Verify())
          {
            var member = this.MockType!.GetMemberDescription(pair.Key);

            failures.Add(
              $"Type: {typeof(T).FullName}, mock type: {this.MockType!.FullName}, member: {member}, message: {failure}");
          }
        }
      }
    }

    if (failures.Count > 0)
    {
      throw new VerificationException(failures);
    }
  }
}

protected abstract List<string> GetFailures();
    
/// <summary>
/// This property is used by Rocks and is not intented to be used by developers.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
public Type? MockType { get; set; }

/// <summary>
/// This property is used by Rocks and is not intented to be used by developers.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
public bool WasInstanceInvoked { get; set; }    

// Maybe this is a file-scoped type.
internal sealed class MockTypeExpectations
    : IExpectations
{
  // May want the capacity at 1, or 2, what's the best guess?
  internal List<(HandlerInformation handler, Argument<int> a, Func<int, int>? callback, int returnValue)> Handlers1 { get; } = new(1);
}
```

This would take some more code gen work, and I should look at the cost of adding in more code gen, compared to what it saves when a mock is used. But it's also not just about perf cost. This also arguably makes things "correct". Currently, someone could create their own `Expectations<T>` instance that contains a bunch of bad objects that will not cast correctly in the mock when expectations are checked. This approach would remove that because the user couldn't do that anymore. 
