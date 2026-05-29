The diagnostic looks like this:

```
ID: CS9192
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\DotNext.IOptionMonadT_Rock_Create.g.cs(436,64): error CS9192: Argument 1 should be passed with 'ref' or 'in' keyword
Code:
@args
```

Here are the current types failing:

* `DotNext.IOptionMonad<T>`
* `DotNext.IResultMonad<T>`
* `DotNext.Buffers.IGrowableBuffer<T>`
* `DotNext.IO.IAsyncBinaryWriter`

Test is passing. Dump the code from the unit test into MockCodeDump.cs and see if it succeeds or fails there.

Well, it fails. So that's consistent in the code-gen app, but why is it different.

I think there's a couple of things to fix. First, the generated delegate doesn't include `ref readonly`, and it probably should. So change it from this:

```c#
internal delegate void CallbackForHandler(global::Variant @args, int @count, scoped global::Variant @result);
```

to this:

```c#
internal delegate void CallbackForHandler(ref readonly global::Variant @args, int @count, scoped global::Variant @result);
```

The same thing goes for the shim. Change from this:

```c#
public void DynamicInvoke(global::Variant @args, int @count, scoped global::Variant @result)
```

to this:

```c#
public void DynamicInvoke(ref readonly global::Variant @args, int @count, scoped global::Variant @result)
```

Then I need to detect that when I call a method that has a parameter that is `ref readonly`, `in` needs to be added to the forwarded call:

```c#
// In the mock:
@handler.Callback?.Invoke(in @args!, @count!, @result!);

// In the shim:
public void DynamicInvoke(ref readonly global::Variant @args, int @count, scoped global::Variant @result) =>
    ((global::IOptionMonad<T>)this.mock).DynamicInvoke(in @args, @count, @result);
```

TODOs:
* DONE - Update delegate generation (create only)
* DONE - Update shim generation (create only)
* DONE - Use `in` for `ref readonly` parameter forwarding (create only)
    * DONE - In mocked method
    * DONE - In shim method
* DONE - Add a bug that if someone defines has a mock that has a member named `Mock`, it'll break Rocks (or at least confirm I'm already handling this)
* DONE - Add a section on default interface members and shims in the Overview doc.
* DONE - There's a space in the shim member generation, fix that! (horrible)