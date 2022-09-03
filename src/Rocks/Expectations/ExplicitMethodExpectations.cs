namespace Rocks.Expectations;

public sealed class ExplicitMethodExpectations<T, TContainingType>
	: Expectations<T>
	where T : class, TContainingType
{
	public ExplicitMethodExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}