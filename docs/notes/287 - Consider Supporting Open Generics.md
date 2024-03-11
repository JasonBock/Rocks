Open generic support.

* DONE - Create a version of the attributes that takes `typeof(...)`.
* Handle non-generic attributes in generator
* Add generic parameters to:
    * Expectations class (and any reference to the class)
    * Mock type

* Ensure that all members, particularly properties and indexes, now support the use of the type-level generic parameters.
* Create a file name that is unique. Probably something like a number representing the number of generic parameters, or see what `FullyQualifiedName` produces.
* Add unit and integration tests.
* Add to code gen tests the ability to make a closed and open version of generic types.