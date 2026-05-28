# Interface Allows Ref Structs As Ref or Ref Readonly Return Values

If an interface uses `allows ref struct` on a type parameter, and that type parameter is used as the return type of a member defined with `ref` or `ref readonly`, there is no reasonable way to mock that interface. The mock would have to be a `ref struct`, and that cannot be assigned to the base interface type. Therefore, Rocks will create this diagnostic in this case.

```csharp
// This will not generate ROCK15
[assembly: Rock(typeof(ITypedReference<>), BuildType.Create)]

public interface ITypedReference<T>
  where T : allows ref struct
{
  T Value { get; }
}

// This will generate ROCK15
[assembly: Rock(typeof(ITypedRefReference<>), BuildType.Create)]

public interface ITypedRefReference<T>
  where T : allows ref struct
{
  ref readonly T Value { get; }
}
```