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