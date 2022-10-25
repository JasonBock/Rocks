namespace Rocks.Expectations;

public sealed class IndexerInitializerExpectations<T>
	: IndexerExpectations<T>
	where T : class
{
	public IndexerInitializerExpectations(IndexerExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}