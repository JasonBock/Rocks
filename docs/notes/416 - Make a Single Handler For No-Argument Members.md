The only two cases are:

* Methods with no type parameters and no parameters
* Property getters

We also need to change the case where a method has type parameters but may not have parameters. We need to handle that as `List<>` and cast accordingly in the method implementation.

Where do we change this?
* DONE - In the setup implementation
* DONE - Changing `List<HandlerX>?` to `HandlerX?`
* DONE - In `Verify()` we need to pass in a collection expression - i.e. `failures.AddRange(this.Verify([this.handlers0], 0));`
* DONE - In the mocked member implementation, change `this.Expectations.handlers0[0];` to `this.Expectations.handlers0;`
* DONE - In the `Remove()`
    * DONE - Change `adornments.Remove(this.@handlers0);` to `this.@handlers0 = null;` - that is, a one-liner, this isn't technically "correct" but "close enough"
* DONE - Create an implementation test for a method with no parameters but type parameters
* DONE - Run implementation and code gen tests to ensure we didn't break anything

TODO:
* Should `init` properties and indexers also only be "one-shot" - that is, they're only called once during the construction of the mock instance. NO, don't do this. Reason is, if the type has a constructor, it's possible the base constructor may call the property or indexer with different values. So no, we can't limit it to one call.

```c#
public interface IThing
{
    void Stuff<T>();
    void Do<T, TValue>(TValue value);
}
```

Stuff<string>() => ...
Stuff<int>() => ...