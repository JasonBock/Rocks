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
* DONE - I should consider making the `expectations` field a property named `Expectations`. This is consistent with the approach uses in the expectation classes.

* It's possible that the `expectations` field **could** collide with any implemented members, or any members on the mock type for that matter. Highly unlikely, but it's possible. Note that this was the same case with the current approach as that uses a `handlers` field. So...I could get a list of all the member names on the mock type (mocked or not), and ensure the field name doesn't collide, but that's a lot of work for very little gain.

What can I clean up with what I've done so far?

* Finish up fixes in `PropertyExpectationsBuilderV4`. Already done in `BuildProperties()`, need to do the other three.
* Oops, I forgot that putting methods in the member expectations classes like `GetHashCode()` can cause an issue. If I `new` them, I can hide the base class implementation, which...should be OK?
* There should be **no** casts done in the mock, or the expectations, unless the method has open generics.
* DONE - Create `[RockCreate<>]` and `[RockMake<>]`. That should be a bit more concise than passing in `BuildType`. I can still use one `RockGenerator` class, and look at the name of the type to determine which one to build. Maybe rename `RockGenerator` to `RockAttributeGenerator`.
* I think I can reduce the number of handler classes made. For example, two methods that take no arguments and return void have exactly the same handler. If I can map arguments+return types to a handler number + the member identifier, I could make that reduction.
* When I generate the handler, what's the "proper" order?

var @handler = this.Expectations.handlers0[0];
@handler.CallCount++;
var @result = @handler.Callback?.Invoke() ?? @handler.ReturnValue;
return @result!;

* The projected types, I'm thinking they should just go under the nested expectation type.
* For `DoesNotReturn`, I'm thinking this can be simplified:

_ = @handler.Callback is not null ?
    @handler.Callback() : @handler.ReturnValue;

Just call the callback if it's not null. Something to consider.

More areas to test:

* DONE - `IHaveLotsOfMembers` is causing an issue/error. Need to investigate.
* DONE - Makes, gotta change that.
* DONE - Open generics on a method
* Properties that are `required` and/or `init`, as `ConstructorProperties` are generated
* Explicit interface implementation, that generates other things
* `ret return` types
* No longer need to project types for pointers.
* Remove FileName.cs from test project.
* Naming conflicts with property names on Handler

Projected Type Generation

OK, things have changed/simplified somewhat. Here are the steps:

* DONE - Callbacks
    * DONE - If a member has an esoteric type (pointer, function pointer, `ref struct``), either as a parameter or a return value, it needs a callback generated
* DONE - Arguments
    * DONE - If an argument is a function pointer or ref struct, it needs a custom `Argument` type generated (note that pointers can use `PointerArgument<>`)
* DONE - Types: There are two types that need to be made together whenever a return type that is a function pointer or a `ref struct` is encountered for each unique type:
    * DONE - Handler: Generate a `unsafe abstract class` that derives from `HandlerV4<TCallback>`, passing in the gen'd callback. It creates the `ReturnValue` property with the type name. Use `FunctionPointerHandlerTypeBuilderV4`, rename as `ProjectedHandlerTypeBuilderV4`
    * DONE - Adornments: Generate a `unsafe sealed class`, `AdornmentsFor...<THandler>` where `THandler` is constrained to the gen'd handler type, with a constructor taking the handler and a `ReturnValue()` method that takes the `ref struct` or function pointer type.

During mock code generation:

* DONE - Handlers
    * DONE - If the parameter type is a pointer, `PointerArgument<>` is used; otherwise, we use the gen'd argument type name. 
    * DONE - If the return type is a pointer, `PointerHandlerV4<>` is the base type to use for the handler. If the return type is a `ref struct` or a function pointer, we use the gen'd handler type as the base type.
* DONE - Member Implementation
    * DONE - Nothing should change. With all the code gen done before, everything is standardized.
* DONE - Expectations
    * DONE - If the return type is a pointer, return `PointerAdornmentsV4<>`. If it's a `ref struct` or a function pointer, use the `AdornmentsFor...<>` gen'd type. Either way, the implementation should be standard for everything.

* DONE - For `ref struct` and function pointers
    * DONE - The gen'd `HandlerFor...` type needs to not be sealed
    * DONE - he gen'd `AdornmentsFor...` type should not be generic (with no constraint either), and pass in the `HandlerFor...` type to the `THandler` parameter. Change the ctor parameter type as well.

Now I remember why `ref struct` returns are done through a delegate. I am gen'ing the return value delegate, but what I need to do is:

* DONE - For the `ReturnValue` property on the custom handler, it needs to be of this return value delegate
* DONE - Any time I gen a `ReturnValue` property, make the type nullable (arguments should always be set, but the return value may not)
* DONE - Adornment needs to get the right return value type if it's a `ref struct`
* DONE - For any mock member implementation that returns a value, if the return type is a `ref struct`, then do `.ReturnValue!()`

* DONE - To be consistent, just do this: `@handler.Callback is not null ? @handler.Callback() : @handler.ReturnValue;` when I return a value - basically don't do the `Callback?.Invoke` thing, it runs into too many corner cases. The other approach will always work (though this is OK for property setters)
* DONE - Property adornments need to be updated to return the right type for esoteric types.

* DONE - I don't think I need to have "explicit" expectation builders for properties and indexers. All they're doing is gen'ing the getters and setters and they don't need to know the type they're implementing.


* DONE - For indexers (getters and setters as well as explicit)
    * DONE - Parameter types needs to be updated to `PointerArgument` or the gen'd type
    * DONE - The returned adornment type needs to be updated

And now...all the tests in Rocks.Tests pass!

What's left?

* Update Rocks.CodeGenerationTest to use new approach. Only include a couple of assemblies to start to ensure it's working, then do all.

Wait. I think I just realized I don't need to gen Handler and Adornment classes for ref structs. I can just use the generic ones. I can probably gen a generic type for function pointers as well, would need the function pointer type baked in for `ReturnValue`, but the callback can be generic(I may already be doing this)

* DONE - Delegates - this is needed for all esoterics. I can be cleaner by creating a name sans the method name, and get the hash of that. This can be used as a key to ensure only unique delegates are created.
* DONE - Arg
    * Pointers - no need, can use `PointerArgument<>`
    * Function pointers and `ref struct` - create the delegate callback and custom argument type (just like what I'm doing)
* DONE - Handler and adornments - Comment out the code that calls `BuildHandlers()` for this, as I think no work needs to be done here.
    * Pointers - no need, handler can use `PointerArgument<>`, adorments, can return `PointerAdornment` when a return as pointer is needed
    * `ref struct` - no need, just use the generic `Handler` and `Adornment` types (note that in this case, the `ReturnValue` is a delegate, because we need a `ref struct` within a `ref struct` to store state, and that's currently not doable with the current design.)
    * Function pointers - if a return is a function pointer, juse use the `Handler` with no return, and bake in the return value (which I think I'm already doing), so I still need to gen the adornment.

So, this is a fun one `unsafe void*** AsVtblPtr();`. I can create custom delegates and stuff

PointerHandler
PointerArgument
PointerAdornments

I think the great pointer experiment is toast. If I have a `void***`, even if I know the count of pointers, I can't pass `void` to a generic. So I **have** to generate the pointer types, whether they're for arguments or return values.

* Argument - only for argument types that are pointers.
* Handler and Adornments - only for return types that are pointers (note that it does not matter if it's `void` or not).

* The type argument is only made when it's a pointer.
* The constraint is only made when it's a pointer.

OK, down to 7 errors in code gen.

* DONE - On `Instance()` methods, `ConstructorParameter` arguments go first, not last.

YAY! No more test or code gen test errors!

* DONE - Issue with `IHaveRefAndOutV4`, specifically with `ref` parameters, not passed in with `ref`. Actually, it's this one: `public void OutArgument(out int @a)`, the gen'd callback has it as a `ref` and it should be `out`

Man, I just keep going back with esoteric types :(. I don't need the pointer number, but `void****` isn't being recognized as a pointer now. What.

OK, things are working again, integration tests that have been converted are compiling and passing successfully. Success!

Considering a big breaking change. Right now I require generic types to be "closed" when they are given to Rocks. This leads to expectation class names that are...kind of ugly. Furthermore, I have to generate mock infrastructure for each closed type. Even though I've tried to support open generics for a while, I think it's time to consider reversing the rule. That is, instead of the user doing this:

```
[RockCreate<IService<int, string>>]
```

I do this:

```
[RockCreate(typeof(IService<,>))]
```

Note that I can't do this, as it leads to a `CS7003` error:

```
[RockCreate<IService<,>>]
```

So, I create a version of the attribute that isn't generic, and takes one parameter, a `Type`. This is the type that we target. If it's open, we then generate the mock infrastructure with everything open (a preliminary test seems that this is doable).

But, for alpha.1, I think I'll leave it as-is, get everything working (which should be the case as everything was closed generic types before) and deal with the ugly names for now. Then I can look at adding support for open generic types (once and for all, hopefully it's not a bust again like it was with pointers).

I'm really starting to get annoyed adding all the events in. I think I should gen extension methods for all the events based on the adornment type returned when setting up an expectation. Not sure how feasible this will be, because the adornment doesn't have the mock type in, so it's not as straightforward to add in extension methods.

* DONE - Tests to add:
    * DONE - Multiple `ref structs`, pointers, and function pointers
* DONE - Add perf tests to see how generators compare as well as mocking perf
* DONE - Consider letting `RockAttributeGenerator` target types and methods, may make things more convenient.
* DONE - Update Rocks.IntegrationTests to use new approach
* DONE - Remove all non-V4 members and rename V4 members to not have V4. This also means I need to search strings to remove it as well.
    * DONE - Note that Expectations is now in just the Rocks anmespace
* DONE - I feel like I updated the code that is in `INamespaceSymbolExtensionsTests.GetNamespaceSymbol`. I wonder if I can cache things like `references` so I don't keep doing that lookup.
* DONE (just leave it, it's fine) Can WriteLines() be updated with a Write() + WriteLine()?
* DONE - Remove the `using`s and see if that breaks anything.
* DONE - Well. I should move the projected types into the gen'd expectations type. However, this will break stuff, because when they're referenced, they need to include the mock type's namespace (if it exists) + the expectations type name. But the projection namespace will need to be a class.
* Naming. I have the name of the type to mock all over the place within the gen'd Expectations class. I'm thinking I could make the names shorter just to make it arguably easier to read.
    * DONE - Projections - just name it `Projections`. Projected types within this would still have the same names
    * DONE - Rock{MockTypeName} - Could just name it `Mock`, may be easier to read
    * DONE - Expectations - Don't need the name of the mock type, so `IHavePointersMethodExpectations` would be `MethodExpectations`
    * I should also do Shims as they don't need a hash code, just `{Shim}{typeFlattenedName}`

* I don't like that `AddRaiseEvent` requires a string now. Maybe I could gen all the event names, like `EventNames`, and then that class would have constants that would be available.

So, how do I add the extension methods for events?

If the mock type has events, I capture the FQN for each adornment. Then, I generate a static `EventExtensions` class.

For each event
    For each adornment
        I create this:
        
 ```
 internal {AdornmentFQN} Raise{EventName}(this {AdornmentFQN} self, {EventArgsName} args) => 
    self.AddRaiseEvent("{EventName}", args);
 ```

 This could be a lot of extension methods, but...it's better than passing those strings everywhere.

 Once I do this, change all the integration tests that call `AddRaiseEvent` to use these extension methods. It may be better to change one integration test to ensure it's working correctly after a test with events passes. Then I can update all the broken tests and refactor all the integration tests.

* The **only** name collision that we could run into now is if someone has "{Namespace}.{MockType}CreateExpectations" in their code, because I assume that's OK to create it (or the "make" version as well). The chances of this are extremely small, and...arguably, the user could create an alias to their code. However, if they're mocking something that is in the project that they make the mock, that won't help, because an alias can't be made. Again, the odds of this happening are really, **really** small, but...I wonder if I can look in the assembly symbol, or see if I can find a `ITypeSymbol` of my proposed name before I make it, and then do something similar with what I do in `VariableNamingContext` - keep adding 1 until I find one that works.
* BIG OOOOF. I forgot about `RockRepository`. I have no idea how this will work. Maybe I just remove this, and make expectations disposable.
* Update documentation
* Check all TODOs
* Inform Steve (BenchmarkMockNet) that the new version will be breaking, how should it be handled?