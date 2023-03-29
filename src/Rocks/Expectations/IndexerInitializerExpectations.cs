namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for the initializers on indexers.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public sealed class IndexerInitializerExpectations<T>
	: IndexerExpectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="IndexerInitializerExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public IndexerInitializerExpectations(IndexerExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}