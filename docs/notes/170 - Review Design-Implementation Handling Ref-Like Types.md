* DONE - Create `RefStructArgument<>` with tests
* DONE - Update projection creation to ignore `ref struct` types
* DONE - Change handler creation to use `RefStructArgument<>`, `Action`, and `Func<>` types
* DONE - Update expectation creation to use `RefStructArgument<>`
* DONE - Run tests and fix any changes
* DONE - Update adornments to use `Action`, and `Func<>` types
* If a parameter type is a `ref struct` and it's `scoped`, we need to fall back to generating a custom delegate type for that to work.
* Add a test in `ParamsTests` in `Rocks.IntegrationTests` for a `params ReadOnlySpan<string>` - this change may make things easier to mock.
* Address TODOs
    * Look for `.IsRefLikeType` in Rocks, just to make sure it's used correctly
    * `ITypeSymbolExtensions.IsEsoteric()` - can be removed?
    * `RefLikeTypeEqualityComparer` - can be removed?
    * `RefLikeArgTypeBuilder` + `RefLikeArgTypeBuilderTests` - can be removed?
    * Add tests for `IParameterSymbolExtensions.IsScoped()`