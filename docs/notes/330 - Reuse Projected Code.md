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

Work
* Create `ProjectedModelInformation`.
* Use existing builders and repurpose them. Can also get rid of the hash code generation on the names, and then can probably remove the hash code generation altogether.
  * Move generated callbacks to the correct `HandlerN` class as a nested definition, and **always** call it `Callback`.
* Can probably remove hash code name generation
* Can probably (finally!) clean up some of the name generation.

TODOs:
* (Handled in "Work" section above) - I'm not sure why I generate a `Handler...` or `Adornments...` with `TCallback` open. I should just set the base type like this: `global::Rocks.Handler<TheCallbackType>`. I can't think of a reason why we should keep the type open. That also means that we generate the `HandlerN` types, the base type name becomes simpler. (I can see why I did it this way because it's consistent with the way handler types are generated for the "normal" path, but I can change that.)

I think I can change it so the `HandlerN` types specify the callback delegate in the type argument of the base declaration. If a delegate needs to be generated, it can be done in the custom `HandlerN` type, like this:

```c#
using System;

public class Argument<T> { }

public class Handler<TCallback>
    where TCallback : Delegate { }

public class Handler0
    : Handler<Handler0.Callback>
{
    public delegate void Callback(int @value);
    
    public Argument<int> @value { get; set; }
}
```

For "normal" callbacks, an `Action` or `Func<>` can be used (which is already happening). If one needs to be generated for these special types, they can be defined this way. That eliminates the hash code generation for the name, so it's a bit cleaner. Even if there's a "duplicate" needed, and, ironically, I'm trying to reduce code duplication, this should be minimal at best, and it feels simpler than a huge delegate name.