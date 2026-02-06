# C# 15 Features

This document contains notes on how I'll handle C# 15 features in Rocks. This document doesn't necessarily reflect any eventual implementation in Rocks that I did. Consider this to be a bunch of notes that I made as I reviewed these features, and the work I actually did may (and probably does) look different.

I'm pulling the feature list from [here](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md#c-next).

## Features

### [Dictionary expressions](https://github.com/dotnet/csharplang/blob/main/proposals/dictionary-expressions.md)

This shouldn't affect Rocks in terms of code generation. Maybe it can be used internally by Rocks, but this shouldn't cause code gen to fail.

### [Unions](https://github.com/dotnet/csharplang/issues/9662)

I have the feeling this will have an effect on code generation. At the very least, it must be supported if a union is a parameter, return type, etc. I'm not sure if unions can be inherited from - if so, that may also have a (potentially big) effect on code generation. I'm not sure where this could be useful within Rocks itself.

### [Closed class hierarchies](https://github.com/dotnet/csharplang/issues/9499)

Definitely could have impact on code generation. I'll probably have to look at the type to see if it's "closed", which has a different meaning than "sealed". Not sure if it's needed within Rocks.

### [Unsafe evolution](https://github.com/dotnet/csharplang/blob/main/proposals/unsafe-evolution.md)

Definitely could have impact on code generation if unsafe members/declarations (e.g. pointers) are in play. Not sure if it's needed within Rocks.

### [Extension indexers](https://github.com/dotnet/csharplang/blob/main/proposals/extension-indexers.md)

Probably has no impact to either code generator or internal Rocks usage.

### [ExtendedLayoutAttribute](https://github.com/dotnet/csharplang/blob/main/proposals/extension-indexers.md)

This **might** have an impact if this is declared on type, and if that attribute declaration should or should not be carried to the mock type. Probably has no internal impact to Rocks.

### [Runtime Async](https://github.com/dotnet/runtime/blob/main/docs/design/specs/runtime-async.md)

My guess is that this is purely a runtime concern, so it should have no impact to code generator or internal implementation. That said...it **might** change things with respect to how asynchronous calls are done? I highly doubt it, but I don't know enough about the feature right now to say that with certainty.

### [Collection expression arguments](https://github.com/dotnet/csharplang/blob/main/proposals/collection-expression-arguments.md)

Should have little to no impact on code generation. May be useful internally to Rocks.

## Conclusion

It seems like C# 15 may have some impact on Rocks. Some features might have a fairly substantial effect on code generation. Testing will definitely need to be done as preview versions of C# 15/.NET 11 are released.