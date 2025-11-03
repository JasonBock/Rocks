What are the specific steps that I need to do?

* Everything goes on the generated expectations type now, not as extensions. So, for example, `PropertyExpectations`, `MethodExpectations` - all of that will change to targeting the expectations type.

* What about explicit implementations? This is problematic, because we don't want the expectations type to inherit directly from the implementing type. But, we have to put explicit implementations somewhere else. We may need to generate a `.ExplicitForXYZ` property on the expectations type so those expectations that will be implementing explicitly on the mock type work (and also does a naming context to not collide with members that exist on the mocked type).

* For methods:
    * Currently, Rocks does this: `expectations.Methods.CallThis(...)`. This needs to change to `expectations.CallThis(...)`. The adornments that are currently used will work here as the return value.
* For properties:
    * This is a little trickier. Currently, Rocks does this: `expectations.Properties.Getters.GetData()`. This needs to change to this: `expectations.GetData`. The adornments that are returned need to have way to specify which accessors are available to mock:
        * Getter: `expectations.GetData.Gets()`
        * Setter: `expectations.GetData.Sets(...)`
        * Initter: `expectations.GetData.Inits(...)`
    After that's done, the adornments can have the normal API
* For indexers:
    * This would be the same as properties. Currently, Rocks does this: `expectations.Indexers.Getters.This(...)`. This needs to change to this: `expectations[...]`. It would also have the same accessors for adornments:
        * Getter: `expectations[...].Get()`
        * Setter: `expectations[...].Sets(...)`
        * Initter: `expectations[...].Inits(...)`
* I don't think any new work has to be done for events, but it may be worth doing something different, like maybe putting an event on the expectations and using the `add` and `remove` thingees to add an event handler. Not sure about this.

* The generated `Instance()` methods will need a naming context to ensure they don't collide with the (rare) case of an existing member named `Instance`.

Maybe spend some time manually changing the generated code to the "shape" that I want, and see how that works.

* I should put `MemberIdentifierAttribute` on the specific `get/set/init`. That way, I don't have to pass in the `PropertyAccessor`. Like this:

```c#
public class PropId
{
	public Guid Id 
	{
		[global::Rocks.MemberIdentifier(2)]
		get;
		[global::Rocks.MemberIdentifier(3)]
		set;  
	}
}

Console.WriteLine(typeof(PropId).GetMemberDescription(2));
```

This prints: `System.Guid get_Id()`. This is a small thing, but I think it's clearer.

The `Handler.cs` and `Adornments.cs` file should be broken up. Maybe even create separate namespaces and that will need to be reflected in generated code.

Create an intermediary set of "property adornments" classes. Instances are returned:

* `GetPropertyAdornments`
    * `Gets()` => returns `Adornments<TAdornments, THandler, TCallback, TReturnValue>`
* `GetAndSetPropertyAdornments`
    * `Gets()` => returns `Adornments<TAdornments, THandler, TCallback, TReturnValue>`
    * `Sets(value)` => returns `Adornments<TAdornments, THandler, TCallback>`
* `GetAndInitPropertyAdornments`
    * `Gets()` => returns `Adornments<TAdornments, THandler, TCallback, TReturnValue>`
    * `Inits(value)` => returns `Adornments<TAdornments, THandler, TCallback>`
* `SetPropertyAdornments`
    * `Sets(value)` => returns `Adornments<TAdornments, THandler, TCallback>`
* `InitPropertyAdornments`
    * `Inits(value)` => returns `Adornments<TAdornments, THandler, TCallback>`

...maybe?

    // Rocks.Analysis\Rocks.Analysis.RockGenerator\IIndexers_Rock_Create.g.cs(105,39): error CS1014: A get or set accessor expected
    DiagnosticResult.CompilerError("CS1014").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\IIndexers_Rock_Create.g.cs", 105, 39, 105, 40),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\IIndexers_Rock_Create.g.cs(136,39): error CS1014: A get or set accessor expected
    DiagnosticResult.CompilerError("CS1014").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\IIndexers_Rock_Create.g.cs", 136, 39, 136, 40),


    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs(34,65): error CS0102: The type 'ILeftRightCreateExpectations' already contains a definition for 'ExplicitForILeft'
    DiagnosticResult.CompilerError("CS0102").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs", 34, 65, 34, 81).WithArguments("ILeftRightCreateExpectations", "ExplicitForILeft"),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs(54,66): error CS0102: The type 'ILeftRightCreateExpectations' already contains a definition for 'ExplicitForIRight'
    DiagnosticResult.CompilerError("CS0102").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs", 54, 66, 54, 83).WithArguments("ILeftRightCreateExpectations", "ExplicitForIRight"),


    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs(54,49): error CS0426: The type name 'ValuePropertyExpectations' does not exist in the type 'ILeftRightCreateExpectations'
    DiagnosticResult.CompilerError("CS0426").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs", 54, 49, 54, 74).WithArguments("ValuePropertyExpectations", "ILeftRightCreateExpectations"),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs(98,49): error CS0426: The type name 'ValuePropertyExpectations' does not exist in the type 'ILeftRightCreateExpectations'
    DiagnosticResult.CompilerError("CS0426").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs", 98, 49, 98, 74).WithArguments("ValuePropertyExpectations", "ILeftRightCreateExpectations"),


`GenerateWhenIndexerHasOptionalArgumentsAsync`
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs(45,94): error CS0111: Type 'IHaveOptionalArgumentsCreateExpectations.Indexer0Expectations' already defines a member called 'Gets' with the same parameter types
    DiagnosticResult.CompilerError("CS0111").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs", 45, 94, 45, 98).WithArguments("Gets", "IHaveOptionalArgumentsCreateExpectations.Indexer0Expectations"),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs(46,9): error CS1501: No overload for method 'Gets' takes 2 arguments
    DiagnosticResult.CompilerError("CS1501").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs", 46, 9, 46, 13).WithArguments("Gets", "2"),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs(64,94): error CS0111: Type 'IHaveOptionalArgumentsCreateExpectations.Indexer0Expectations' already defines a member called 'Sets' with the same parameter types
    DiagnosticResult.CompilerError("CS0111").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs", 64, 94, 64, 98).WithArguments("Sets", "IHaveOptionalArgumentsCreateExpectations.Indexer0Expectations"),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs(65,9): error CS1501: No overload for method 'Sets' takes 3 arguments
    DiagnosticResult.CompilerError("CS1501").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\IHaveOptionalArguments_Rock_Create.g.cs", 65, 9, 65, 13).WithArguments("Sets", "3"),


Error:

ID: CS0102
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\Autofac.Core.IActivatedEventArgsT_Rock_Create.g.cs(276,55): error CS0102: The type 'IActivatedEventArgsCreateExpectations<T>' already contains a definition for 'Instance'
Code:
Instance

```c#
var expectations = context.Create<IThingCreateExpectations>();
var setups = expectations.Setups;
setups.DoThis().ExpectedCallCount(2);
setups.Name.Sets("name");

var mock = expectations.Instance();
mock.DoThis();
mock.Name = "name";
```

    // Rocks.Analysis\Rocks.Analysis.RockGenerator\MockTests.IMember_Rock_Create.g.cs(19,7): error CS0027: Keyword 'this' is not available in the current context
    DiagnosticResult.CompilerError("CS0027").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\MockTests.IMember_Rock_Create.g.cs", 19, 7, 19, 11),


    // Rocks.Analysis\Rocks.Analysis.RockGenerator\IWorker_Rock_Create.g.cs(56,46): error CS0426: The type name 'WorksPropertyExpectations' does not exist in the type 'IWorkerCreateExpectations'
    DiagnosticResult.CompilerError("CS0426").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\IWorker_Rock_Create.g.cs", 56, 46, 56, 71).WithArguments("WorksPropertyExpectations", "IWorkerCreateExpectations"),



    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs(43,127): error CS1503: Argument 1: cannot convert from 'ILeftRightCreateExpectations.SetupsExpectations' to 'ILeftRightCreateExpectations'
    DiagnosticResult.CompilerError("CS1503").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs", 43, 127, 43, 131).WithArguments("1", "ILeftRightCreateExpectations.SetupsExpectations", "ILeftRightCreateExpectations"),
    // Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs(63,129): error CS1503: Argument 1: cannot convert from 'ILeftRightCreateExpectations.SetupsExpectations' to 'ILeftRightCreateExpectations'
    DiagnosticResult.CompilerError("CS1503").WithSpan("Rocks.Analysis\Rocks.Analysis.RockGenerator\ILeftRight_Rock_Create.g.cs", 63, 129, 63, 133).WithArguments("1", "ILeftRightCreateExpectations.SetupsExpectations", "ILeftRightCreateExpectations"),


I'm guessing there's an issue with explicit implementation and name conflicts. But I'm curious why this doesn't happen with the current implementation. Let's run this and see the specifics with the new changes.

Target type: `MassTransit.Request<,,,,>`

Seems like the problem is that there are multiple explicit implementation needed for types with the name `Request`, though their type parameters are different. Basically a naming conflict. I think this should be "easy" enough to circumvent, though it'll be a big ugly, but it should also be a pretty rare occurrence.

* In `MockMembersExpectationsBuilder`, we get a hash set of explicit types and `foreach` over that - we may need to change to `for` because we're currently in an enumeration and we need to be able to do the following things:
    * For the current explicit type, see how many explicit types exist in the list by the type name.
        * If it's one, just use the type name
        * If it's more than one, see how many explicit types exist in the list with the type flattened name
            * If it's one, use the type flattened name
            * If it's more than one, use the fully qualified name with some modifications (probably):
                * Replace `"::"` with `string.Empty`
                * Replace `'.'` with `string.Empty`
                * How do generics show up here?

For the vast majority of explicit implementation cases, the first case will hold. It only starts getting ugly if we need disambiguation. But this should handle all disambiguation cases.

* DONE (`ExplicitImplementationGeneratorTests`, `GenerateWithDuplicateInterfaceNamesDifferingByTypeParametersAsync()`, `GenerateWithDuplicateInterfaceNamesAsync()`) Create tests where
    * Two explicit types differ by type parameters
    * Two explicit types differ by namespace
* Solve the issue
* Ensure two new tests pass along with the other current tests

TODO:
* DONE - Update the `Arguments.cs`, `Adornments.cs`, and `Handler.cs` split up the types to one file each.
* DONE - Change `MemberIdentifier` to only take one parameter
* DONE - The mocking code can be put in a file-scoped namespaced - e.g. instead of `namespace Stuff { ... }`, it would be `namespace Stuff;`
* DONE - Create updated "Minimized" builders. This should make it easier to build things and make it different from all the others.
    * DONE - `MockMembersExpectationsBuilder` - This can take a `buildsForExplicit` or something like that to either emit `this.` or `this.parent` with a readonly `Expectations` reference in the method builder. This would also create the containing "explicit" class
        * DONE - `MethodExpectationsBuilder`
        * DONE - `PropertyExpectationsBuilder` - remember to put `[MemberIdentifier]` on the `get/set/init`, not on the property, with no `PropertyAccessor`
        * DONE - `IndexerExpectationsBuilder` - remember to put `[MemberIdentifier]` on the `get/set/init`, not on the property, with no `PropertyAccessor`
* DONE - The `Action<AdornmentsPipeline> adornmentsFQNsPipeline` thing in the expectations may go away with all of this and will not be needed.
* DONE - Ensure original expectation implementations are no longer there
* DONE - The `parent` field in the generated expectation classes needs to have a naming context because parameters can have the name "parent".
* DONE - Ensure explicits are handled correctly. Properties will need `expectationsSource` for the constructor. Note: Rocks currently does not handle duplicate interface names, so while I should come up with a solution for that case, it's not pressing ATM. At least put a `TODO` in the code.
* DONE - Need to ensure "defaults" in methods and indexers are handled correctly.
    * DONE - OK, "defaults" is just doing an extra implementation for the methods and indexers where it puts the correct type instead of an `Argument<>` with the default value set.
    * DONE - Tests to review:
        * DONE - `MethodGeneratorTests`
            * DONE - `GenerateWhenOptionalArgumentsAndParamsExistAsync`
            * DONE - `GenerateWhenOptionalArgumentsExistAsync`
            * DONE - `GenerateWithOptionalParametersAndParamsAsync`
        * DONE - `IndexerGeneratorTests`
            * DONE - `GenerateWhenIndexerHasOptionalArgumentsAsync`
        * DONE - `DefaultValuesGeneratorTests`
            * DONE - `GenerateWhenExplicitImplementationHasDefaultValuesAsync`
            * DONE - `GenerateWhenGenericParameterHasOptionalDefaultValueAsync`
            * DONE - `GenerateWithPositiveInfinityAsync`
* DONE - Naming collisions
    * DONE - A type could have a member the same name that is generated for an explicit implementation
* DONE - Create a "SetupsExpectations" class that contains all of the expectations stuff that's generated, and then there will be one `Setups` property. This will eliminate **all** naming conflicts that could happen.
    * DONE - `MethodGeneratorTests`
        * DONE - `GenerateAsync`
        * DONE - `GenerateWhenOptionalArgumentsAndParamsExistAsync`
        * DONE - `GenerateWhenOptionalArgumentsExistAsync`
        * DONE - `GenerateWithOptionalParametersAndParamsAsync`
    * DONE - `PropertyGeneratorTests`
        * DONE - `GenerateAsync`
    * DONE - `IndexerGeneratorTests`
        * DONE - `GenerateWhenIndexerHasOptionalArgumentsAsync`
    * DONE - `ExplicitImplementationGeneratorTests`
        * DONE - `GenerateWithExplicitMethodAsync`
        * DONE - `GenerateWithExplicitPropertySetterAsync`
        * DONE - `GenerateWithDuplicateInterfaceNamesDifferingByTypeParametersAsync`
        * DONE - `GenerateWithDuplicateInterfaceNamesAsync`
    * DONE - `DefaultValuesGeneratorTests`
        * DONE - `GenerateWhenExplicitImplementationHasDefaultValuesAsync`
        * DONE - `GenerateWhenGenericParameterHasOptionalDefaultValueAsync`
        * DONE - `GenerateWithPositiveInfinityAsync`
* DONE - Use expression bodies for the getters. For example, instead of this:

```c#
internal global::Autofac.Core.IActivatedEventArgsPartialTarget<T>.ServicePropertyExpectations Service { get => new(this); }
```

We generate this:

```c#
internal global::Autofac.Core.IActivatedEventArgsPartialTarget<T>.ServicePropertyExpectations Service => new(this);
```
* Add some space
    * Create
        * DONE - Between `HandlerX` and field definition groupings
        * DONE - All the new expectations changes I made (gotta make things pretty)
        * DONE - Between the two gen'd methods for optional arguments
        * DONE - Between generated adornments classes
        * DONE - Shims
        * Mock type generation
    * Make
        * DONE - Between all gen'd members
* Add XML comments for all generated code to help/assist the user. See explicit implementation for an example.
* Testing strategy
    * Run code gen tests
    * Update and run integration tests
        * Last file updated: ClassGenericIndexerTests.cs
    * Update and run unit tests
    * Add unit and integeration tests for
        * Ref returns
* Clean up builders
    * I think I can get rid of `PropertyExpectationTypeName`
* Update all documentation such that it reflects the new syntax.
    * Consider writing an "upgrade" doc that provides assistance on how test code should be changes from the old to the new style.
* Release a new alpha, 10.0.0-alpha.2


To update code to the new format, follow these steps:

* Methods
    * Replace `.Methods` with `.Setups`
* Properties
    * Replace `.Properties().Getters().Target()` with `.Setups.Target.Gets()`
    * Replace `.Properties().Setters().Target(...)` with `.Setups.Target.Sets(...)`
* Indexers
    * Replace `.Indexers().Getters().This(...)` with `.Setups[...].Gets()`
    * Replace `.Indexers().Setters().This(value, ...)` with `.Setups[...].Sets(value)`