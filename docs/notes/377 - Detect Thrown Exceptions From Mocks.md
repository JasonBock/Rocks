Want to find:

* DONE - `throw new global::Rocks.Exceptions.ExpectationException(`, add `this.Expectations.WasExceptionThrown = true;` before it.
* DONE - `if (this.WasInstanceInvoked)`, change to `if (this.WasInstanceInvoked && !this.WasExceptionThrown)`
* DONE - Fix all unit tests
* Minor improvement 
    * `WriteLine()` to `WriteLines()` in `MockPropertyBuilder`
    * `NullabilityGeneratorTests.GenerateWhenTargetIsInterfaceAndMethodIsConstrainedByTypeParameterThatIsAssignedAsync()`, look at `As<T>()`, the `else` formatting is weird. Also `ConstraintsGeneratorTests.GenerateTargetClassAsync()`.