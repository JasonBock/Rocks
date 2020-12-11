# Esoteric type support

This came up if an interface is created like this:

```
unsafe public interface IHavePointer
{
  void Foo(int* value);
}
```

This is a perfectly valid thing to do in C# (although I've never seen it actually happen), but Rocks couldn't support this because the expectations relied on Expressions, and it didn't like pointer types. Now, with source generators, I think I can actually do this.

First, we need to define what a "esoteric" or "special" type is. It basically comes down to CS0306, which I found in Roslyn looks like this:

http://sourceroslyn.io/#Microsoft.CodeAnalysis.CSharp/Symbols/TypeSymbolExtensions.cs,352

```
public static bool IsPointerOrFunctionPointer(this TypeSymbol type)
        {
            switch (type.TypeKind)
            {
                case TypeKind.Pointer:
                case TypeKind.FunctionPointer:
                    return true;
 
                default:
                    return false;
            }
        }
```

http://sourceroslyn.io/#Microsoft.CodeAnalysis.CSharp/Symbols/TypeSymbolExtensions.cs,1121

```
        internal static bool IsRestrictedType(this TypeSymbol type,
                                                bool ignoreSpanLikeTypes = false)
        {
            // See Dev10 C# compiler, "type.cpp", bool Type::isSpecialByRefType() const
            RoslynDebug.Assert((object)type != null);
            switch (type.SpecialType)
            {
                case SpecialType.System_TypedReference:
                case SpecialType.System_ArgIterator:
                case SpecialType.System_RuntimeArgumentHandle:
                    return true;
            }
 
            return ignoreSpanLikeTypes ?
                        false :
                        type.IsRefLikeType;
        }
```
	
So, basically, if the given type has any parameters or return values that have a type kind == Pointer or FunctionPointer, or IsRefLikeType == true, then we have to generate a bunch of "special" things. Primarily we need generated types that are used when expectations are set.