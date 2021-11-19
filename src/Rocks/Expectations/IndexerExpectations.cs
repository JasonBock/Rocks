namespace Rocks.Expectations;

public class IndexerExpectations<T>
	: ExpectationsWrapper<T>
	where T : class
{
	public IndexerExpectations(Expectations<T> expectations)
		: base(expectations) { }
}