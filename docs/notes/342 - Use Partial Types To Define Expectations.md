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
* Add generics tests for partial
* `RockAnalyzer` needs to be handled differently if `[RockPartial]` is used
* Update code gen test project to also generate the partial approach