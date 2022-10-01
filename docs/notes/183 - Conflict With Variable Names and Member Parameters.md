Just created `VariableNamingContext`. Now need to generate names using this in:

* Methods
  * DONE - `MockMethodValueBuilder`
  * `MockMethodVoidBuilder`
  * `MethodExpectationsExtensionsMethodBuilder`
  * `MockConstructorExtensionsBuilder`
* Indexers
  * `MockIndexerBuilder`
  * `IndexerExpectationsExtensionsIndexerBuilder`
  * `ExplicitIndexerExpectationsExtensionsIndexerBuilder`
  
Note: `VariableNamingContext` will not hold the literal name of a parameter (i.e. `@event` if the name is `event`). I'll create an extension method called `GetLiteralName(this IParameterSymbol self)` to do this in https://github.com/JasonBock/Rocks/issues/184