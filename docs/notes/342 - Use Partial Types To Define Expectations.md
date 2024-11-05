How do I know which type the attribute is on? It `TargetSymbol` on the given `GeneratorAttributeSyntaxContext`. So...that's easy. Then it's just a matter of getting:

* DONE - `IsPartial` - in this case, it would be `true` (the current `[RockAttribute]` would say `false`)
* DONE - `DeclaredAssessibility` - `public` or `internal`
* DONE - `Namespace`, `Name`, etc. - all the stuff to say, "how do we generate the expectation type in the case when it's `partial`"?
* DONE - `IsClass` - is it a `class` or `struct`? Because I think that would be interesting to play with.
* DONE - Maybe allow non-sealed for partial scenario
* DONE - Handle "Make" generation as well
* DONE - Add integration tests
* DONE - Add tests for `[RockPartial]`
* DONE - Update changelog and put in `9.0.0 - Not Yet Released`
* DONE - Add generics tests for partial
* DONE - `RockAnalyzer` needs to be handled differently if `[RockPartial]` is used
* DONE - Update code gen test project to also generate the partial approach

IObservable`1PartialTarget

Can't produce files twice. Pointer files in code gen are getting done twice. Need to only generate once. Should be able to reproduce in a unit test.

My source generator has worked such that my FAWMN call gives back an `IncrementalValuesProvider<MockModelInformation>`. I call `Collect()` on that, and that result gets passed to `RegisterSourceOutput()` on a `IncrementalGeneratorInitializationContext` instance. Within my action, I produce my generated code, along with generating a distinct set of helper types for certain cases that can arise based on types that the user targets.

This all works just fine. However, recently I added another attribute to handle a different scenario, but will still produce the same result. OK, I make another FAWMN call, `Collect()` the `IncrementalValuesProvider<MockModelInformation>` results (note that this is the same exact type that the first one does), and produce source code. The problem is now that there can be a case where both paths need to generate a helper type that has the same file name, and so compilation will fail:

```
ID: CS8785
Description: error CS8785: Generator 'RockGenerator' failed to generate source. It will not contribute to the output and compilation errors may occur as a result. Exception was of type 'ArgumentException' with message 'The hintName 'Pointer_Projection.g.cs' of the added source file must be unique within a generator. (Parameter 'hintName')'.
```

Is there a way to combine the two `IncrementalValuesProvider<MockModelInformation>` results such that I can handle both at the same time, ensuring I don't duplicate code creation? I'm kind of lost figuring out how to do this, even after reading [this](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md#incrementalvaluesprovidert). 

* Rename generated event extensions class so it uses the name of the target type to avoid collisions. Use the name of the partial type when a partial is used.
