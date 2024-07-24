* DONE - Make `BuildType` `[Flags]`

* Then, add attribtue + tests:

```c#
namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock built.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class RockAttribute
  : Attribute
{
   /// <summary>
   /// Creates new <see cref="RockAttribute"/> instance.
   /// </summary>
   /// <param name="mockType">The type to mock.</param>
   /// <param name="buildType">The type of mock to build.</param>
   public RockAttribute(Type mockType, BuildType buildType) => 
		(this.MockType, this.BuildType) = (mockType, buildType);

   /// <summary>
   /// Gets the kind of mock to build.
   /// </summary>
   public BuildType BuildType { get; }
	/// <summary>
	/// Gets the type to mock.
	/// </summary>
	public Type MockType { get; }
}
```

* git commit

* Update generator to `RockGenerator`, and add support for this Attribute

* Add tests for this, and then commit.

* Update `RockCreateAttribute` and `RockMakeAttribute` to be obsolete:

```c#
[Obsolete("This attribute will be removed in 9.0.0. Please use RockAttribute instead.", false)]
```

* `ITypeSymbolExtensions.HasErrors()` could be updated to this:

```c#
	internal static bool HasErrors(this ITypeSymbol self)
	{
		if (self.TypeKind == TypeKind.Error)
		{
			return false;
		}

		if (self is INamedTypeSymbol namedSelf && namedSelf.TypeParameters.Length > 0)
		{
			for (var i = 0; i < namedSelf.TypeArguments.Length; i++)
			{
				var selfTypeArgument = namedSelf.TypeArguments[i];
				var selfTypeParameter = namedSelf.TypeParameters[i];

				if (!SymbolEqualityComparer.Default.Equals(selfTypeArgument, selfTypeParameter) &&
					!selfTypeArgument.HasErrors())
				{
					return false;
				}
				else
				{
					continue;
				}
			}
		}

		return true;
	}
```

* `INamedTypeSymbolExtensions.HasOpenGenerics()` can be deleted

* Update `RockAnalyzer` to include `RockAttribute`