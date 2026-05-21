Create a `public enum CodeVisibility { Public, Internal }`. For `[Rock]` add another parameter that's optional and set to `Internal` (as that's the default right now). `[RockPartial]` will just assume the visibility of the partial type is what should be used for all gen'd code. Pass this value along with `TypeMockModel`, which will go into `MockModelInformation`.

There's an `Accessibility` property on `TypeMockModel`, but it's only used in two places: to declare the accessiblity for the outer expectations class for a create and make. So let's just reuse that, and then use it for all generated types:
* Create
    * DONE - Expectations (partial or otherwise)
    * SetupsExpectations, constructor, all setup members, and `Setups` property
        * Method
        * Property
        * Indexer
    * Handlers
    * Expectations constructor
    * `Instance()` methods
    * Adornments, interface, adornments types and their constructors
    * `Remove()` methods
* Make
    * Expectations (partial or otherwise)
    * `Instance()` methods