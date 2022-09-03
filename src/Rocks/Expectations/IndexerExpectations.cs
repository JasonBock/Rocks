namespace Rocks.Expectations;

public class IndexerExpectations<T>
	: Expectations<T>
	where T : class
{
	public IndexerExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}