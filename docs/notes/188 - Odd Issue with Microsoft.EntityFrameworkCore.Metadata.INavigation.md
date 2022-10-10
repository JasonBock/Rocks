This one is going to be a doozy. The inheritance hierarchy is fairly deep with some redundancy, there are `new` hiding members everywhere, DIMs are present...and the resulting gen-d code is a mess. Something is tripping it up somewhere.

So, what does the hierarchy look like?

* Microsoft.EntityFrameworkCore.Metadata.INavigation
  * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyNavigation
    * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyNavigationBase
      * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyPropertyBase
	    * Microsoft.EntityFrameworkCore.Infrastructure.IReadOnlyAnnotatable
  * Microsoft.EntityFrameworkCore.Metadata.INavigationBase
    * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyNavigationBase
      * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyPropertyBase
	    * Microsoft.EntityFrameworkCore.Infrastructure.IReadOnlyAnnotatable
	* Microsoft.EntityFrameworkCore.Metadata.IPropertyBase
      * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyPropertyBase
	    * Microsoft.EntityFrameworkCore.Infrastructure.IReadOnlyAnnotatable
	  * Microsoft.EntityFrameworkCore.Infrastructure.IAnnotatable
        * Microsoft.EntityFrameworkCore.Infrastructure.IReadOnlyAnnotatable
		
That's a lot to see, but there is some redundancy. Here's a "flattened" version of that:

* Microsoft.EntityFrameworkCore.Metadata.INavigation
  * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyNavigation
    * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyNavigationBase
      * Microsoft.EntityFrameworkCore.Metadata.IReadOnlyPropertyBase
	    * Microsoft.EntityFrameworkCore.Infrastructure.IReadOnlyAnnotatable
  * Microsoft.EntityFrameworkCore.Metadata.INavigationBase
	* Microsoft.EntityFrameworkCore.Metadata.IPropertyBase
      * Microsoft.EntityFrameworkCore.Infrastructure.IAnnotatable
        * Microsoft.EntityFrameworkCore.Infrastructure.IReadOnlyAnnotatable

I'm wondering if I need to do something like, if I've already "seen" an interface when I'm trying to find mockable members, just ignore it. Though that may be the wrong approach as well.

Where it seems to be getting broken is in the shim generation. It's creating a bunch for `INavigation` with different interface types, and when it gets to the second one, it starts to do this odd indentation that once it finally gets out of, it appears to be invalid code. Plus, the shims have the same type name, so that won't work either.

So, first thing. The shim name will be same if I detect two or more shims. The shims are referenced using a FQN, so that's good. But I think I need to use the hashcode technique to come up with a unique name for each shim type. Furthermore, I need to use FQNs when I create new instances of them in the constructor.

I think it might be in `ShimMethodBuilder`. It's not outdenting correctly. And when I `.Indent--`, now everything looks right.

OK, so I think I have two remaining problems:
* Come up with unique names for a shim type. Use the hashcode technique to do this. Maybe "ShimRock{InterfaceName}{hash code of FQN InterfaceName}"
* When a shim is created in the constructor, it needs to use the FQN (keep in mind the hashcode name as well)

D'oh, shim types don't need to be FQN-ed, they are nested private classes to the mock type itself.

Oops. So I'm realizing that shims are kind of a mix of a simple mock and a make, especially when the target interface inherits other interfaces.

```csharp
using Rocks;

public interface IDoNotHaveADim
{
	int IAmNotADim();
}

public interface IHaveADim
	: IDoNotHaveADim
{
	int IAmADim() => 2;
}

public static class Test
{
	public static void Go() => Rock.Create<IHaveADim>();
}
```

Rocks will make a shim that will not implement `IAmADim()` as expected, but it needs to implement `IAmNotADim()`. Otherwise I get `CS0535`. So, I need to look at all the non-DIM members of an interface, and provide "make" implementations of them that do nothing. They will **never** be called by Rocks because will either try to match an handler to the invocation, or throw an exception, so they can essentially do nothing like what happens in a make, but they still need to show up in the shim type.