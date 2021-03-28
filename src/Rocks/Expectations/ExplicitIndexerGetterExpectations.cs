namespace Rocks.Expectations
{
	public sealed class ExplicitIndexerGetterExpectations<T, TContainingType>
		: ExpectationsWrapper<T>
		where T : class, TContainingType
	{
		public ExplicitIndexerGetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}