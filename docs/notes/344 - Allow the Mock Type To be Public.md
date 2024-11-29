* DONE - Make comment about the `final` CodeAnalysis package versions still in play
* DONE - Need to create `feature/9.1.0`
* Add `MockTypeVisibility` enum: `Public`, `Internal`, `Private`, can debate the first two.
* Create an optional `mockTypeVisibility` parameter with `RockAttribute` and `RockPartialAttribute`, default is `MockTypeVisibility.Private`
* That gets passed into the builder infrastructure.
* When the mock type is built, use that value to determine the visibility of the type
* Write a test to show CRGP in action
* Update docs
* Update changelog