OK, this will be a LOT of work. What needs to be done?

# Ref Struct Work

Basically [this](https://github.com/JasonBock/Rocks/issues/170). I may be able to create custom types for `Span<>` and related `Span` types. As I mentioned in the issue, there may be support for all `ref struct` types in generics in the future, so this can be generalized, but `Span<>` is pervasive enough that making supporting types for it I think will be beneficial.

OK, I did some work and updated the issue. I can create a handful of types that can be used for `Span<>` and `ReadOnlySpan<>`. Other `ref struct` types will have to still have all of that gen'd until C# will support `ref struct`s in generics. I should probably also consider having distinct properties for `TypeReferenceModel`. I have `IsEsoteric`, `IsPointer`, and `IsRefLikeType`. Maybe what's better is `IsPointer`, `IsFunctionPointer`, and `IsRefLikeType`

# Create New Supporting Types

While I already have some of these types, they'll need to change. Look at the work in `GeneratedBaseMocking`.

* `Expectations`
* `Handler<TCallback>`
* `Handler<TCallback, TReturnValue>`

I'm not sure these needs to change, basically all the adornment classes.
* `MethodAdornments<TCallback>`
* `MethodAdornments<TCallback, TReturnValue>`

# DONE - Attributes

Create two attributes, `RockCreateAttribute` and `RockMakeAttribute`, that take a generic parameter, can only exist at assembly level, and multiple can exist.

Actually, I just made one attribute, and moved `BuildType`, which I use so there's just one. This may also help if I add "other" ways to create mocks.

# DONE - Create `RockGenerator`

This looks for `RockAttribute`. However...`MockModel` uses `InvocationExpressionSyntax` node, but that's only used for diagnostic reporting. So, I'll turn that into a `SyntaxNode`, and then I can pass in the node for the attribute declaration.

# Alter Generated Code

A lot of work happens here. Again, look at `GeneratedBaseMocking`.

# DONE - Create `Handler{n}` Classes

These need to be created for each member with an identifier.

# DONE - Create Handler Lists

These need to be created for each member with an identifier.

# DONE - Create Expectations Properties

These used to be extension methods; now, they'll live on the expectations type. Methods, properties, indexers, along with the explicit member implementations.

# DONE - Implement `Verify()`

Basically just need to do `.AddRange()` for each handler list.

# DONE - Update Member Handling

We no longer need casts. Just need to modify which list the member iterates.

# DONE - Create Custom Expectations Classes

These will be nested within the expectations class. The extension methods I made now go here.

# Casts for Type Parameter Handlers

Oops. I forgot there are cases where the handlers need to be open generics, because the method could have open generics (properties/indexers don't matter). I'll need to generate those type parameters on the custom handler I make, and then still do a cast on the parameter if it's based on a type parameter.

# Remove ALL Casts

I think the way this is set up, I can truly remove all casts from all member implementations (save for the type parameter case mentioned in the previous section).

# Create `Pointer...<>` Types

This is basically [this](https://github.com/JasonBock/Rocks/issues/244) for the 3rd time.

# Create `Span...<>` and `ReadOnlySpan<>...` Types

Do what I talked about above.

# Create `InvalidRockAttributeDiagnostic`

I assume that I'll always find a `RockAttribute` with a generic parameter and a `BuildType` constructor argument. Someone *could* create an attribute to mess with this. So, I'll add some checks that if I find `RockAttribute` that really isn't mine, I'll create a diagnostic.


What can I clean up with the gen'd code?

* DONE - There's a blank line after the list handlers are new'd up. Remove it.
* DONE - If `Callback` isn't null, do a one liner like this: `@handler.Callback?.Invoke();`. If there's a return value, I think I can do this: `@handler.Callback?.Invoke() ?? @handler.ReturnValue`
* DONE - Need to create a blank line between property and/or indexer implementations.
* DONE - Add a blank line after any member implementations in the expectations classes.
* DONE - When I create a handler:
    * Do not need to set `Callback` or `CallCount`
    * Update the base `HandlerV4<>` class to set `ExpectedCallCount` to `1`.
    * If the member has no parameters, just do `();` after the `new...()` constructor call.
* DONE - After gen'ing a member expectation class (even the internal property/indexer ones), add a blank line.
* DONE - After gen'ing the member expectation classes, add a blank line.
* DONE - After gen'ing the member expectation properties, add a blank line.
* DONE - There's a space between `internal` and the target type for `Instance()`, remove it.
* DONE - There's a blank line after generating `Instance()`, remove it.
* I should consider making the `expectations` field a property named `Expectations`. This is consistent with the approach uses in the expectation classes.

* It's possible that the `expectations` field **could** collide with any implemented members, or any members on the mock type for that matter. Highly unlikely, but it's possible. Note that this was the same case with the current approach as that uses a `handlers` field. So...I could get a list of all the member names on the mock type (mocked or not), and ensure the field name doesn't collide, but that's a lot of work for very little gain.

What can I clean up with what I've done so far?

* There should be **no** casts done in the mock, or the expectations, unless the method has open generics.
* Create `[RockCreate<>]` and `[RockMake<>]`. That should be a bit more concise than passing in `BuildType`. I can still use one `RockGenerator` class, and look at the name of the type to determine which one to build. Maybe rename `RockGenerator` to `RockAttributeGenerator`.

More areas to test:

* `IHaveLotsOfMembers` is causing an issue/error. Need to investigate.
* This may be complicated, and not sure if this is worth it, but...it may be possible that a mock type has duplicated handler types. It may be worth trying to reduce the number of handler types made, though I'm not sure of this.
* Properties that are `requires` and/or `init`, as `ConstructorProperties` are generated
* Explicit interface implementation, that generates other things
* Open generics on a method
* `ret return` types
* Makes, gotta change that.
