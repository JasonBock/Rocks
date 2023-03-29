namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for indexers.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public class PropertyExpectations<T>
	: Expectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="PropertyExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public PropertyExpectations(Expectations<T> expectations)
	   : base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}