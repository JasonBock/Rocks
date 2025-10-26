What are the specific steps that I need to do?

* Everything goes on the generated expectations type now, not as extensions. So, for example, `PropertyExpectations`, `MethodExpectations` - all of that will change to targeting the expectations type.

* What about explicit implementations? This is problematic, because we don't want the expectations type to inherit directly from the implementing type. But, we have to put explicit implementations somewhere else. We may need to generate a `.ExplicitForXYZ` property on the expectations type so those expectations that will be implementing explicitly on the mock type work (and also does a naming context to not collide with members that exist on the mocked type).

* For methods:
    * Currently, Rocks does this: `expectations.Methods.CallThis(...)`. This needs to change to `expectations.CallThis(...)`. The adornments that are currently used will work here as the return value.
* For properties:
    * This is a little trickier. Currently, Rocks does this: `expectations.Properties.Getters.GetData()`. This needs to change to this: `expectations.GetData`. The adornments that are returned need to have way to specify which accessors are available to mock:
        * Getter: `expectations.GetData.Gets()`
        * Setter: `expectations.GetData.Sets(...)`
        * Initter: `expectations.GetData.Inits(...)`
    After that's done, the adornments can have the normal API
* For indexers:
    * This would be the same as properties. Currently, Rocks does this: `expectations.Indexers.Getters.This(...)`. This needs to change to this: `expectations[...]`. It would also have the same accessors for adornments:
        * Getter: `expectations[...].Get()`
        * Setter: `expectations[...].Sets(...)`
        * Initter: `expectations[...].Inits(...)`
* I don't think any new work has to be done for events, but it may be worth doing something different, like maybe putting an event on the expectations and using the `add` and `remove` thingees to add an event handler. Not sure about this.

* The generated `Instance()` methods will need a naming context to ensure they don't collide with the (rare) case of an existing member named `Instance`.

Maybe spend some time manually changing the generated code to the "shape" that I want, and see how that works.

* I should put `MemberIdentifierAttribute` on the specific `get/set/init`. That way, I don't have to pass in the `PropertyAccessor`. Like this:

```c#
public class PropId
{
	public Guid Id 
	{
		[global::Rocks.MemberIdentifier(2)]
		get;
		[global::Rocks.MemberIdentifier(3)]
		set;  
	}
}

Console.WriteLine(typeof(PropId).GetMemberDescription(2));
```

This prints: `System.Guid get_Id()`. This is a small thing, but I think it's clearer.

The `Handler.cs` and `Adornments.cs` file should be broken up. Maybe even create separate namespaces and that will need to be reflected in generated code.

Create an intermediary set of "property adornments" classes. Instances are returned:

* `GetPropertyAdornments`
    * `Gets()` => returns `Adornments<TAdornments, THandler, TCallback, TReturnValue>`
* `GetAndSetPropertyAdornments`
    * `Gets()` => returns `Adornments<TAdornments, THandler, TCallback, TReturnValue>`
    * `Sets(value)` => returns `Adornments<TAdornments, THandler, TCallback>`
* `GetAndInitPropertyAdornments`
    * `Gets()` => returns `Adornments<TAdornments, THandler, TCallback, TReturnValue>`
    * `Inits(value)` => returns `Adornments<TAdornments, THandler, TCallback>`
* `SetPropertyAdornments`
    * `Sets(value)` => returns `Adornments<TAdornments, THandler, TCallback>`
* `InitPropertyAdornments`
    * `Inits(value)` => returns `Adornments<TAdornments, THandler, TCallback>`

...maybe?

TODO:
* DONE - Update the `Arguments.cs`, `Adornments.cs`, and `Handler.cs` split up the types to one file each.
* DONE - Change `MemberIdentifier` to only take one parameter
* DONE - The mocking code can be put in a file-scoped namespaced - e.g. instead of `namespace Stuff { ... }`, it would be `namespace Stuff;`
* Create updated "Minimized" builders. This should make it easier to build things and make it different from all the others.
    * `MockMembersExpectationsBuilder` - This can take a `buildsForExplicit` or something like that to either emit `this.` or `this.parent` with a readonly `Expectations` reference in the method builder. This would also create the containing "explicit" class
        * `MethodExpectationsBuilder`
        * `PropertyExpectationsBuilder` - remember to put `[MemberIdentifier]` on the `get/set/init`, not on the property, with no `PropertyAccessor`
        * `IndexerExpectationsBuilder` - remember to put `[MemberIdentifier]` on the `get/set/init`, not on the property, with no `PropertyAccessor`
* DONE - The `Action<AdornmentsPipeline> adornmentsFQNsPipeline` thing in the expectations may go away with all of this and will not be needed.
* Need to ensure "defaults" in methods and indexers are handled correctly.
* Ensure original expectation implementations are no longer there
* Add some space between `HandlerX` and field definition groupings
* Update unit tests to ensure things are working
* Once I think everything is correct, update all unit and integration tests
* Run code-gen tests and hope they work
* Clean up builders
    * "Old" implementations are deleted
    * I think I can get rid of `PropertyExpectationTypeName`
* Update all documentation such that it reflects the new syntax.
    * Consider writing an "upgrade" doc that provides assisstance on how test code should be changes from the old to the new style.