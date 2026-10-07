I'll keep it simple. Gather all the inaccessible abstract members, and emit them like "makes". No exceptions, just no-op-ish behavior.

Note: we can't remove `ROCK8` because of `HasInaccessibleAbstractMembersWithInvalidIdentifiers()`. Maybe we reword the explanation of the diagnostic.

```c#
public abstract class Holder
{
    protected Holder() { }

    public abstract Data Value { get; protected set; }

    protected internal struct Data { }
}
```

If the property is an inaccessible abstract member like the code above, what we need to do is ... well, I got it to work :)

Properties:
* DONE - `PropertyModelTests` - can't assume that `PropertyDeclarationSyntax` will exist, rather, pass in property name and look for that off of `ITypeSymbol`
* DONE - `InternalGeneratorTests.GenerateWhenAbstractClassUsedInaccessibleNestedClassInPropertyAsync()` - make sure it passes

TODOs:
* Types to change
    * DONE - `MockableMethods`
    * DONE - `MockableProperties`
        * DONE - Properties
        * DONE - Indexers
    * `MockableEvents`
    * `MockableConstructors` ?
        * Pass an `ImmutableArray<MockableXYZResult>` for `inaccessibleAbstractMembers`
        * During mock type creation, look for the `InaccesibleAbstractMembers`, and implement them like a "make" (no-op or return `default!`);
* DONE - For the makes, need to just put a `WriteLine()` at the end of every member generation, oh well, we'll have one extra blank line at the end, so be it :)
* Tests to change
    * DONE - Properties
        * DONE - Without intermediate
        * DONE - With intermediate
    * DONE - Indexers
        * DONE - Without intermediate
        * DONE - With intermediate
    * Events
        * DONE - Add case in `MockableEventDiscoveryTests`
        * Without intermediate
        * With intermediate
    * Constructors ?
        * Without intermediate
        * With intermediate
    * `MockModelTests.CreateWhenTargetHasInternalAbstractMembersAsync()` - **all** of them should have `Information` be non-null
* Might as well do #440 - handling events and constructor - while I'm here.
* Update overview docs
    * Basically move the intermediatary type doc on this issue into a separate bullet that describes what 11.0.0 will do
* Make sure all test that are failing are **only** failing because of code diffs.

Follow-ups:
* Ping MS Discord chat about how to handle unit testing of SGs in a "better"-ish way.