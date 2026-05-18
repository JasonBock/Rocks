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
* DONE - Run integration tests, all should pass.
    * DONE - Fix: In all `Remove()` gen'd methods, I need to add in the method generics to the `Remove()` signature if the method has its' own generics. Only needs to be done with methods.
    * DONE - Fix: In all setups, pass `this.parent`, not `this`.
        * DONE - `MethodExpectationsMethodBuilder`
        * DONE - `PropertyExpectationsPropertyBuilder`
        * DONE - `IndexerExpectationsIndexerBuilder`
    * DONE - Fix: No longer is it `.First`, it's `[0]`
    * DONE - Fix: Use `global::Rocks.Expectations.Remove(...)`, not `this.Remove(...)`
    * DONE - Fix: For the `Remove(...)` call, we need to specify **all** generic type values. This will look ugly, but it's necessary.
* DONE - Run BenchmarkMockNet with current state, and then referncing local Rocks to see what the differences are (if any). Consider adding `CollectionsMarshal.AsSpan()` for enumeration if there's a noticeable drop-off.
* DONE - Add integration tests that test "remove" functionality.
* DONE - So, one thing that must change is using `memberIdentifer` as the name for adornments - i.e. `AdornmentsForHandler0`. It's not very descriptive, and if members get added to the mock type, the id value can change such that users will get compilation errors in tests even though nothing really should have broken. We need a name that is consistent no matter what other members are there, fairly descriptive, and yet won't be too long. The handler names do not need to change - they can stay as `Handler0` as this is still a name that the user should not care about. I think I should do something where I take all the parameter type names, concatenate them and get a hash value for that. Note that overloads using `ref` and `out` need to have those "directions" added in. Note that [there is a limit](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0015) to how big a name can get. We already have `{ExpectationsName}Adornments` to start so we should try to keep this name as small as possible. Also, I have [an issue](https://github.com/JasonBock/Rocks/issues/403) to add XML comments to generated types, so maybe I just do `{MemberName}Adornments{hash "fingerprint"}` for methods. Properties would get `Get/Set/Init` in the name. Indexers need to start with `this`. The XML comments would have the member name specified in it so that would show up in Intellisense. These types need to be updated: 
    * DONE - `MethodModel`: Add a `string ParameterHash { get; }` property that gets a small hash of the concatenated property values (see code below)
    * DONE - `MethodExpectationsMethodBuilder`: When referencing and constructing the adornment
    * DONE - `PropertyExpectationsPropertyBuilder`: When referencing and constructing the adornment
    * DONE - `IndexerExpectationsIndexerBuilder`: When referencing and constructing the adornment
    * DONE - `MockAdornmentsBuilder`: When defining and referencing the adornment
    * DONE - Final check: Look for the string "AdornmentsForHandler" for any stragglers that I missed.
* We need more than just the parameter types and `ref/out`. We need to include constraints as well. (Note, I checked, nullability does not matter for overloading, they're considered the same). These two methods are valid overloads, but my current scheme will gen the same hash code for both, which causes all sorts of errors:
    * `T? Get<T>(string key, T? defaultValue = null) where T : class;`
    * `T? Get<T>(string key, T? defaultValue = null) where T : struct;`
* Update **all** NuGet package references
* Run code gen tests, all should pass.
* If perf is good, remove `Handlers` file as it is no longer needed. Also, look for and remove any commented code with "Handlers<" in it.
* Update unit tests with new gen'd code, all should pass.
* Update docs to reflect new feature

TODO:
* In `TestGenerator.SpecificDiagnostics()`, consider changing the implementation such that it can allow splats - i.e. `ASPIRE*`. This will ignore **all** Aspire warnings.
* Add this in as a new separate feature: If there can only be one handler - i.e. the parameter count is 0 - there is no need to make a `List<THandler>?`. Instead, just make a `THandler?` field. In the mock implementation, change it from `[0]` to just the field itself. `Remove()` may need to use collection expressions, or we have `ShouldRemove(Handler)` methods that return a `bool` that specify if the reference should be set to `null`. Same with `Verify()`. If I do [the ordering feature](https://github.com/JasonBock/Rocks/issues/380), this may not work as I **might** end up supporting more than one handler for no-parameter methods.

Hash "fingerprint" code (came from Copilot) - may want to consider https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/:

```c#
using System;
using System.Security.Cryptography;
using System.Text;

var input = "int_string";

try
{
    var shortHash = GetSecureShortHash(input);
    Console.WriteLine($"Secure Short Hash: {shortHash}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

/// <summary>
/// Generates a short hash from a string using SHA256 
/// and returns the first 12 characters from the hex string.
/// </summary>
static string GetSecureShortHash(string input)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(input);

    using var sha256 = SHA512.Create();
    var bytes = Encoding.UTF8.GetBytes(input);
    var hashBytes = sha256.ComputeHash(bytes);
    var hex = Convert.ToHexString(hashBytes);

    return hex.Substring(0, 12);
}
```