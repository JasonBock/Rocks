namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for the getters on indexers.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public sealed class IndexerGetterExpectations<T>
	: IndexerExpectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="IndexerGetterExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public IndexerGetterExpectations(IndexerExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}