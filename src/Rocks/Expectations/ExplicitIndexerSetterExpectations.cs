namespace Rocks.Expectations
{
	public sealed class ExplicitIndexerSetterExpectations<T, TContainingType>
		: ExpectationsWrapper<T>
		where T : class, TContainingType
	{
		public ExplicitIndexerSetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}