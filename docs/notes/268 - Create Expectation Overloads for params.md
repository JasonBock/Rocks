We need to do a couple of things:

* During expectations generation for a method and indexers
    * If the `params` is **not** ref-like, then we do another generation pass. In other words, change this: `needsGenerationWithDefaults |= parameter.HasExplicitDefaultValue || parameter.IsParams;` to this: `needsGenerationWithDefaults |= parameter.HasExplicitDefaultValue || (parameter.IsParams && !parameter.Type.IsRefLikeType)`;
    * If a 2nd pass is needed (e.g. because there's an optional parameter), then if the `params` argument **is** ref-like, we still generate with `RefStructArgument` as the parameter, and simple pass that over to the overload.

Another snag: `void Do(int value = 3, params ReadOnlySpan<string> args);`. What will happen is we'll gen the 2nd expectation method like this: `Do(int @value = 3, global::Rocks.RefStructArgument<global::System.ReadOnlySpan<string>> @args)`, and that's wrong, because we can't have the optional in front defined like this. We could do:

```c#
Do([global::System.Runtime.InteropServices.OptionalAttribute, global::System.Runtime.InteropServices.DefaultParameterValueAttribute(3)] int @value, global::Rocks.RefStructArgument<global::System.ReadOnlySpan<string>> @args)
```

We'd have to detect if those attributes exist or not before we define the method this way. This gets to, how would Rocks handle this in the first place, outside of a `params`? In other words, what would Rocks do with this:

```c#
public interface IStuff
{
    void Perform(int mustHave,
        string data,
        int value = 3);
}
```

Or this?

```c#
using System.Runtime.InteropServices;

public interface IStuff
{
    void Perform(int mustHave,
        string data,
        [Optional, DefaultParameterValue(3)] int value);
}
```

Or this?

```c#
using System.Runtime.InteropServices;

public interface IStuff
{
    void Perform(int mustHave,
        [Optional, DefaultParameterValue(3)] int value, 
        string data);
}
```

Once I go through this, then I think I'll have enough information to handle what I'm going to do with this very small corner case with optionals and ref-like params types.

TODO:
* DONE - If a parameter is optional via the attributes, then:
    * DONE - When defining the expectations, we must include the attributes and not put the default value with the parameter - i.e. `int @value = 3`
    * DONE - When defining the mock and/or the make, we are including the attributes, but we cannot assign the parameter to the default value.
* DONE - If we have the case where an optional parameter comes before a ref-like `params` parameter and the optional does not use the attributes, then we **must** define the optional parameter with the 2nd pass overload with attributes.
* Need unit **and** integration tests for all these scenarios, especially indexers
    * Indexer versions (probably need to update the indexer expectations getters and setters, and implementation...maybe, they may be right)
        * DONE - GenerateWhenParamsInIndexerIsNotRefStructAndOptionalExistsWithAttributesAsync
        * DONE - GenerateWhenParamsInIndexerIsNotRefStructAndOptionalExistsWithoutAttributesAsync
        * DONE - GenerateWhenParamsInIndexerIsRefStructAndOptionalExistsWithAttributesAsync
        * GenerateWhenParamsInIndexerIsRefStructAndOptionalExistsWithoutAttributesAsync
* Code gen tests must pass