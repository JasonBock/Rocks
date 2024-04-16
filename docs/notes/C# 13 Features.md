# C# 13 Features

This document contains notes on how I'll handle C# 13 features in Rocks. This document doesn't necessarily reflect any eventual implementation in Rocks that I did. Consider this to be a bunch of notes that I made as I reviewed these features, and the work I actually did may (and probably does) look different.

I'm pulling the feature list from [here](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md#c-next).

## Features

### [Ref Struct Interfaces](https://github.com/dotnet/csharplang/issues/7608)

This may have impact on Rocks, especially as it pertains to generating projected types. I'm currently tracking this [here](https://github.com/JasonBock/Rocks/issues/170). I may also need to investigate if this affects generated code.

### [Semi-auto-properties](https://github.com/dotnet/csharplang/issues/140)

This was removed in C# 12 - looks like it's coming in C# 13. Doesn't seem like it will affect code generation. I hope this makes it. I may ditch any field declaractions by default and go all-in on properties :).

### [Default in deconstruction](https://github.com/dotnet/roslyn/pull/25562)

Should have no impact on Rocks.

### [Roles/Extensions](https://github.com/dotnet/csharplang/issues/5497)

This is a feature that's been in discussion for years. I'm not quite sure where the current syntax has landed, and what it does exactly. It may have a lot of impact, or it may be minimal - it's hard to tell at this point.

### [Escape character](https://github.com/dotnet/csharplang/issues/7400)

Should have no impact on Rocks.

### [Method group natural type improvements](https://github.com/dotnet/csharplang/blob/main/proposals/method-group-natural-type-improvements.md)

Should have no impact on Rocks.

### [Lock object](https://github.com/dotnet/csharplang/issues/7104)

Should have no impact on Rocks.

### Implicit indexer access in object initializers

Unknown - there's no information on what this actually is.

### [Params-collections](https://github.com/dotnet/csharplang/issues/7700)

This probably won't have any impact, but I should confirm this with tests.

## Conclusion

It seems like C# 13 may have some impact on Rocks. There are some features to test to ensure that no changes are needed.