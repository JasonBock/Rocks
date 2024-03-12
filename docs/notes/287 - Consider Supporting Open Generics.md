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
* Projected types need to generate the correct expectations type name
* Add unit and integration tests for both creates and makes
* Add to code gen tests the ability to make a closed and open version of generic types.