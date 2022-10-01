This is interesting. The code example causes Rocks to emit:

```csharp
where TValue : unmanaged, struct, global::IValue<TValue>
```

Which is wrong, according to this compiler error:

```csharp
error CS0449: The 'class', 'struct', 'unmanaged', 'notnull', and 'default' constraints cannot be combined or duplicated, and must be specified first in the constraints list.
```

What's interesting is that if I feed the code to SharpLab and tell it to implement `IUnmanagedValue`, I get correct code:

```csharp
public class UnmanagedValue
    : IUnmanagedValue
{
    public void Use<TValue>(Value<TValue> value) where TValue : unmanaged, IValue<TValue>
    {
        throw new System.NotImplementedException();
    }
}
```

Oddly enough, decompiling the code gives this:

```csharp
public class UnmanagedValue : IUnmanagedValue
{
    public void Use<[IsUnmanaged] TValue>(Value<TValue> value) where TValue : struct, IValue<TValue>
    {
        throw new NotImplementedException();
    }
}
```

It seems to ignore `unmanaged` and just uses `struct`. This doesn't mean a lot, but it is interesting to note.