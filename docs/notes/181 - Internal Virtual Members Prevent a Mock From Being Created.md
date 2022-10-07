https://github.com/JasonBock/Rocks/issues/181

First, create the test.

Need to not filter on `CanBeSeenByContainingAssembly()`. Then, if this is `false`, and the member is `abstract`. This has to be done with both interfaces and classes.

HasInaccessibleAbstractMembers