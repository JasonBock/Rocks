So, what's the easiest way to know that a text value, like `public` or `event` is a C# keyword?

This may be what I'm looking for: https://sourceroslyn.io/#Microsoft.CodeAnalysis.CSharp/Syntax/SyntaxKindFacts.cs,820

But, I just found out, I can put `@` in front of **every** parameter name no matter what, and that's fine. So...well, that would fix it :).

I would only do this when I create the method/indexer/constructor definition, and then whenever I'm referencing the name of the parameter. VariableNamingContext would still use the literal name of the parameter for comparison, though I don't think I would ever use a name for a variable that would be a C# keyword anyway.

Types to look at:

* Create
  * Methods
    * DONE - `MockMethodValueBuilder`
    * DONE - `MockMethodVoidBuilder`
    * DONE - `MethodExpectationsExtensionsMethodBuilder`
    * DONE - `MockConstructorExtensionsBuilder`
	* DONE - `MockConstructorBuilder`
	* Well, really, all the extension methods, because they can all create `self`
  * Indexers
    * `MockIndexerBuilder`
    * `IndexerExpectationsExtensionsIndexerBuilder`
    * `ExplicitIndexerExpectationsExtensionsIndexerBuilder`
* Make
  * `MockConstructorBuilder`
  * `MockConstructorExtensionsBuilder` (note that this needs the namingContext for `constructorProperties`)
  * `MockIndexerBuilder`
  * `MockMethodValueBuilder`
  * `MockMethodVoidBuilder`

Other things
* Make sure `foundMatch` is created with a context