namespace Rocks
{
	public sealed class ExplicitIndexerExpectations<T, TContainingType>
		: ExpectationsWrapper<T>
		where T : class, TContainingType
	{
		public ExplicitIndexerExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}