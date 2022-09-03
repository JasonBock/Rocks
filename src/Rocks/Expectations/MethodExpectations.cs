namespace Rocks.Expectations;

public sealed class MethodExpectations<T>
	: Expectations<T>
	where T : class
{
	public MethodExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}