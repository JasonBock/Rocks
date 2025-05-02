Syntax nodes to look for:

* IdentifierName
* ClassDeclarationSyntax
* InterfaceDeclarationSyntax
* RecordDeclarationSyntax
* ParameterSyntax
* ObjectCreateExpressionSyntax

Note: to have a shared library for shared work, we'd want a "Rocks.Compilation" package that both "Rocks.Analysis" and "Rocks.Completions" both reference

For configuration:
"I believe the idea is you add a .globalconfig file to the test states AnalyzerConfigFiles"
"you can use context.TextDocumnt.Project.AnalyzerOptions to get to the options"
"IIRC you can override a method in the child class of CSharpCodeRefacgoringTest and you get passed the solution that it creates, and you can do whatever you like in that too"

* https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md#access-analyzer-config-properties
* https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md#consume-msbuild-properties-and-metadata

The 2nd link is probably what I want.

TODO:
* DONE - Need to add project to package
* DONE - Add `BuildType.Make` as well
* DONE - Figure out how to add project configuration values to specify a different file
* DONE - Look for any nodes after the last created attribute list node to determine if one or two lines should be added
* DONE - Add more tests
    * DONE - For all target syntax nodes
    * DONE - When the cursor won't "parent up" to a target syntax nodes
    * DONE - When multiple refactorings are offered
* DONE - Look at removing the `<NoWarn>` elements at this point.
* DONE - Should consider not adding a `using` for the mock type and just put in a fully qualified one. That would remove ambiguity
* DONE - Need to handle an open generic scenario
* DONE - Update docs so people know about the refactoring.
