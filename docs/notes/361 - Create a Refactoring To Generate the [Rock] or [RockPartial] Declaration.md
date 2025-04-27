Syntax nodes to look for:

* ClassDeclarationSyntax
* InterfaceDeclarationSyntax
* RecordDeclarationSyntax
* ParameterSyntax
* ObjectCreateExpressionSyntax
* IdentifierName

TODO:
* Need to add project to package
* Add more tests
    * For all target syntax nodes
    * When the cursor won't "parent up" to a target syntax nodes
* Figure out how to add project configuration values to specify a different file
* Look for any nodes after the last created attribute list node to determine if one or two lines should be added
* Add `BuildType.Make` as well
* Consider doing a `[RockPartial]` - that might be harder. Maybe only do it when the cursor is on a `partial` `class` type, and then it goes directly on that class definition
* Look at removing the `<NoWarn>` elements at this point.
* Random thought...have I ever tried covariance and contravariance? I would imagine that at this point, I would've seen an issue if I wasn't handling it correctly, but maybe I should have unit+integration tests in place if I don't already.