* DONE - Create `RefStructArgument<>` with tests
* DONE - Update projection creation to ignore `ref struct` types
* DONE - Change handler creation to use `RefStructArgument<>`, `Action`, and `Func<>` types
* Update expectation creation to use `RefStructArgument<>`
* Run tests and fix any changes
* Update adornments to use `Action`, and `Func<>` types
* Add a test in `ParamsTests` in `Rocks.IntegrationTests` for a `params ReadOnlySpan<string>` - this change may make things easier to mock.
* Address TODOs
    * `ITypeSymbolExtensions.IsEsoteric()` - can be removed?
    * `RefLikeTypeEqualityComparer` - can be removed?
    * `RefLikeArgTypeBuilder` - can be removed?