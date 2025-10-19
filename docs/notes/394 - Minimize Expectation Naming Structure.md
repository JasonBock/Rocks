What are the specific steps that I need to do?

* Everything goes on the generated expectations type now, not as extensions. So, for example, `PropertyExpectations`, `MethodExpectations` - all of that will change to targeting the expectations type.

* What about explicit implementations? This is problematic, because we don't want the expectations type to inherit directly from the implementing type. But, we have to put explicit implementations somewhere else. We may need to generate a `.ExplicitForXYZ` property on the expectations type so those expectations that will be implementing explicitly on the mock type work (and also does a naming context to not collide with members that exist on the mocked type).

* For methods:
    * Currently, Rocks does this: `expectations.Methods.CallThis(...)`. This needs to change to `expectations.CallThis(...)`. The adornments that are currently used will work here as the return value.
* For properties:
    * This is a little trickier. Currently, Rocks does this: `expectations.Properties.Getters.GetData()`. This needs to change to this: `expectations.GetData`. The adornments that are returned need to have way to specify which accessors are available to mock:
        * Getter: `expectations.GetData.Get()`
        * Setter: `expectations.GetData.Set(...)`
        * Initter: `expectations.GetData.Init(...)`
    After that's done, the adornments can have the normal API
* For indexers:
    * This would be the same as properties. Currently, Rocks does this: `expectations.Indexers.Getters.This(...)`. This needs to change to this: `expectations[...]`. It would also have the same accessors for adornments:
        * Getter: `expectations[...].Get()`
        * Setter: `expectations[...].Set(...)`
        * Initter: `expectations[...].Init(...)`
* I don't think any new work has to be done for events, but it may be worth doing something different, like maybe putting an event on the expectations and using the `add` and `remove` thingees to add an event handler. Not sure about this.

* The generated `Instance()` methods will need a naming context to ensure they don't collide with the (rare) case of an existing member named `Instance`.