namespace Rocks.Expectations;

public sealed class IndexerGetterExpectations<T>
	: IndexerExpectations<T>
	where T : class
{
	public IndexerGetterExpectations(IndexerExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}