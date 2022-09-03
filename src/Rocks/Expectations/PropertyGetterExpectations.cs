namespace Rocks.Expectations;

public sealed class PropertyGetterExpectations<T>
	: PropertyExpectations<T>
	where T : class
{
	public PropertyGetterExpectations(PropertyExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}