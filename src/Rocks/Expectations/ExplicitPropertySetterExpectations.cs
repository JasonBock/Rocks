namespace Rocks.Expectations
{
	public sealed class ExplicitPropertySetterExpectations<T, TContainingType>
		: ExpectationsWrapper<T>
		where T : class, TContainingType
	{
		public ExplicitPropertySetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}