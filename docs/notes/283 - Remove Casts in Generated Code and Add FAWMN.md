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

# Update Member Handling

We no longer need casts. Just need to modify which list the member iterates.

# Create Custom Expectations Classes

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