# Consider Writing a Suppressor for CA2012

https://github.com/JasonBock/Rocks/issues/354

Docs:
* Design - https://github.com/dotnet/roslyn/blob/main/docs/analyzers/DiagnosticSuppressorDesign.md
* Class - https://learn.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticsuppressor?view=roslyn-dotnet-4.13.0

* https://github.com/Flash0ver/F0.Analyzers/blob/main/source/production/F0.Analyzers/CodeAnalysis/Suppressors/CA1707IdentifiersShouldNotContainUnderscoresSuppressor.cs
* https://github.com/Flash0ver/F0.Analyzers/blob/main/source/test/F0.Analyzers.Tests/CodeAnalysis/Suppressors/CA1707IdentifiersShouldNotContainUnderscoresSuppressorTests.cs

* Create `Rocks.Scenarios.slnx` and a new `Rocks.Scenarios.csproj` project, similar to what I do in Transpire, so I can see the analyzers and suppressors in action.
* Create the `ValueTaskInReturnValueSuppressor` class
* Write tests
* Make the expectations check that if `ValueTask` or `ValueTask<>` is passed to `ReturnValue` and `ExpectedCallCount` is greater than 1, **maybe** throw an exception. Need to think about this.