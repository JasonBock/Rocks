namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock.
/// </summary>
/// <param name="type">The mock type to create.</param>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class RockAttribute<T>(BuildType type)
	: Attribute
	where T : class
{
	/// <summary>
	/// Gets the mock type.
	/// </summary>
	public BuildType Type { get; init; } = type;
}