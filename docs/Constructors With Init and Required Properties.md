# Constructors With Init and Required Properties

These are some random ideas around `init` properties and constructors. I may need to also handle [required members](https://github.com/dotnet/csharplang/blob/main/proposals/required-members.md), which is not just for properties but fields as well.

## `init` Properties

For `init` properties, would this work?

```
public interface ITest
{
	int A { get; init; }
	string B { get; init; }
	Guid C { get; init; }
}

public ITest Instance(int AProperty = default, string BProperty = default, Guid CProperty = default) =>
	new MockITest() { A = AProperty, B = BProperty, C = CProperty };

var chunk = rock.Instance(BProperty: "value");
```

This way you can choose which properties to initialize, if the caller doesn't provide a value, they'll just be the default anyway.

I think this is legal syntax. Now, if constructors are involved:

```
public class Test
{
	public Test(string value) { }
	
	public virtual int A { get; init; }
	public virtual string B { get; init; }
	public virtual Guid C { get; init; }
}

public Test Instance(string value, int AProperty = default, string BProperty = default, Guid CProperty = default) =>
	new MockTest(value) { A = AProperty, B = BProperty, C = CProperty };
```

I can see all sorts of issues popping up with name collisions, and what about properties that aren't mockable but do have inits that are accessible? Would we want to include those as well in the parameter list?

I tried a POC:

```
static Test Instance(string value, int AProperty = default, string? BProperty = default, Guid CProperty = default) =>
	new Test(value) { A = AProperty, B = BProperty, C = CProperty };

var test1 = Instance("a", AProperty: 4);
Console.WriteLine(test1);

var test2 = Instance("a", BProperty: "B value", CProperty: Guid.NewGuid());
Console.WriteLine(test2);

public class Test
{
	public Test(string value) { }

	public virtual int A { get; init; }
	public virtual string? B { get; init; }
	public virtual Guid C { get; init; }

	public override string ToString() =>
		$"A = {this.A}, B = {this.B}, C = {this.C}";
}
```

And this gives:

```
A = 4, B = , C = 00000000-0000-0000-0000-000000000000
A = 0, B = B value, C = cbdf9c87-881f-41a0-8db6-2920b06436fd
```

So...there's hope. But this would mean I would have to put a bunch of stuff back in from issue #158 :|. Well, maybe. We may not need to have expectations on its initialization, but we may need to have some of the props (esp. ones that can't be mocked) set to initial values.

## `required` Properties

Required properties have been proposed for C#, and there's talk of them showing up in C# 11, but nothing is committed yet. In fact, at this point, this can't be used as a preview feature just yet (though [sharplab.io](https://sharplab.io/) allows you to play with it).

We need to be concerned with this, because, while properties on interfaces cannot be marked with `required`, an `abstract` class, or a class with `virtual` properties can be `required`, and then an override **must** be `required` as well:

```
using System;

public abstract class HaveRequiredProperties 
{
    public abstract string Data { get; set; }    
    public abstract required Guid Id { get; set; }
}

public class RequiredProperties
    : HaveRequiredProperties
{
    public override string Data { get; set; }    
    public required override Guid Id { get; set; }
}

var rp = new RequiredProperties { Id = Guid.NewGuid() };
```

This means I need a way to set all these properties in the `Instance()` calls.

Here's the idea. I create a nested class called `RequiredPropertyValues`:

```
namespace Rocks.IntegrationTests
{
	internal static class CreateExpectationsOfRequiredPropertiesExtensions
	{
		internal static RequiredProperties Instance(this Expectations<RequiredProperties> self, RequiredPropertyValues requiredPropertyValues)
		{
			if (self.Mock is null)
			{
				var mock = new RockRequiredProperties(self) { Id = requiredPropertyValues.Id };
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException("Can only create a new mock once.");
			}
		}
		
		internal sealed class RequiredPropertyValues
		{
			internal required Guid Id { get; set; }		
		}
	}
	
	// More mock stuff is generated here...
```

This type name would always be the same, and the parameter name would always be camel-cased of it. The odds of this running into a name collision would be really small. If this **does** happen, I can always create some kind of configuration for Rocks where one of the values could be `RequiredPropertyValuesClassName`. But again, I don't see the need for this right now.

The generated code simply includes all the required property definitions into `RequiredPropertyValues`, and then adds a last parameter to every `Instance()` method. This will force the user to specify the required parameters. In turn, these values would be mapped to the mock instance.

If a constructor has any optional parameters, this `RequiredPropertyValues` parameter has to go first. This is a corner-case, but it has to be done that way.

If a property is both `init` and `required`, it would be `required`. In other words, I wouldn't make an optional parameter in the `Instance()` methods for this property; it would be handled by the generated `RequiredPropertyValues` class.