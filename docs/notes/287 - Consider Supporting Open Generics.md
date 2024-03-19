Open generic support.

* DONE - Create a version of the attributes that takes `typeof(...)`.
* Handle non-generic attributes in generator.
    * DONE - `HasErrors()` - if the type parameter's `TypeKind` is `TypeParameter`, then don't check it for an error
    * DONE - `GetMembers()` - I think I need to do that with `OriginalDefinition`
    * I don't think `IsOpenGeneric()` and `HasOpenGenerics()` are 100% correct.
    * DONE - Need to remove the diagnostic that prevents open generics
* DONE - Add generic parameters (and constraints) to:
    * DONE - Expectations class (and any reference to the class)
    * DONE - Ensure that the name of the expectations class is what's referenced everywhere else
    * DONE - Constraints
* DONE - Ensure that all members, particularly properties and indexes, now support the use of the type-level generic parameters.
* DONE - Create a file name that is unique. Probably something like a number representing the number of generic parameters, or see what `FullyQualifiedName` produces.
* DONE - Projected types need to generate the correct expectations type name
* Projected callback, handlers, members should not add types (and constraints) if the generic parameter was defined on the type.
* Add unit and integration tests for both creates and makes
* Add to code gen tests the ability to make a closed and open version of generic types.

Bugs:
* If a method has its own type parameters, it can collide with a type parameter from the containing type. To get around this, I'll need to use a `VariableNamingContext` when I generate generic methods if the containing type is generic as well. Note that I ONLY need to change method type names relative to the type parameter names if the method is generic. Probably need to do this with (for methods only):
    * DONE - Constraints (right now, this gives back a `string`, make this a `Constraints` class, with a `TypeName` for the associated type parameter, and `Values` as an `EquatableArray<string>` that has the constraint values - e.g. `notnull`, `class`, etc.)
        * DONE - Mock implementations (create and make)
        * DONE - Expectations (create)
        * DONE - Handlers (create)
    * DONE - Expectations handling defaults (think where it calls `this.` to switch the parameters around)
    * DONE - Shims 
        * DONE - Return values - think `base.CallBaseMethod<T>(...);`
        * DONE - Builders
    * DONE - Handlers (create)
    * DONE - Mock implementations (create and make)
    * DONE - Expectation setups (create)
* You only do a replacement for a type parameter name IF the type parameter comes from the method. What happening right now is that a method is using a type parameter from the type, but because I'm blindly trying to replace given the list from the type's type parameters, I'll add a "1" at the end, even though it doesn't need to be replaced. I think the way to fix this is to look at the type name I'm about to pass into the VNC for the type's type parameters, and if that name is in the list for the method's type parameters, then we see if it needs to be changed with the VNC; otherwise, we just keep it as-is.
    * DONE - Constraints
        * DONE - Mock implementations (create and make)
        * DONE - Expectations (create)
        * DONE - Handlers (create)
    * DONE - Expectations handling defaults (think where it calls `this.` to switch the parameters around)
    * DONE - Shims 
        * DONE - Return values - think `base.CallBaseMethod<T>(...);`
        * DONE - Builders
    * DONE - Handlers (create)
    * DONE - Mock implementations (create and make)
    * DONE - Expectation setups (create)

var typeName = method.IsGenericMethod && method.TypeArguments.Contains(_.Type.FullyQualifiedName) ?
    typeArgumentsNamingContext[_.Type.FullyQualifiedName] : _.Type.FullyQualifiedName;

* Projected callbacks (create)
* Maybe other projections (create)

* If the generic type is closed, I need to generate for a `ref struct` this: `global::System.Span<int> @value` - see `RefStructGeneratorTests.CreateWhenParameterIsScopedAsync()`. Now I'm gen'ing `global::System.Span @value` because there are closed type arguments.
    * Look for `namedType.TypeArguments.Length > 0`, should be `namedType.IsOpenGeneric`
    * Argument evaluation delegate
    * `IsValid()`


Generic Types Big Change
* DONE - Make `TypeArguments` and `TypeParameters` on both `TypeReferenceModel` and `MethodModel` collections of `TypeReferenceModel` objects. 
* DONE - Create a method called ... `BuildName()` (?) that maybe does what `GetName()` does, but also has an overload that takes another `TypeReferenceModel parentType` value. That would be used to ensure the type returns a FQN that has renames correct, even if it recursive.
* DONE - Look for `typeArgumentsNamingContext[` as that should no longer exist.
* DONE - Look at all `TypeArguments` and `TypeParameters` references and determine how they should change
    * DONE - `TypeReferenceModel.TypeArguments`
    * DONE - `TypeReferenceModel.TypeParameters`
    * DONE - `MethodModel.TypeArguments`
    * DONE - `MethodModel.TypeParameters`
* DONE (sans the CsvHelper issue below) - Ensure all Rocks.Tests pass.

Things are just not getting better. I feel like fix or change something, and then something else breaks somewhere.

* DONE - Make a `NamingContext` abstract class that `VariableNamingContext` and a new class, `TypeArgumentNamingContext`, derive from. The first is all about variables and naming, the second is about type arguments.
* DONE - Revisit implementation for type argument renaming
    * Create
        * DONE - Handlers
            * DONE - All types for the base `Handler<...>` type must have renames
            * DONE - All fields for the `Handler` must have renames
        * DONE - Method implementations
            * DONE - Signature: the return and parameter types must have renames (note the method name type arguments are fine)
        * DONE - Method expectations
            * DONE - All method parameter types must have renames (including the default switcheroo)
        * DONE - Adornments
            * DONE - All types for the base `Adornments<...>` type must have renames, seems to need the fixes starting with the 3rd parameter
    * DONE - Make
        * DONE - Method implementations
            * DONE - Signature: the return and parameter types must have renames (note the method name type arguments are fine)
* DONE - `FullyQualifiedNameNoGenerics` add the `?` right after if it's nullable. We need to remove that, and anyone using it needs to check for nullability after the fact.
* DONE - Multiple nullables are causing issues
* DONE - Duplicates of generated types, especially the "Argument" delegate + custom type, need to be handled correctly, even if the type parameter names are different. Maybe I make a custom equality comparer that gets the name without generics, and then does `<,..,>` for all the type parameters, and uses that to compare (I'm guessing I don't need to care about nullable types here)
* DONE - In `GetNameForGeneric()`, if `FullyQualifiedName` doesn't have `<` in it, then return it as-is. It's really not "generic" and ... I think that'll work.
* DONE - For creates and makes...getting `CS8714` warning, which I really don't think I can address, even the "Generate Overrides" tool in VS doesn't realize it should add a `notnull` constraint for `TKey`. So I'll just disable/restore the warning (oh god, this is going to break a lot of unit tests because of diff). The real issue is that type I'm targeting doesn't have the constraint on it like it should. For example, if I write `public class MyDictionary<TKey, TValue> : Dictionary<TKey, TValue> { }`, the warning will show up right there because `TKey` on `Dictionary<,>` has the `notnull` constraint. So `MyDictionary<,>` **should** have the constraint as well, but it doesn't, because it's a warning, not an error. This should be done for `CS8714`, `CS8633`, and `CS8618`, so I do this in one take and not have to update all the tests three times. NOTE: Need to remove the `#pragma warning disable\restore CS8618` pair done in `Handlers` because it'll be at the top now.
* DONE - When a type is targeted for mocking, and it's generic, and an open generic is created, and it has events...I can't create the `...EventExtensions` class, because the generic type parameters are unknown. I can't pull the static class into the expectations class because it must be a top-level class. The "extension everything" idea that's been discussed for centuries in C# **may** help in the future, but for now...I'll change Rocks so it doesn't make the helpers in this case, and I'll also need to add docs on this.
* DONE - I noticed that when I mocked a CSLA type - `IFieldData` - I got the CSLA analyzer saying it needed to be serializable. Shouldn't this have shown up on the `Mock` type? Not because of the custom CSLA analyzer, but because the attribute...oh wait, `IFieldData` doesn't have `[Serializable]` on it, so...oh well? Should still double-check serialization rules for Rocks to see if that all still works or not.
* `Explicit...For...` classes need to have type arguments specified, otherwise duplicate definitions occur (see `MassTransit.Request<,,,,>` "Create" for an example). Probably need to use the flattened name for these explicit expectation classes.
    * `MethodExpectationsBuilder`
    * `PropertyExpectationsBuilder`
    * `IndexerExpectationsBuilder`
* DONE - Renames (see `MassTransit.ExceptionConsumeContext<>` "Create")
    * Projected callback and return value delegates should have type parameters if needed, with resolved names
    * Method parameter types still seem to have some renaming issues
    * Expectation parameter types still seem to have some renaming issues
    * Adornment creation in expectation handlers has issue (`global::Rocks.Arg.Any<T1?>()`)
* Some type parameter renames are getting truncated by one character
    
* When a mock returns one of the "Task" types, type parameter renames need to occur (see `Proto.Deduplication.DeduplicationContext<>`)
* Add tests for `BuildName()` in `TypeReferenceModel`, esp. with tuple types and nested open generic values (e.g. `Dictionary<string, List<T>>` or something like that)
* Ensure all Rocks.Tests pass again.
* Ensure all Rocks.CodeGenerationTests pass.
* Ensure all Rocks.IntegrationTests pass again.
* Update docs
    * Open generics are now supported, and how it's done
    * Event extension methods won't be created if the target type is generic, an open generic is requested, and that target type has events.
* Override `ToString()` on `TypeReferenceModel` to return `FullyQualifiedName` 
* Should consider doing a test for equality on a `TypeReferenceModel`, and then look for where `.FullyQualifiedName` is used for equality

TODO FUTURE

* I need to do a massive rethink on naming. The whole `GetName()` extension method, and how I get names, is so convoluted and inconsistent, and it's made adding some features much harder than it should be. I need to figure out what names I need, in what format, and at what times in the application, so I make things consistent.
* Along with naming, maybe I can generate **all** of the names as they should be for generation. The model is purely there for the code, so...if I know a type parameter `T` needs to change to `T1` because there's a `T` on the type, just do that right away.
