namespace Rocks.Expectations
{
	public sealed class ExplicitMethodExpectations<T, TContainingType>
		: ExpectationsWrapper<T>
		where T : class, TContainingType
	{
		public ExplicitMethodExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}