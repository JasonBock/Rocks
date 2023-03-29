namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for the setters on properties.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public sealed class PropertySetterExpectations<T>
	: PropertyExpectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="PropertySetterExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public PropertySetterExpectations(PropertyExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}