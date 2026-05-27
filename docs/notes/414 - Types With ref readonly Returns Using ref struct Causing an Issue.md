Diagnostics:

```
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ITypedReferenceT_Rock_Create.g.cs(75,11): error CS8345: Field or auto-implemented property cannot be of type 'T' unless it is an instance member of a ref struct.
    DiagnosticResult.CompilerError("CS8345").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ITypedReferenceT_Rock_Create.g.cs", 75, 11, 75, 12).WithArguments("T"),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ITypedReferenceT_Rock_Make.g.cs(22,11): error CS8345: Field or auto-implemented property cannot be of type 'T' unless it is an instance member of a ref struct.
    DiagnosticResult.CompilerError("CS8345").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ITypedReferenceT_Rock_Make.g.cs", 22, 11, 22, 12).WithArguments("T"),
```

I think the problem is this. An interface has a type parameter (call it `T`) constrained with `allows ref struct`. That type parameter is used with a return value (or probably an `out` parameter as well), but as a `ref T`. This forces **everything** to be a `ref struct`, which causes all sorts of problems.

We could just not put the anticonstraint on the custom expectation type, but that means you can't use `ref struct` types with the interface, which kind of limits things.

It seems like I can just make the mock type `public` (with it being a `ref readonly struct` and passing return values in with constructor parameters), and make the `Instance(...)` return values the mock type. That gets rid of all the compiler errors. However, this makes me wonder...since I can't do this:

```c#
var stringValue = "Jason";
var stringReference = new LocalReference<string>(ref stringValue);
TakeTypedReference(ref stringReference);

static void TakeTypedReference(ref ITypedReference<string> type) { }
```

This won't work - I'll get a `CS1503` when I try to call `TakeTypedReference()`.

Honestly, at this point, I think the better thing to do is to not handle a type definition like this, because mocking it has no benefit. You can't pass the mock into something defined as the interface because it must be a `ref struct` and that isn't allowed.

(Side note, I think [this discussion](https://github.com/dotnet/csharplang/discussions/8392)) is what got Copilot off the rails during my stream :).

Here's the rule: If an interface has a type parameter constrained with `allows ref struct` and that type parameter is used as the type `ref` or `ref readonly` return value, we can't mock it. Reason is, we'd have to make the mock type a `ref struct`, and you can't pass a `ref struct` to a parameter of the interface type because of the rules around `ref struct` - it would cause boxing.

TODO:
* DONE - Detect when `allows ref struct` is on a type parameter that's used as a `ref` return value (or probably an `out` parameter as well, not sure). In this case, we create a diagnostic
* DONE - Run all integration and code gen tests
* Add a test for this diagnostic
* Add a unit test for interfaces that `allows ref struct` but just returns it as `T` (i.e. not a `ref` or `ref readonly`)
* Change diagnostics and descriptors to use IDs defined in a static class (similar to what I did in Transpire)
* Update docs to talk about conditions that Rocks doesn't support:
    * Types with generic parameters that are constraints with `allows ref struct` where that type is used as a `ref` or `ref return`
    * Static abstract members in interfaces
    * Sealed, non-virtuals, etc. (profiler API could be used, but would be very difficult, reference TypeMock)
