namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for the getters on properties.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public sealed class PropertyGetterExpectations<T>
	: PropertyExpectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="PropertyGetterExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public PropertyGetterExpectations(PropertyExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}