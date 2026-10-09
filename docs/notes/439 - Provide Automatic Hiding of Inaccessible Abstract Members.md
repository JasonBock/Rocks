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

There's a difference between:

```c#
// There's no [InternalsVisibleTo]
internal abstract void Work();
```

and

```c#
protected abstract void Work(Data value);

// Nested type
protected class Data { }
```

The second one, we can automatically no-op the implementation - it's "accessible" from the mock, but only the mock type that we generate. The first one, this is truly a "inaccesible abstract member" and we can't handle it.

```c#
public abstract class InternalTargets { public abstract void VisibleWork(); internal abstract void Work(); }

public abstract class IntermediateInternalTargets
	: InternalTargets
{
	internal override sealed void Work() => throw new NotImplementedException();
}
```

The assembly itself would need to provide that intermediary type for you if you wanted to provide your own implementation. Or the developer is saying, `InternalTargets` is publicly visible, but there's some factory method that returns instance of it via derived types that are within the assembly.

If a member is `protected` or `protected internal`, Rocks can mock it **if** all the types related to the member (e.g. parameter types, return types, property types, event types) are "accessible".

If the member itself is inaccessible, Rocks can't mock it. End of story.
If the member itself is accessible, but there's at least one inaccessible type member, this is what we're currently calling "InaccessibleAbstractMembers"

* Accessible - a member that can be mocked
* InaccessibleAbstractOutsideOfMock - a member that can't be mocked, but does not prevent a mock from being made. It will be implemented like a "make" would do.
* InaccessibleAbstractOutsideOfAssembly - a member that can't be mocked. For example:`internal abstract void Work();` with no `[InternalsVisibleTo]`


Properties:
* DONE - `PropertyModelTests` - can't assume that `PropertyDeclarationSyntax` will exist, rather, pass in property name and look for that off of `ITypeSymbol`
* DONE - `InternalGeneratorTests.GenerateWhenAbstractClassUsedInaccessibleNestedClassInPropertyAsync()` - make sure it passes

TODOs:
* Types to change
    * DONE - `MockableMethods`
    * DONE - `MockableProperties`
        * DONE - Properties
        * DONE - Indexers
    * DONE - `MockableEvents`
* DONE - For the makes, need to just put a `WriteLine()` at the end of every member generation, oh well, we'll have one extra blank line at the end, so be it :)
* Tests to change
    * DONE - Properties
        * DONE - Without intermediate
        * DONE - With intermediate
    * DONE - Indexers
        * DONE - Without intermediate
        * DONE - With intermediate
    * DONE - Events
        * DONE - Add case in `MockableEventDiscoveryTests`
        * DONE - Without intermediate
        * DONE - With intermediate
    * `MockModelTests.CreateWhenTargetHasInternalAbstractMembersAsync()` - **all** of them should have `Information` be non-null
* Might as well do #440 - handling events and constructor - while I'm here.
* Update overview docs
    * Basically move the intermediatary type doc on this issue into a separate bullet that describes what 11.0.0 will do
* Make sure all test that are failing are **only** failing because of code diffs.

Follow-ups:
* Ping MS Discord chat about how to handle unit testing of SGs in a "better"-ish way.