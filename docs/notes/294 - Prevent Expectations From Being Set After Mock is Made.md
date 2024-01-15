Something like this should throw an `ExpectationException`:

```c#
var expectations = new MethodVoidTestsCreateExpectations();
expectations.Methods.OneParameter(3);

var mock = expectations.Instance();
mock.OneParameter(3);

expectations.Methods.OneParameter(4);
```

Expected: <Rocks.Exceptions.VerificationException> and property Message equal to "The following verification failure(s) occured: Mock type: Rocks.IntegrationTests.IDataCreateExpectations+Mock, member: int CalculateValue(), messsage: The expected call count is incorrect. Expected: 1, received: 0."

But was:  "The following verification failure(s) occured: Mock type: Rocks.IntegrationTests.IDataCreateExpectations+Mock, member: set_Value(value), messsage: The expected call count is incorrect. Expected: 1, received: 0."


global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
global::System.ArgumentNullException.ThrowIfNull(@value);
