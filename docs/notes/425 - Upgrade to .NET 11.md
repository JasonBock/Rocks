We can change the gen'd code from having a `readonly` field to just a `{ get; }` property. See Rocks.Performance, `CurrentExpectations` to see how `SetupExpectations` changed to not having `this.setups` anymore.

TODO:

* Add a test to mock a type with a union, unit and integration
* Why do I still have `GeneratorSyntaxContextFactory` if it's not referenced anymore?