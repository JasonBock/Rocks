* DONE - Need to change the diagnostics so that they're descriptors, and then report all of those from our analyzer.
* Finalize work on analyzer code
* Change CodeGen such that it runs the analzyer as well, as we need the diagnostics reported on the types. Or...we only gen when the type is valid, which is actually nicer (probably) because then we don't need to filter out types. We'd only gen for the types that were valid targets. 

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