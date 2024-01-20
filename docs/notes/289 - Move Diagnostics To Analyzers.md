* DONE - Need to change the diagnostics so that they're descriptors, and then report all of those from our analyzer.
* Finalize work on analyzer code
* Change CodeGen such that it runs the analzyer as well, as we need the diagnostics reported on the types. Or...we only gen when the type is valid, which is actually nicer (probably) because then we don't need to filter out types. We'd only gen for the types that were valid targets. 
* Remember to restart and do a spot-check in IntegrationTests with doing a mock on a sealed type, the analyzer should find that. Maybe put a `pragma` around the error so that we have an integration test in place, but not necessary.

Tests to update/rewrite
* DONE - Add tests for the descriptors
* DONE - Use MockModelTests (older) to see what the tests should be for the analyzer.
* DONE - Look at the (older) versions of these tests to rewriter
    * DONE - RockAttributeGeneratorTests
        * DONE - CreateWhenAnExactMatchWithANonVirtualMemberExistsAsync
        * DONE - CreateWhenTargetTypeIsInvalidAsync
        * DONE - MakeWhenTargetTypeIsInvalidAsync
    * DONE - ConstructorGeneratorTests
        * DONE - GenerateWhenNoAccessibleConstructorsExistAsync
        * DONE - GenerateWhenOnlyConstructorsIsObsoleteAsync
        * DONE - GenerateWhenTypeArgumentsCreateDuplicateConstructorsAsync
    * DONE - NonPublicMembersGeneratorTests
        * DONE - CreateWithInaccesibleTypesUsedOnConstraintsAsync
        * DONE - CreateWithInternalAbstractMemberInDifferentAssemblyAsync
        * DONE - CreateWithInternalAbstractMemberInDifferentAssemblyWithUnreferenceableNameAsync
    * DONE - ObsoleteGeneratorTests
        * DONE - CreateWhenGenericContainsObsoleteTypeAsync
        * DONE - GenerateWhenAMethodAndItsPartsAreObsoleteAsync
        * DONE - GenerateWhenAPropertyAndItsTypeAreObsoleteAsync
        * DONE - GenerateWhenATargetTypeIsObsoleteAsync

DONE - `new[] { diagnostic }` can be changed to `[diagnostic]`

There is a point to be made that if there's a compiler error, like `RockCreate<ThisIsAnEnum>`, which, given the constraint on `T`, you can't provide an enum. But...I may end up doing `RockCreate(typeof(SomeType))` to support open generics, so in that case, there's no way to put a constraint the parameter.

There are a number of issues showing up now that I fixed code gen. Yay.

* REPORTED - For the event extensions, if the handler type has any generics, the generated event extension method needs to declare them, along with (probably) any constraints that exist.
    * `Blazored.LocalStorage.ILocalStorageService`
    * `Castle.Components.DictionaryAdapter.Xml.IXmlIterator`
    * `Castle.Components.DictionaryAdapter.IDictionaryAdapter`
    * and many others
* REPORTED - In the member expectations handlers (for methods and indexers), a parameter can be named `handler`, which will collide with our `handler` variable. Need to use `VariableNamingContext` here.
    * `AngleSharp.Dom.IWindowTimers`
* REPORTED - Odd, in some cases, generating the expectations creates code in a constructor like this: `() = ();`. Not sure what's going on there. Actually, I think when I'm looking to see if I need to create a member expectation class, I need to also check if they require explicit implementations or not. Just for properties/indexers, methods are doing it right.
    * `Microsoft.EntityFrameworkCore.Metadata.IMutableTrigger`
* FIXED - Looks like some code is getting the `CS1702` warning, can probably just suppress that.
* REPORTED - `Refit.IApiResponse` is giving an error during a make build type. Looks like a property is marked with `[MemberNotNullWhen(false, nameof(Error))]`, which...I can't get my code gen tool to actually gen the mock code :(. So I'm not sure what's wrong. Well, it's because the property is returning `default!`, which is `false`, which causes the `CS8775` warning. Frankly, the easiest way to fix this is to put a `#pragma warning disable CS8775` around the member "return". There is also a `MemberNotNull` attribute, so I may want to "fix" that one as well. Whee. I think if I had `PragmaDisable(string[])` and `PragmaRestore(string[])` around the entire make creation with a known list of code, that'll do the trick.
