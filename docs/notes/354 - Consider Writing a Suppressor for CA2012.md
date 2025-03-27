# Consider Writing a Suppressor for CA2012

https://github.com/JasonBock/Rocks/issues/354

Docs:
* Design - https://github.com/dotnet/roslyn/blob/main/docs/analyzers/DiagnosticSuppressorDesign.md
* Class - https://learn.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticsuppressor?view=roslyn-dotnet-4.13.0

* https://github.com/Flash0ver/F0.Analyzers/blob/main/source/production/F0.Analyzers/CodeAnalysis/Suppressors/CA1707IdentifiersShouldNotContainUnderscoresSuppressor.cs
* https://github.com/Flash0ver/F0.Analyzers/blob/main/source/test/F0.Analyzers.Tests/CodeAnalysis/Suppressors/CA1707IdentifiersShouldNotContainUnderscoresSuppressorTests.cs

Analyzer defintion: 
* Package: `Microsoft.CodeAnalysis.NetAnalyzers`
* Class: `UseValueTasksCorrectly` - https://github.com/dotnet/roslyn-analyzers/blob/1e98fa3f107e780569a167a9250bb600778456dc/src/NetAnalyzers/Core/Microsoft.NetCore.Analyzers/Tasks/UseValueTasksCorrectly.cs


Huh??

```
    System.TypeInitializationException : The type initializer for 'Microsoft.CodeAnalysis.Testing.Lightup.ProgrammaticSuppressionInfoWrapper' threw an exception.
  ----> System.InvalidOperationException : Property 'System.Collections.Immutable.ImmutableArray`1[Microsoft.CodeAnalysis.Diagnostics.Suppression] Suppressions' produces a value of type 'System.Collections.Immutable.ImmutableArray`1[Microsoft.CodeAnalysis.Diagnostics.Suppression]', which is not assignable to type 'System.Collections.Immutable.ImmutableHashSet`1[System.ValueTuple`2[System.String,Microsoft.CodeAnalysis.LocalizableString]]'

  Stack Trace: 
ProgrammaticSuppressionInfoWrapper.FromInstance(Object instance)
DiagnosticExtensions.ProgrammaticSuppressionInfo(Diagnostic diagnostic)
<>c.<FilterDiagnostics>b__103_0(ValueTuple`2 d)
ArrayWhereIterator`1.ToArray(ReadOnlySpan`1 source, Func`2 predicate)
ArrayWhereIterator`1.ToArray()
Enumerable.ToArray[TSource](IEnumerable`1 source)
ImmutableArray.CreateRange[T](IEnumerable`1 items)
ImmutableArray.ToImmutableArray[TSource](IEnumerable`1 items)
AnalyzerTest`1.FilterDiagnostics(ImmutableArray`1 diagnostics)
AnalyzerTest`1.GetSortedDiagnosticsAsync(Solution solution, ImmutableArray`1 analyzers, ImmutableArray`1 additionalDiagnostics, CompilerDiagnostics compilerDiagnostics, IVerifier verifier, CancellationToken cancellationToken)
AnalyzerTest`1.GetSortedDiagnosticsAsync(EvaluatedProjectState primaryProject, ImmutableArray`1 additionalProjects, ImmutableArray`1 analyzers, IVerifier verifier, CancellationToken cancellationToken)
AnalyzerTest`1.VerifyDiagnosticsAsync(EvaluatedProjectState primaryProject, ImmutableArray`1 additionalProjects, DiagnosticResult[] expected, IVerifier verifier, CancellationToken cancellationToken)
AnalyzerTest`1.RunImplAsync(CancellationToken cancellationToken)
AnalyzerTest`1.RunAsync(CancellationToken cancellationToken)
TestAssistants.RunSuppressorAsync[TDiagnosticSuppressor,TDiagnosticAnalyzer](String code, IEnumerable`1 expectedDiagnostics, OutputKind outputKind, IEnumerable`1 additionalReferences) line 97
ValueTypeInReturnValueSuppressorTests.SuppressWhenValueTaskIsPassedToReturnValueAsync() line 48
```

Interesting, it might be a bug: https://github.com/dotnet/roslyn-analyzers/issues/7541

* DONE - Create `Rocks.Scenarios.slnx` and a new `Rocks.Scenarios.csproj` project, similar to what I do in Transpire, so I can see the analyzers and suppressors in action.
* DONE - Create the `ValueTaskInReturnValueSuppressor` class
* Write tests

TODOs:
* Make the expectations check that if `ValueTask` or `ValueTask<>` is passed to `ReturnValue` and `ExpectedCallCount` is greater than 1, **maybe** throw an exception. Need to think about this.
* Complain to .NET team (lol) about CA2012 and why it flags for `ValueTask.FromResult("done")` but not `new ValueTask<string>("done")`
* Write a refactoring to create the `[Rock]` attribute
* Capture the issue about running CodeGenerationTests with too many tasks