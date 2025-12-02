# Updating Expectation Calls in Rocks 10.0.0

With version 10 and up, Rocks has a new expectations API that is sligtly different than previous versions. If you're updating to 10.0.0 or above, you should be able to change test code fairly quickly by following these recommendations:

* Methods
    * Replace `.Methods()` with `.Setups`
* Properties
    * Replace `.Properties().Getters().Target()` with `.Setups.Target.Gets()`
    * Replace `.Properties().Setters().Target(...)` with `.Setups.Target.Sets(...)`
* Indexers
    * Replace `.Indexers().Getters().This(...)` with `.Setups[...].Gets()`
    * Replace `.Indexers().Setters().This(value, ...)` with `.Setups[...].Sets(value)`

For example, let's say the target type to mock looked like this:

```c#
public interface ITarget
{
    int Process(Guid id);
    string Target { get; set; }
    string this[int index] { get; set; }
}
```

Previous Rocks code would look something like this:

```c#
[assembly: Rock(typeof(ITarget), BuildType.Create)]

using var context = new RockContext();
var expectations = context.Create<ITargetCreateExpectations>();
expectations.Methods().Process(Arg.Any<Guid>()).ReturnValue(3);
expectations.Properties().Getters().Target().ReturnValue("target");
expectations.Properties().Setters().Target("target");
expectations.Indexers().Getters().This(42).ReturnValue("value");
expectations.Indexers().Setters().This("value", 42);
```

In 10.0.0 and beyond, it'll look like this:

```c#
[assembly: Rock(typeof(ITarget), BuildType.Create)]

using var context = new RockContext();
var expectations = context.Create<ITargetCreateExpectations>();
var setups = expectations.Setups;
setups.Process(Arg.Any<Guid>()).ReturnValue(3);
setups.Target.Gets().ReturnValue("target");
setups.Target.Sets("target");
setups[42].Gets().ReturnValue("value");
setups[42].Sets("value");
```

Note that in this example, the `Setups` property is captures as a reference in the `setups` local variable. This is not required - you could have done this:

```c#
[assembly: Rock(typeof(ITarget), BuildType.Create)]

using var context = new RockContext();
var expectations = context.Create<ITargetCreateExpectations>();
expectations.Setups.Process(Arg.Any<Guid>()).ReturnValue(3);
expectations.Setups.Target.Gets().ReturnValue("target");
expectations.Setups.Target.Sets("target");
expectations.Setups[42].Gets().ReturnValue("value");
expectations.Setups[42].Sets("value");
```

For mocking scenarios that require multiple expectation setups on one mock instance, it may be somewhat easier to use the "capture" technique. It really comes down to personal preference.
