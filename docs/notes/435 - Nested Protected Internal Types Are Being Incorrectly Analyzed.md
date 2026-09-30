Code gen results before changes:

        Total assembly count is 124
        Total discovered type count is 15906

OK, we may be able to have the mock derive from something that is using a `protected` nested type, but the problem remains that constraints that use that nested type and all of our gen'd expectation code and other things can't "see" the nested type. So it doesn't matter, we can't do that.

OK! I think there's another way. First, I keep track of inaccessible symbol **lists**, not a boolean. So, instead of:

```c#
var inaccessibleAbstractMembers = false;
```

do this:

```c#
var inaccessibleAbstractMembers = new List<IMethodSymbol>();
```

When I need to add to it, I just do:

```c#
inaccessibleAbstractMembers.Add(hierarchyMethod);
```

and then say "there are inaccesible members" by saying `inaccessibleAbstractMembers.Length > 0`

**However**, the key point is that, if a method comes up that I'm adding to the list of candidates where `IsOverride` is `true`, then do this:

```c#
inaccessibleAbstractMembers.Remove(hierarchyMethod.OverriddenMethod);
```

Then, what a user can do is create an intermediary type:

```c#
public class TestClientBase
    : ClientBase<TestClientBase>
{
    protected override sealed TestClientBase NewInstance(ConfigurationTypeThing configuration) => new(configuration);
}
```

and that **should** get rid of the member so it allows the user to still mock other members on the type. Maybe a future enhancement could be to generate the intermediary type that takes the inaccessible members, create a type between the target type and the mock type that does the "override sealed" gymnastics for you, so you don't have to do this - it'll be transparent.

This needs to be done on:
* DONE - `MockableMethodDiscovery`
* DONE - `MockablePropertyDiscovery`
* IGNORED (see TODO) - `MockableEventDiscovery`

`MockableConstructorDiscovery` **shouldn't** matter because we only look at the constructors on the mock type. The user may need to add constructors to "mask" mock constructors so inaccessible types won't show up.

TODO:
* DONE - Write up a feature to potentially do this automatically for the user. That is, they say they want to mock `ClientBase<>`, we detect that there's overriden inaccessiable abstract members, so we create this intermediately type underneath the scenes, and that's what the mock type derives from. This can be problematic because we can't call the base member as it's `abstract`, and we don't know what the user will want to do in that case.
* DONE - Write up a feature to handle events similar to properties and methods - that is, look through the hierachy and process `IEventSymbol` values the "same" way.
* DONE - Write up a feature to see if I can list the specific members for diagnostics like `ROCK8` as that would be very nice for user experience. I really don't want to get into a case where it could be a large number of inaccessible members, but not knowing what any of them are makes it hard to diagnose and troubleshoot.