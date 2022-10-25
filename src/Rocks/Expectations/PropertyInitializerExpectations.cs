namespace Rocks.Expectations;

public sealed class PropertyInitializerExpectations<T>
	: PropertyExpectations<T>
	where T : class
{
	public PropertyInitializerExpectations(PropertyExpectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}