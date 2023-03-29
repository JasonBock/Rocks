namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for indexers.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public class IndexerExpectations<T>
	: Expectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="IndexerExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public IndexerExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}