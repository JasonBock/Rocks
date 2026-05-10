TODO:

* DONE - Change the `Handler` property on `Adornments<,,>` to this: `protected internal THandler Handler { get; protected set; }`
* DONE - Add `protected void Remove(TAdornments adornments, List<THandler> handlers)` to `Expectations`. This should just call `Remove()` on `handlers` with the `Handler` property value on the given `adornments`.
* DONE - Change the generated code:
    * DONE - Instead of using `Handlers<THandler>?`, create a `List<THandler>?`
    * DONE - In `MethodExpectationsMethodBuilder`, `PropertyExpectationsPropertyBuilder`, and `IndexerExpectationsIndexerBuilder`, change this:
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
    * DONE - In the generated `Expectations` type, for each handler, we need to add the following `Remove()` implementation:
```c#
internal void Remove(Adornment0 adornment)
{
    this.Remove(adornment, this.@handler0);
    if (this.@handlers0.Count == 0) { this.@handlers0 = null; }
}
```
* DONE - Update `Verify<THandler>(Handlers<THandler> handlers, uint memberIdentifier)` on `Expectations` to take a `List<THandler>` as the first parameter.
* DONE - Add `Remove(Expectations expectations)` to `RockContext()` that removes the given expectation from the internal list.
* DONE - Comment out all the code in `Handlers<>`.
* DONE - Since I need to check `WasInstanceInvoked` in adornments, it may be easier if I added `ThrowIfInstanceInvoked()` on `NewMockInstanceException`. It could also be used in a couple of code gen'd places. 
    * DONE - In the `Adornments` types, pass in a reference to `Expectations`, and then check `WasInstanceInvoked`. If it's `true`, throw an exception.
    * DONE - In all gen'd code, when an adornment is made, pass in the `this` `Expectations` reference to the constructor.
        * DONE - `MethodExpectationsMethodBuilder`
        * DONE - `PropertyExpectationsPropertyBuilder`
        * DONE - `IndexerExpectationsIndexerBuilder`
* Run integration tests, all should pass.
    * DONE - Fix: In all `Remove()` gen'd methods, I need to add in the method generics to the `Remove()` signature if the method has its' own generics. Only needs to be done with methods.
    * DONE - Fix: In all setups, pass `this.parent`, not `this`.
        * DONE - `MethodExpectationsMethodBuilder`
        * DONE - `PropertyExpectationsPropertyBuilder`
        * DONE - `IndexerExpectationsIndexerBuilder`
    * DONE - Fix: No longer is it `.First`, it's `[0]`
    * DONE - Fix: Use `global::Rocks.Expectations.Remove(...)`, not `this.Remove(...)`
    * Fix: For the `Remove(...)` call, we need to specify **all** generic type values. This will look ugly, but it's necessary.
* Update **all** NuGet package references
* Run code gen tests, all should pass.
* Run BenchmarkMockNet with current state, and then referncing local Rocks to see what the differences are (if any).
* If perf is good, remove `Handlers` file as it is no longer needed.
* Update unit tests with new gen'd code, all should pass.
* Update docs to reflect new feature