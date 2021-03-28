namespace Rocks.Expectations
{
	public class PropertyExpectations<T>
		: ExpectationsWrapper<T>
		where T : class
	{
		public PropertyExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}