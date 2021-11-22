namespace Rocks.Expectations;

public sealed class PropertyInitExpectations<T>
	: PropertyExpectations<T>
	where T : class
{
	public PropertyInitExpectations(Expectations<T> expectations)
		: base(expectations) { }
}