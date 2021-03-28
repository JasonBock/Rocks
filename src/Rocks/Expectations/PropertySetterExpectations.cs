namespace Rocks.Expectations
{
	public sealed class PropertySetterExpectations<T>
		: PropertyExpectations<T>
		where T : class
	{
		public PropertySetterExpectations(Expectations<T> expectations)
			: base(expectations) { }
	}
}