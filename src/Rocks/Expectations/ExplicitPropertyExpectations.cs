namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for explicit properties.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
/// <typeparam name="TContainingType">The type that defines the properties.</typeparam>
public class ExplicitPropertyExpectations<T, TContainingType>
	: Expectations<T>
	where T : class, TContainingType
{
	/// <summary>
	/// Creates a new <see cref="ExplicitPropertyExpectations{T, TContainingType}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public ExplicitPropertyExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}