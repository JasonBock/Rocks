* DONE - Need to change the diagnostics so that they're descriptors, and then report all of those from our analyzer.
* Finalize work on analyzer code
* Change CodeGen such that it runs the analzyer as well, as we need the diagnostics reported on the types. Or...we only gen when the type is valid, which is actually nicer (probably) because then we don't need to filter out types. We'd only gen for the types that were valid targets. 
* Remember to restart and do a spot-check in IntegrationTests with doing a mock on a sealed type, the analyzer should find that. Maybe put a `pragma` around the error so that we have an integration test in place, but not necessary.

Tests to update/rewrite
* Add tests for the descriptors
* Use MockModelTests (older) to see what the tests should be for the analyzer.
* Look at the (older) versions of these tests to rewriter
    * RockAttributeGeneratorTests
        * CreateWhenAnExactMatchWithANonVirtualMemberExistsAsync
        * CreateWhenTargetTypeIsInvalidAsync
        * MakeWhenTargetTypeIsInvalidAsync
    * ConstructorGeneratorTests
        * GenerateWhenNoAccessibleConstructorsExistAsync
        * GenerateWhenOnlyConstructorsIsObsoleteAsync
        * GenerateWhenTypeArgumentsCreateDuplicateConstructorsAsync
    * NonPublicMembersGeneratorTests
        * CreateWithInaccesibleTypesUsedOnConstraintsAsync
        * CreateWithInternalAbstractMemberInDifferentAssemblyAsync
        * CreateWithInternalAbstractMemberInDifferentAssemblyWithUnreferenceableNameAsync
    * ObsoleteGeneratorTests
        * CreateWhenGenericContainsObsoleteTypeAsync
        * GenerateWhenAMethodAndItsPartsAreObsoleteAsync
        * GenerateWhenAPropertyAndItsTypeAreObsoleteAsync
        * GenerateWhenATargetTypeIsObsoleteAsync

`new[] { diagnostic }` can be changed to `[diagnostic]`

There are a number of issues showing up now that I fixed code gen. Yay.