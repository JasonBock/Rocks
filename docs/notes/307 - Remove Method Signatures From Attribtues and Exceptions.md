Steps:

* DONE - Change `[MemberIdentifier]` to just take the identifier
* DONE - Update code that emits `[MemberIdentifier]` to only pass the identifier
* DONE - Change `GetMemberDescription()` to creating the signature using Reflection
* Update passing the method signature to exceptions using `GetMethodSignature()`
    * In method bodies
    * In `Expectations.Verify()`