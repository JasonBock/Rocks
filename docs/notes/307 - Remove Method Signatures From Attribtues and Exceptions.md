Steps:

* DONE - Change `[MemberIdentifier]` to just take the identifier
* DONE - Update code that emits `[MemberIdentifier]` to only pass the identifier
* DONE - Change `GetMemberDescription()` to creating the signature using Reflection
* DONE - Update passing the method signature to exceptions using `GetMethodSignature()`
    * DONE - In method bodies
    * DONE - In `Expectations.Verify()`
* DONE - Fix all tests