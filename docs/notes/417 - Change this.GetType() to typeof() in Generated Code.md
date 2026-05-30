* DONE - `Expectations`
    * DONE - Get rid of `MockType`
    * DONE - Add `Type mockType` as a parameter to `Verify()` and change all call sites to call `Expectations.Verify<..>(handlers, memberIdentifer, typeof(Mock))` in `MockExpectationsVerifyBuilder`
* DONE - `this.GetType()` should be removed and/or replaced with `typeof(MockTypeName)`
    * DONE - `MockConstructorExtensionsBuilder`
    * DONE - `ExpectationExceptionBuilder`
    * DONE - `MockEventsBuilder`

TODO:
* I may be doing this already, but in the code gen tests, show the "top N" slowest code generations, along with the number of mocked members, for potential improvements.