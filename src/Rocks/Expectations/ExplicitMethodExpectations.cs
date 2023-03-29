namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for explicit methods.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
/// <typeparam name="TContainingType">The type that defines the methods.</typeparam>
public sealed class ExplicitMethodExpectations<T, TContainingType>
	: Expectations<T>
	where T : class, TContainingType
{
	/// <summary>
	/// Creates a new <see cref="ExplicitMethodExpectations{T, TContainingType}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public ExplicitMethodExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}