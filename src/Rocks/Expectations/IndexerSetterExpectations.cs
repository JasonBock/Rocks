namespace Rocks.Expectations;

public sealed class IndexerSetterExpectations<T>
	: IndexerExpectations<T>
	where T : class
{
	public IndexerSetterExpectations(IndexerExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}