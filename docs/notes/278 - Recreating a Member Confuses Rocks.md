This should only do this:

```csharp
public class Mock
    : JsBinaryOperator
{
    public override object Evaluate(object target)
    {
        throw new NotImplementedException();
    }
}
```

But Rocks is generating this:

```csharp
public override object Evaluate(global::MockTests.ScriptScopeContext @scope)

public override object Evaluate(object @target)
```

The 2nd one is correct, but the first one should not be there. Well...it **can** be there, but it should not be calling the base. What happens is that Rocks thinks it can call `base()` because that overload of `Evaluate()` is not `abstract`, but it thinks it's call the abstract overload that takes an `object`. I'm not sure how I can "fake" C# out to let me call that one.

SharpLab.io says this can be compiled:

```csharp
public override object Evaluate(ScriptScopeContext scope)
{
    return (this as JsOperator).Evaluate(scope);
}
```

But that may end up in a stack overflow.

What **does** work is this:

```csharp
public override Tag Evaluate(ScriptScopeContext scope)
{
    return base.Evaluate(scope: scope);
}
```

Note that I included the name of the parameter. Because those are different, that resolves things.

So, maybe the thing to fix this is that when I call a base method. But, I need to capture the overridden and use **those** parameter names, because it's possible the overridden method changed the parameter names.

What about indexers? And shims?

I think I overthought it by looking at the Overridden. If it's abstract, that isn't the target we want. We literally want to target the method we're given, because it's not abstract.

Tests
* DONE - Run code gen tests
* Run integration tests
* Add one where the override changes the parameter name, it should still work and use the overriding method parameter name
* Add for MethodModel and PropertyModel
* Fix unit tests (lots of param name additions will cause diff errors)