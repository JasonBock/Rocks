namespace Rocks.Expectations
{
	public sealed class IndexerGetterExpectations<T>
		: IndexerExpectations<T>
		where T : class
	{
		public IndexerGetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}