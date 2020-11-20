namespace Rocks
{
	public sealed class PropertyGetterExpectations<T>
		: ExpectationsWrapper<T>
		where T : class
	{
		public PropertyGetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}