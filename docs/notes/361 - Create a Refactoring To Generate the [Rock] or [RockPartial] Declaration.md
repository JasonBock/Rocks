Syntax nodes to look for:

* IdentifierName
* ClassDeclarationSyntax
* InterfaceDeclarationSyntax
* RecordDeclarationSyntax
* ParameterSyntax
* ObjectCreateExpressionSyntax

Note: to have a shared library for shared work, we'd want a "Rocks.Compilation" package that both "Rocks.Analysis" and "Rocks.Completions" both reference

TODO:
* DONE - Need to add project to package
* DONE - Add `BuildType.Make` as well
* Figure out how to add project configuration values to specify a different file
* DONE - Look for any nodes after the last created attribute list node to determine if one or two lines should be added
* Add more tests
    * DONE - For all target syntax nodes
    * DONE - When the cursor won't "parent up" to a target syntax nodes
    * DONE - When multiple refactorings are offered
* Look at removing the `<NoWarn>` elements at this point.
