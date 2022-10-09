If the type is defined like this:

```csharp
public class AsyncEnumeration
{
	public virtual async IAsyncEnumerable<string> GetRecordsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await Task.CompletedTask;
		yield return "x";
	}
}
```

This is how it should be implemented:

```csharp
public class X : AsyncEnumeration
{
    public override async IAsyncEnumerable<string> GetRecordsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
		await foreach(var iteratorValue in base.GetRecordsAsync(cancellationToken))
        {
            yield return iteratorValue;
        }
    }
}
```

Specifically:
* Determine if the method (which would only be for a value return) is `IAsyncEnumerable<>`. If so, `isAsyncIterator = true`.
* If there is a `CancellationToken` parameter that is marked with `[EnumeratorCancellation]`, that should be passed to the async iterator (since this is an override, that parameter would work for the call)
* Whenever there is a "return", it has to be transformed into the `await foreach` with a `yield return`
* For a "Make", the implementation has to be this:

```csharp
public override async IAsyncEnumerable<string> GetRecordsAsync(
	[EnumeratorCancellation] CancellationToken cancellationToken = default)
{
	await global::System.Threading.Tasks.Task.CompletedTask;
	yield break;
}
```

Or...another approach would be this:

```csharp
public class X : AsyncEnumeration
{
    public override IAsyncEnumerable<string> GetRecordsAsync(
        CancellationToken cancellationToken = default)
    {
        return base.GetRecordsAsync(cancellationToken);
    }
    
    private IAsyncEnumerable<string> Stuff() => default;
}
```

Note that the method is not `async`. So, if I detect that the method is an async iterator, make the method **not** `async`, remove `[EnumeratorCancellation]` from any attribute generation on the parameters (in fact, arguably I should never emit it), and just `return` whatever I'd typically return normally from the method. That's easier.

Well, now I'm confused. The "Create" scenario just works. It passes. ??

I do have to remove `[EnumeratorCancellation]`, because a warning can become an error.