* Look to see if there's any way to get a list of members from a type that can be overriden from Roslyn. (That would make things so much easier in the long run)
* We need to find a way to handle parameter matching "correctly" for this case. Look at the symbol original definition type kind. If it's a method, we don't care, but if it's a type, we DO care. Meaning, look at something like this: `namedLeft.TypeArguments[0].OriginalDefinition.ContainingSymbol.Kind`. If `Kind` is a `Method` for both, then we don't care (or maybe we just positionally, look at the `MatchWhenMethodsDifferByGenericParameterName()` test), but if it's something like `NamedType`, then if the names are different, that IS different.

ID: CS0111
Description: Rocks\Rocks.RockGenerator\System.Reactive.Concurrency.HistoricalScheduler_Partial_Rock_Create.g.cs(518,124): error CS0111: Type 'HistoricalSchedulerPartialTarget.MethodExpectations' already defines a member called 'ScheduleAbsolute' with the same parameter types
Code:
ScheduleAbsolute

ID: CS0111
Description: Rocks\Rocks.RockGenerator\System.Reactive.Concurrency.VirtualTimeSchedulerTAbsolute, TRelative_Partial_Rock_Make.g.cs(72,47): error CS0111: Type 'VirtualTimeSchedulerPartialTarget<TAbsolute, TRelative>.Mock' already defines a member called 'ScheduleAbsolute' with the same parameter types
Code:
ScheduleAbsolute

TODO:
* Add integration test in for this case.
* May need to restructure tests in `IMethodSymbolExtensionsMatchTests` so it's getting methods from the same type. Could define `[MarkerOne]` and `[MarkerTwo]` attributes in the test code to specifically point out which two methods to compare.
* Add a bug to Rocks about `"throw new global::Rocks.Exceptions.ExpectationException"` gen-d code in properties being off in terms of indent.