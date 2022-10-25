namespace Rocks.Expectations;

public sealed class PropertyIniterExpectations<T>
	: PropertyExpectations<T>
	where T : class
{
	public PropertyIniterExpectations(PropertyExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}