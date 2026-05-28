The diagnostic looks like this:

```
ID: CS9192
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\DotNext.IOptionMonadT_Rock_Create.g.cs(436,64): error CS9192: Argument 1 should be passed with 'ref' or 'in' keyword
Code:
@args
```

Here are the current types failing:

* DotNext.IOptionMonad<T>
* DotNext.IResultMonad<T>
* DotNext.Buffers.IGrowableBuffer<T>
* DotNext.IO.IAsyncBinaryWriter