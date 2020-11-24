namespace Rocks
{
	public sealed class ExplicitPropertyGetterExpectations<T, TContainingType>
		: ExpectationsWrapper<T>
		where T : class, TContainingType
	{
		public ExplicitPropertyGetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}