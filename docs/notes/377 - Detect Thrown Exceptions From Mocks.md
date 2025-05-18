Want to find:

* `throw new global::Rocks.Exceptions.ExpectationException(`, add `this.Expectations.WasExceptionThrown = true;` before it.
* `if (this.WasInstanceInvoked)`, change to `if (this.WasInstanceInvoked && !this.WasExceptionThrown)`