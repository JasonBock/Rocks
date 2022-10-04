Note that this would be OK:

```csharp
public interface IX
{
    void Foo(object initializationData = null);   
}

public class X : IX
{
    public void Foo(object? initializationData = null)
    {
        throw new System.NotImplementedException();
    }
}
```

But if I end up calling the base class:
```csharp
public class NeedsNullable
{
	public virtual void Initialize(object initializationData = null) { }
}

public class NN : NeedsNullable
{
    public override void Initialize(object? initializationData = null)
    {
        base.Initialize(initializationData);
    }
}
```

This will create a warning, so the call must be done this way:

```csharp
        base.Initialize(initializationData!);
```

So...
* Whenever I need to emit code for a parameter, if it has a default value of `null`, it must have `?` even if the base implementation doesn't have it.
* If I call a base method with that parameter, it must have `!`.

What types? I don't think any of the extension methods need the annotations, just the mock implementation. Well...yes, they do. They need to know that a type that was `string` is now going to be declared as `string?`, which means all the `Arg<>` parameters need the right type as well, though they don't need the parameter to be forced to `!` like it will be when a base or shim method is called.

Back up. I think I need `RequiresNullableAnnotation(this IParameterSymbol)` and `RequiresNullForgiving(this IParameterSymbol)` extension methods, and just use those instead of trying to calculate that everywhere, and it can be "optimized" to just look for default `null` values on a parameter that doesn't have the `?` annotation for reference types.

* DONE - `DelegateBuilder`
* Create
  * DONE - `MockMethodValueBuilder`
  * DONE - `MockMethodVoidBuilder`
  * DONE - `MethodExpectationsExtensionsMethodBuilder`
  * `MockIndexerBuilder`
  * `IndexerExpectationsExtensionsIndexerBuilder`
  * `ExplicitIndexerExpectationsExtensionsIndexerBuilder`
  * `MockConstructorBuilder`
  * `MockConstructorExtensionsBuilder`
* Make
  * `MockMethodValueBuilder`
  * `MockMethodVoidBuilder`
  * `MockIndexerBuilder`
  * `MockConstructorBuilder`
  * `MockConstructorExtensionsBuilder`

Things to check
* `parametersDescription` may not need to be done. It wasn't needed in `MockMethodValueBuilder`...the Make one, I believe