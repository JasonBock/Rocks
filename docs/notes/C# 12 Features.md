# C# 12 Features

This document contains notes on how I'll handle C# 12 features in Rocks. This doesn't cover all the C# 12 features; I'm only focused on those that I think will have significant impact on how Rocks works.

Also, this document doesn't necessarily reflect any eventual implementation in Rocks that I did. Consider this to be a bunch of notes that I made as I reviewed these features, and the work I actually did may (and probably does) look different.

I'm pulling the feature list from [here](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md#c-next).

## In Play

### [ref readonly parameters](https://github.com/dotnet/csharplang/issues/6010)

This may have some impact in that Rocks would need to ensure that a `ref readonly` parameter is generated correctly in the mock. But that should be it.

### [Collection Expressions](https://github.com/dotnet/csharplang/issues/5354)

This shouldn't change anything that needs to be generated. I may want to use this within Rocks.

### [Inline Arrays](https://github.com/dotnet/csharplang/blob/main/proposals/inline-arrays.md)

My guess is that this won't have an impact, but I should review this more deeply. For example, if a method has a parameter that is marked with `[InlineArray]`, does that have any downstream effects on Rocks code gen? I'm thinking "no", but it's something to test out.

Using it within Rocks is a possibility, but I'm not sure where.

### [nameof accessing instance members](https://github.com/dotnet/csharplang/issues/4037)

Should have no impact.

### [Using aliases for any type](https://github.com/dotnet/csharplang/issues/4284)

Should have no impact.

### [Primary Constructors](https://github.com/dotnet/csharplang/issues/2691)

Maybe I use this within Rocks, though from what I've been able to glean, I'm not sure I'm totally on-board with this. From the consumer side, I don't think this will impact any code gen.

### [Params Span<T> + Stackalloc any array type](https://github.com/dotnet/csharplang/issues/1757)

Just have to confirm this has no issues with current code gen. I don't actually use `params` within Rocks.

### [Lambda default parameters](https://github.com/dotnet/csharplang/issues/6051)

A nice addition for lambdas and method groups, but this shouldn't have any impact on Rocks.

### [Default in deconstruction](https://github.com/dotnet/roslyn/pull/25562)

Should have no impact.

## Experimental Features

There are some features within C# 12 that will not be considered finalized, but they may end up in a future version. I think it's worth looking at what they are and what impact they could have.

### [Interceptors](https://github.com/dotnet/csharplang/issues/7009)

This may have some very interesting promise. I'm going to write a separate set of notes on this.

### [Roles/Extensions](https://github.com/dotnet/csharplang/issues/5497)

This is a feature that's been in discussion for years. I'm not quite sure where the current syntax has landed, and what it does exactly.

## Deferred

### [Semi-auto-properties](https://github.com/dotnet/csharplang/issues/140)

This may show up in a future version, but it won't be in C# 12. It wouldn't have affected code gen, but...I was really hoping this would make it. I would've ditched any field declaractions by default and went all-in on properties :).

## Conclusion

It seems like C# 12 should have minimal impact on Rocks itself. There are some features to test to ensure that no changes are needed.