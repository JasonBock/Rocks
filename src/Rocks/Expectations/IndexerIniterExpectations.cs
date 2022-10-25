namespace Rocks.Expectations;

public sealed class IndexerIniterExpectations<T>
	: IndexerExpectations<T>
	where T : class
{
	public IndexerIniterExpectations(IndexerExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}