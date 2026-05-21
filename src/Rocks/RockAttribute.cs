namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock built.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class RockAttribute
  : Attribute
{
	/// <summary>
	/// Creates a new <see cref="RockAttribute"/> instance.
	/// </summary>
	/// <param name="mockType">The type to mock.</param>
	/// <param name="buildType">The type of mock to build.</param>
	/// <param name="codeVisibility">The visibility of the mock members, defaults to <see cref="CodeAccessibility.Public"/>.</param>
	public RockAttribute(Type mockType, BuildType buildType, CodeAccessibility codeVisibility = CodeAccessibility.Public) =>
		(this.MockType, this.BuildType, this.CodeVisibility) = (mockType, buildType, codeVisibility);

	/// <summary>
	/// Gets the kind of mock to build.
	/// </summary>
	public BuildType BuildType { get; }
	/// <summary>
	/// Gets the visibility of generated members.
	/// </summary>
	public CodeAccessibility CodeVisibility { get; }
	/// <summary>
	/// Gets the type to mock.
	/// </summary>
	public Type MockType { get; }
}