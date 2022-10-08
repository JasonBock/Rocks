So, you **can** specific the `class` or `struct` constraints on an overload, but it's not required. Here's the error message (CS0460):

```
"Constraints for override and explicit interface implementation methods are inherited from the base method, so they cannot be specified directly, except for either a 'class', or a 'struct' constraint."
```

So...do we **never** specify constraints for **any** method implementation? They are all overrides or explicit interface implementations, so...I think I can just remove all constraint generation on methods.

Classes are different, we still need to do it for classes.

I thought I had tests for this for generics on methods, but...I'm thinking I don't. 

Anyway, let's remove adding constraints to methods (though I think they should stay on the extension methods):

Oops. if I defined a class like this:

```
public abstract class AbstractClassGenericMethod<T>
{
	public abstract TData? NullableValues<TData>(TData? data);
}
```

I have to define the overridden method like this:

```
public class X : AbstractClassGenericMethod<int>
{
    public override TData? NullableValues<TData>(TData? data) where TData : default
    {
        throw new System.NotImplementedException();
    }
}
```

So, maybe 

* DONE - Create
  * DONE - `MockMethodValueBuilder`
  * DONE - `MockMethodVoidBuilder`
* DONE - Make
  * DONE - `MockMethodValueBuilder`
  * DONE - `MockMethodVoidBuilder`