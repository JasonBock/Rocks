namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for the setters on indexers.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
public sealed class IndexerSetterExpectations<T>
	: IndexerExpectations<T>
	where T : class
{
	/// <summary>
	/// Creates a new <see cref="IndexerSetterExpectations{T}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public IndexerSetterExpectations(IndexerExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}