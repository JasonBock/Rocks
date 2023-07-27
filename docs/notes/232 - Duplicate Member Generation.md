So, having this code:

```csharp
public abstract class ValueComparer
{
	public abstract object? Snapshot(object? instance);
}

public class ValueComparer<T>
	: ValueComparer
{
	public override object? Snapshot(object? instance) => null;

	public virtual T Snapshot(T instance) => default!;
}

public class GeometryValueComparer<TGeometry>
	: ValueComparer<TGeometry>
{ }
```

If I ask VS to generate overrides, I get this:

```csharp
public class X : GeometryValueComparer<object>
{
   public override bool Equals(object? obj) => base.Equals(obj);
   public override int GetHashCode() => base.GetHashCode();
   public override object? Snapshot(object? instance) => base.Snapshot(instance);
   public override object Snapshot(object instance) => base.Snapshot(instance);
   public override string? ToString() => base.ToString();
}
```

Both `Snapshot()` members cannot be implemented. Commenting out one of them doesn't fix it either.

I think the problem is that by setting `TGeometry` to `object`, you end up with a method on the base class that already does what you want. But you can't override it. Note that setting `TGeometry` to something other than `object`, like `string`, all 5 overrides work just fine.

So, I think in `GetMockableMethods`, if I find an exact match, I should not include the existing one in the list, nor should I include the one I just found.

I think here's the problem:

```csharp
var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod) == MethodMatch.None) &&
    !_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));
```

I'm only considering matches that do not exist on the same type. As this example shows, `ValueComparer<object>` leads to this exact condition.

* Change it to only do the first one.
* Then, if it is not `null`:
    * Remove it from the current list.
    * Add the check for a different type with this: `if (!hierarchyMethod.IsSealed)`