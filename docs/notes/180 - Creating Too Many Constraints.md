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

Oddly enough, decompiling the code in SharpLab gives this:

```csharp
public class UnmanagedValue : IUnmanagedValue
{
    public void Use<[IsUnmanaged] TValue>(Value<TValue> value) where TValue : struct, IValue<TValue>
    {
        throw new NotImplementedException();
    }
}
```

It seems to ignore `unmanaged` and just uses `struct`. This doesn't mean a lot, but it is interesting to note. Though it does have `[IsUnmanaged]`...

ILSpy, if set to C# 10 decompiliation, gives this:

```csharp
public void Use<TValue>(Value<TValue> value) where TValue : unmanaged, IValue<TValue>
{
	throw new NotImplementedException();
}
```

But C# 5 gives this:

```csharp
public void Use<[System.Runtime.CompilerServices.IsUnmanaged] TValue>(
	[System.Runtime.CompilerServices.Nullable(new byte[] { 1, 0 })] Value<TValue> value) 
	where TValue : struct, [System.Runtime.CompilerServices.Nullable(new byte[] { 1, 0 })] IValue<TValue>
{
	throw new NotImplementedException();
}
```

Interesting that the constraint seems to be encoded as an `IsUnmanagedAttribute`. Looking at IL, that's what it is:

```csharp
.method public final hidebysig newslot virtual 
	instance void Use<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime]System.Runtime.InteropServices.UnmanagedType), class Testy.IValue`1<!!TValue>) TValue> (
		class Testy.Value`1<!!TValue> 'value'
	) cil managed 
{
	.param type TValue
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)
	.param constraint TValue, class Testy.IValue`1<!!TValue>
		.custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8[]) = (
			01 00 02 00 00 00 01 00 00 00
		)
	.param [1]
		.custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8[]) = (
			01 00 02 00 00 00 01 00 00 00
		)
	// Method begins at RVA 0x20b0
	// Header size: 1
	// Code size: 6 (0x6)
	.maxstack 8

	IL_0000: newobj instance void [System.Runtime]System.NotImplementedException::.ctor()
	IL_0005: throw
} // end of method UnmanagedValue::Use
```