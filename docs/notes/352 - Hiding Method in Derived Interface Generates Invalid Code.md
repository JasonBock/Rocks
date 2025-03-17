We may need a `TypeMatch` enumeration:

```c#
internal enum TypeMatch
{
    None,
    DifferByConstraintsOnly,
    Exact
}
```

This could be returned by `ITypeSymbolExtensions.IsMatch()`, and interrogated at the call site. Then we would also need to update `MethodMatch`:

```c#
internal enum MethodMatch
{
   Exact,
   DifferByReturnTypeOnly,
   DifferByConstraintsOnly,
   None,
}
```

Or...

```c#
internal enum MethodMatch
{
   Exact,
   DifferByReturnTypeOrConstraintsOnly,
   None,
}
```


Then when we are trying to determine if a method should be implemented explicitly (which should only be with interfaces), we can take into consideration the `Differ...`. Or we could "merge" the `DifferBy...` values together. May be better to keep them separated.

TODO:
* Should I also do this with an equivalent "class" configuration?
* Because I changed ordering of type constraints, some unit tests might fail because constraint generation is out-of-order.