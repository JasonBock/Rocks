namespace Rocks
{
	public sealed class IndexerSetterExpectations<T>
		: IndexerExpectations<T>
		where T : class
	{
		public IndexerSetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}