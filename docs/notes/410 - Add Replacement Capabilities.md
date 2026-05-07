TODO:

* DONE - Change the `Handler` property on `Adornments<,,>` to this: `protected internal THandler Handler { get; protected set; }`
* DONE - Add `protected void Remove(TAdornments adornments, List<THandler> handlers)` to `Expectations`. This should just call `Remove()` on `handlers` with the `Handler` property value on the given `adornments`.
* Change the generated code:
    * Instead of using `Handlers<THandler>?`, create a `List<THandler>?`
    * In `IndexerExpectationsIndexerBuilder`, `MethodExpectationsMethodBuilder`, and `PropertyExpectationsPropertyBuilder`, change this:
```c#
if (this.parent.handlers2 is null) { this.parent.handlers2 = new(@handler); }
else { this.parent.handlers2.Add(@handler); }
return new(@handler);
```
to this:
```c#
if (this.parent.handlers2 is null) { this.parent.handlers2 = new(1); }
this.parent.handlers2.Add(@handler);
return new(@handler);
```
    * In the generated `Expectations` type, for each handler, we need to add the following `Remove()` implementation:
```c#
internal void Remove(Adornment0 adornment)
{
    this.Remove(adornment, this.@handler0);
    if (this.@handlers0.Count == 0) { this.@handlers0 = null; }
}
```
* Update `Verify<THandler>(Handlers<THandler> handlers, uint memberIdentifier)` on `Expectations` to take a `List<THandler>` as the first parameter.
* Add `Remove(Expectations expectations)` to `RockContext()` that removes the given expectation from the internal list.
* Comment out all the code in `Handlers<>`.
* In the `Adornments` types, pass in a reference to `Expectations`, and then check `WasInstanceInvoked`. If it's `true`, throw an exception.
* Run integration tests, all should pass.
* Update **all** NuGet package references
* Run code gen tests, all should pass.
* Run BenchmarkMockNet with current state, and then referncing local Rocks to see what the differences are (if any).
* If perf is good, remove `Handlers` file as it is no longer needed.
* Update unit tests with new gen'd code, all should pass.
* Update docs to reflect new feature