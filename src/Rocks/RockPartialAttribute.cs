namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock built
/// based on the type the attribute is specific on.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class RockPartialAttribute
	: Attribute
{
	/// <summary>
	/// Creates a new <see cref="RockPartialAttribute"/> instance.
	/// </summary>
	/// <param name="mockType">The type to mock.</param>
	/// <param name="buildType">The type of mock to build.</param>
	/// <remarks>
	/// Only one <see cref="BuildType"/> flag can be specified.
	/// If more than one exists within <paramref name="buildType"/>,
	/// the type will default to <see cref="BuildType.Create"/>.
	/// </remarks>
	public RockPartialAttribute(Type mockType, BuildType buildType)
	{
		this.MockType = mockType;

		if (buildType.HasFlag(BuildType.Create) && buildType.HasFlag(BuildType.Make))
		{
			this.BuildType = BuildType.Create;
		}
		else
		{
			this.BuildType = buildType;
		}
	}

	/// <summary>
	/// Gets the kind of mock to build.
	/// </summary>
	public BuildType BuildType { get; }
	/// <summary>
	/// Gets the type to mock.
	/// </summary>
	public Type MockType { get; }
}
