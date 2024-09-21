* Change `No handlers were found` to `No handlers were provided`, and in that case, there's no need to format parameter values.
* Create an extension method for the `No handlers match for` case, and reuse that everywhere.

* MockMethodValueBuilder
* MockMethodVoidBuilder
* MockPropertyBuilder
* MockIndexerBuilder

* Could we make `Argument<>` defined such that it could take a collection expression on construction?
* `IDE0058` seems to collide with fluent APIs - https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0058