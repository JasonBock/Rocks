namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock of type "create" built.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RockCreateAttribute
	: Attribute
{
	/// <summary>
	/// Create a new <see cref="RockCreateAttribute"/> instance.
	/// </summary>
	/// <param name="mockType">The type to mock.</param>
	public RockCreateAttribute(Type mockType) =>
		this.MockType = mockType;

	/// <summary>
	/// Gets the type to mock.
	/// </summary>
	public Type MockType { get; }
}

/// <summary>
/// Used to specify a type that should have a mock of type "create" built.
/// </summary>
/// <typeparam name="T">The type to mock.</typeparam>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RockCreateAttribute<T>
	: Attribute
	where T : class
{ }