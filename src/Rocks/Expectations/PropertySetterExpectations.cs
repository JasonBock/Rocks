namespace Rocks.Expectations;

public sealed class PropertySetterExpectations<T>
	: PropertyExpectations<T>
	where T : class
{
	public PropertySetterExpectations(PropertyExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}