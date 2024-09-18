So, where to start with this?

Need to create a `ProjectedModelInformation` type, similar to `MockModelInformation`. When `MockModel.Create()` is called, it needs to uniquely gather all types that match the list:

* Pointers
* `ArgIterator`
* `RuntimeArgumentHandle`
* `TypedReference`

I'm not sure how to return this from `GetMockInformation()`. I also noticed that I'm returning a `List<MockModelInformation>`. Which is OK, but I should have a `HashSet<MockModelInformation>` within, and then return that as a list. Anyway, maybe what I return is a `(List<MockModelInformation> mocks, List<ProjectModelInformation> projections)`, both of which have been gathered using a `HashSet<>`. Maybe I do this as `(ImmutableArray<MockModelInformation> mocks, ImmutableArray<ProjectModelInformation> projections)` (or as a simple record).

For `ProjectedModelInformation`, this should contain just one property: `Target` as a `TypeReferenceModel`. That in and of itself should have all we need, and if we need more, we can add it to `ProjectedModelInformation`.

When `MockModel.Create()` is called, we need to gather all "projected" types based on the list above. This will be pretty tricky. This can come from:

* Method parameters and return types
* Property types
* Indexer parameters and types

Note that we don't have to look through type arguments because these types can't be passed to type arguments. The main usage of projected typed is to get around generic limitations for a small set of types. Since `ref struct`s can be handled naturally via `allows ref struct` in C# 13, these are the only ones left.

Maybe put a `IsProjected` property of type `bool` on `TypeReferenceModel`, and that handles the discovery. Then, when each method, property, and indexer model is created, they're examined to find projected types, and this is gathered within `MockModel.Create()`. That could then be returned.

Now we need to figure out the "shape" of the code that is needed to be created. I think it's something like this:

```c#
namespace {{ProjectedTypeNamespace}}
{
  [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
  internal partial static class Projections
  {
    internal {{isUnsafe}} delegate bool {{validationDelegateName}}({{fullyQualifiedName}} @value){{unmanagedConstraint}};
	
    internal {{isUnsafe}} sealed class {{argName}}{{parameterType}}
      : global::Rocks.Argument{{unmanagedConstraint}}
    {
      // ...argument implementation goes here....
    }
  }
}
```

We can repurpose types to do what is already close to what we need, like `PointerArgTypeBuilder`, `MockProjectedAdornmentsTypesBuilder`, and `MockAdornmentsBuilder`. 

Thought...if I see that I need to make an `int*`, actually generate a "generic" version of this. That way, I can reuse the generic unmanaged type for **any** "single" pointer type. Meaning, `char*` could also be used for that as well. Neat! (if this works)

OK, after much rethinking, I think I have a way through.
* `ProjectedModelInformation` has two properties: `uint PointerCount` and `Type? Type`. Have to break this up into an explicit constructor with just `(Type type)`. If `type` is a pointer, then we get the pointer count, and set `Type` to `null`. Else, we set `Type` to type, and leave `PointerCount` 0.
* When we build the projections, if there is a pointer count, we create a generic pointer argument type using `T : unmanaged`. Else, we create one for both a function pointer and the three types as needed. All of these will go into `Rocks.Projections`, as they will be unique.
* When I create handlers and mock implementations, I create `Callback` nested types and use the projected as needed.
* If the projected type is a return value, I need to add to `ProjectedModelInformation` a `IsUsedInReturnValue`, because that doesn't need to generate a projected `Argument` type, but it does need to generate special `Handler` and `Adornments` types. The `Callback` should be generated in the projected `Handler`. Use `MockProjectedAdornmentsTypesBuilder.BuildTypes()` as a guide.

CRAP.

All of my work is for nothing, because of `void*` pointers. 

Well...I can handle it if I look at the pointer type, and see if it's "pointing at" a void. If **that's** the case, then I have no choice but to generate a `Pointer...VoidArgument` type, similar to what I'm doing with the special types, and not make it generic. Then reference it everywhere. I'm starting to think to get the right "argument" type, I should put that on a `TypeReferenceModel` method, or somewhere to share it everywhere.

Work
* DONE - Update `TypeReferenceModel` to have a `NeedsProjection` property. This will be `true` for pointers and the three special types.
* DONE - Create `ProjectedModelInformation`.
* DONE - Return a `List<ProjectedModelInformation>` from `ForAttributeWithMetadataName()`. This will be a bit tricky, because we want to return the list of 
* DONE - Generate the projections first **before** we reference them in the mock code. This will ensure that the files are created correctly.
  * DONE - Use existing builders and repurpose them. Can also get rid of the hash code generation on the names, and then can probably remove the hash code generation altogether.
  * DONE - Move generated callbacks to the correct `HandlerN` class as a nested definition, and **always** call it `CallbackForHandler` (can't call it `Callback` because there's a property called `Callback` )
  * Update mock, expectation, and adornments, basically anywhere a projected type is used, to use the correct type with the right name.
    * DONE - Expectations - need to use the projected argument name
      * DONE - `MethodExpectationsMethodBuilder`
        * The method arguments are still using the old projected name
      * DONE - `IndexerExpectationsIndexerBuilder`
      * DONE - `PropertyExpectationsPropertyBuilder`
    * DONE - Adornments
      * DONE - Still using the old projected delegate name (need to change what's being put in the adornments pipeline)
* DONE - Add "void pointer" gen-d code to
  * DONE - Handler list
  * DONE - Expectation builders
    * `MethodExpectationsMethodBuilder`
    * `IndexerExpectationsIndexerBuilder`
    * `PropertyExpectationsPropertyBuilder`
* DONE - Need to have a `NeedsProjectedDelegate` and change `NeedsProjection` to `RequiredProjectedArgument`. Reason is for `scoped` parameters.
* DONE - Update mock code to use the new projections as necessary.
* DONE - Tests
  * DONE - Should split out the tests into separate classes
    * DONE - `Pointer`
    * DONE - `MultiplePointer`
    * DONE - `ArgIterator`
    * DONE - `TypedReference`
    * DONE - `RuntimeArgumentHandle`
  * DONE - Should do the "special" types as parameter and return, just to be sure.
  * DONE - Should have multiple special types in one test, to ensure only one projection file is made for each.
  * DONE - For generics
    * DONE - The `Handler` definition is generic, so it needs to be included when `CallbackForHandler` is reference
    * DONE - When the `@handlersN` field is created, the name isn't correct.
  * In IntegrationTests: 
    * DONE - "CSC : error CS8785: Generator 'RockGenerator' failed to generate source. It will not contribute to the output and compilation errors may occur as a result. Exception was of type 'ArgumentException' with message 'The hintName 'Pointer_Projection.g.cs' of the added source file must be unique within a generator.". I believe because an `int*` and `char*` will both be separate `TypeReferenceModel` objects, so then we generate two `Pointer_Projection.g.cs` files. I can reproduce in a unit test, just gotta fix it.
    * CS8909 on function pointer comparison. I believe this is a warning, so I could put a `#pragma` around the comparison in the gen-d argument class. If I do this, I should add docs that explain that devs should take care if they compare function pointers as the results may not be what they expect.
    * `Rocks.IntegrationTests.PointerTestTypes.ISurface_Rock_Create.g.cs(22,53,22,54): error CS0693: Type parameter 'T' has the same name as the type parameter from outer type 'ISurfaceCreateExpectations.Handler0<T>'` - I'm guessing I'm not doing type argument name resolution correctly here. The target type, `ISurface`, is in `PointerTests` class.
* Remove code
  * Can probably remove hash code name generation
  * Can probably (finally!) clean up some of the name generation.
  * Can probably remove "pointer" name stuff from 
  * Can probably delete most of `MockProjectedDelegateBuilder` and all of `MockProjectedTypesBuilder`
  * Not sure I need `ProjectedModelInformation`, I put a lot on `TypeReferenceModel` as it is, if I put `PointerNames` as a `string?` on it, that may do it.
* Update changelog
* Update docs
  * If I allow function pointer comparison, add a note on function pointer comparison to include a warning, referring to these links:
    * https://github.com/dotnet/docs/issues/28782 (see "Comment 5")
    * https://github.com/dotnet/roslyn/issues/48919

TODOs:
* (Handled in "Work" section above) - I'm not sure why I generate a `Handler...` or `Adornments...` with `TCallback` open. I should just set the base type like this: `global::Rocks.Handler<TheCallbackType>`. I can't think of a reason why we should keep the type open. That also means that we generate the `HandlerN` types, the base type name becomes simpler. (I can see why I did it this way because it's consistent with the way handler types are generated for the "normal" path, but I can change that.)

I think I can change it so the `HandlerN` types specify the callback delegate in the type argument of the base declaration. If a delegate needs to be generated, it can be done in the custom `HandlerN` type, like this:

```c#
using System;

public class Argument<T> { }

public class Handler<TCallback>
    where TCallback : Delegate { }

public class Handler0
    : Handler<Handler0.CallbackForHandler>
{
    public delegate void CallbackForHandler(int @value);
    
    public Argument<int> @value { get; set; }
}
```

For "normal" callbacks, an `Action` or `Func<>` can be used (which is already happening). If one needs to be generated for these special types, they can be defined this way. That eliminates the hash code generation for the name, so it's a bit cleaner. Even if there's a "duplicate" needed, and, ironically, I'm trying to reduce code duplication, this should be minimal at best, and it feels simpler than a huge delegate name.