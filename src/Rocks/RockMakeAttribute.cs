namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock of type "make" built.
/// </summary>
[Obsolete("This attribute will be removed in 9.0.0. Please use RockAttribute instead.", false)]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RockMakeAttribute
	: Attribute
{
	/// <summary>
	/// Create a new <see cref="RockMakeAttribute"/> instance.
	/// </summary>
	/// <param name="mockType">The type to mock.</param>
	public RockMakeAttribute(Type mockType) =>
		this.MockType = mockType;

	/// <summary>
	/// Gets the type to mock.
	/// </summary>
	public Type MockType { get; }
}

/// <summary>
/// Used to specify a type that should have a mock of type "make" built.
/// </summary>
/// <typeparam name="T">The type to mock.</typeparam>
[Obsolete("This attribute will be removed in 9.0.0. Please use RockAttribute instead.", false)]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RockMakeAttribute<T>
	: Attribute
	where T : class
{ }