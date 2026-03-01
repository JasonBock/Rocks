* Testing - In general, other than the builders which are essentially tested by the "Generators" unit tests, code should have unit tests in isolation to ensure specific parts can be tested separately.

* "Remove Unnecessary Usings"
* Why does `Verify()` take so long? https://github.com/ecoAPM/BenchmarkMockNet/blob/main/Results.md#verify. I mean, it's not that slow :), but maybe there's a way to improve it slightly. Maybe change `protected List<string> Verify<THandler>(Handlers<THandler> handlers, uint memberIdentifier)` to `protected IEnumerable<string> Verify<THandler>(Handlers<THandler> handlers, uint memberIdentifier)` as we really don't need to allocate a list here because `AddRange()` on `List<>` takes an `IEnumerable<>`.
* `Builders\Create`
    * `NamingContext`
        * `this[]->get` - at least comment this.
    * `TypeArgumentsNamingContext` - maybe I should make one for each `ITypeReferenceModel` and `MethodModel` on construction? That way, I don't have to keep making them throughout the code. These should not vary once iterated for a type or method.
    * There are three versions of `static string GetOptionalParameter(ParameterModel parameter, ParameterModel lastParameter, string typeName, string requiresNullable)` - should be moved into one method, probably on `ParameterModel` itself, then it would only need the 2nd parameter.
    * `ExpectationExceptionBuilder`
        * `Build()`
            * Instead of calling the `Indent` incrementors and decrementors twice, why not `writer.Indent += 2`? Probably will make absolutely no perf difference, but it's a bit less code.
    * `MethodExpectationsMethodBuilder`
        * `Build()`
            * `foreach` at line 159, pull that out to another method
            * Line 139 and 149, combine to `WriteLines()`
    * `IndexerExpectationsIndexerBuilder`
        * `BuildGetter()`
            * Line 105 and 240, condense to `WriteLines()`
    * `MethodExpectationsMethodBuilder`
        * `Build()`
            * Line 48, maybe just use `parameter.Type.FullyQualifiedName`? Or if `BuildName(...)` is the right call, change comment. Though this may mean with the `GetOptionalParameter()` refactor named above, do we always use `BuildName()` there?
    * `MockAdornmentsBuilder`
        * `Build()`
            * Combine `WriteLine()` and `WriteLines()` at the beginning into one `WriteLines()`
    * `MockConstructorExtensionsBuilder`
        * `Build()`
            * Line 71, combine into `WriteLines()`
    * `MockMembersExpectationsBuilder`
        * `Build()`
            * Change `.ToList()` to `.ToArray()` - in fact, might as well search for this everywhere because I'm guessing an array allocation will be slightly cheaper than a list. Would be kind of an odd, fun performance test to do.
    * `MockMethodVoidBuilder` and `MockMethodValueBuilder`
        * `Build()`
            * Both have code to get `methodParameters` that is almost identical except for getting `AttributesDescription`. See if this can be shared. In fact, the methods and the method types in general are almost identical
    * `MockProjectedDelegateBuilder`
        * `GetProjectedCallbackDelegateFullyQualifiedName()`
            * `ITypeReferenceModel typeToMock` is unused
    * `NamingContext`
        * Add some comments around what this does.
    * `RockCreateBuilder`
        * `Build()`
            * Some `WriteLine()` into one `WriteLines()`
    * `VariablesNamingContext.cs`
        * Split two classes into separate files.

    * `ShimMethodBuilder`
        * `Build()`
            * Why does `typeArgumentsNamingContext` use `shimType` instead of the method in question? If there's a reason, document it.
* `Builders\Make`
    * `MockMethodVoidBuilder` and `MockMethodValueBuilder`
        * The methods and the method types in general are almost identical
* `Descriptors`
    * May be helpful to have a "shared" `internal` type here to have all the IDs listed to make it somewhat easier when new ones need to be added.
* `Extensions`
    * `ITypeParameterSymbolExtensions`
        * `GetConstraints()` - Maybe now figure out how to determine if the `default` constraint exists?
* "Remove Unnecessary Usings"
* Format every file
* Ensure all the code in the Rocks library has the "disclaimer" XML comment at the top to discourage users from using them for anything outside of code-generated Rocks usage.