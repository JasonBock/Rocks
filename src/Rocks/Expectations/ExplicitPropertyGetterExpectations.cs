namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for the getters on explicit properties.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
/// <typeparam name="TContainingType">The type that defines the properties.</typeparam>
public sealed class ExplicitPropertyGetterExpectations<T, TContainingType>
	: ExplicitPropertyExpectations<T, TContainingType>
	where T : class, TContainingType
{
	/// <summary>
	/// Creates a new <see cref="ExplicitPropertyGetterExpectations{T, TContainingType}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public ExplicitPropertyGetterExpectations(ExplicitPropertyExpectations<T, TContainingType> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}