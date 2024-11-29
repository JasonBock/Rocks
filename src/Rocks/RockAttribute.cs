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
   /// <param name="visibility">The visibility of the mock - default is <see cref="MockTypeVisibility.Private"/>.</param>
   public RockAttribute(Type mockType, BuildType buildType, MockTypeVisibility visibility = MockTypeVisibility.Private) =>
		(this.MockType, this.BuildType, this.Visibility) = (mockType, buildType, visibility);

	/// <summary>
	/// Gets the kind of mock to build.
	/// </summary>
	public BuildType BuildType { get; }
	/// <summary>
	/// Gets the type to mock.
	/// </summary>
	public Type MockType { get; }
	/// <summary>
	/// Gets the visibility of the mock type.
	/// </summary>
	public MockTypeVisibility Visibility { get; }
}