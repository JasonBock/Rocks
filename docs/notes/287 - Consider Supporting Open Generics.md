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

With `CsvHelper.Configuration.IHasTypeConverterOptions<TClass, TMember>`, Rocks doesn't implement `Map<>`. It needs to type argument rename with:
    * Handler
        * The callback and return types for a handler definition
    * Method implementation
        * The return value on the definition
        * Parameter types
    * Expectations
        * Parameter types (also for the default switcheroo)