namespace Rocks
{
	public sealed class ExplicitPropertyExpectations<T, TContainingType>
		: ExpectationsWrapper<T>
		where T : class, TContainingType
	{
		public ExplicitPropertyExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}