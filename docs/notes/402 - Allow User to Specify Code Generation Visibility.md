Create a `public enum CodeVisibility { Public, Internal }`. For `[Rock]` add another parameter that's optional and set to `Internal` (as that's the default right now). `[RockPartial]` will just assume the visibility of the partial type is what should be used for all gen'd code. Pass this value along with `TypeMockModel`, which will go into `MockModelInformation`.

There's an `Accessibility` property on `TypeMockModel`, but it's only used in two places: to declare the accessiblity for the outer expectations class for a create and make. So let's just reuse that, and then use it for all generated types:
* DONE - Create
    * DONE - Expectations (partial or otherwise)
    * DONE - SetupsExpectations, constructor, all setup members, and `Setups` property
        * DONE - Method
        * DONE - Property
        * DONE - Indexer
    * DONE - Handlers
    * DONE - Expectations constructor
    * DONE - `Instance()` methods
    * DONE - Adornments, interface, adornments types and their constructors
    * DONE - `Remove()` methods
    * DONE - Events
* DONE - Make
    * DONE - Expectations (partial or otherwise)
    * DONE - `Instance()` methods