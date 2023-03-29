namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for methods.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public sealed class MethodExpectations<T>
	: Expectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="MethodExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public MethodExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}