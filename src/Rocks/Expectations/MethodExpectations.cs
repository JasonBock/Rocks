namespace Rocks.Expectations;

public sealed class MethodExpectations<T>
	: ExpectationsWrapper<T>
	where T : class
{
	public MethodExpectations(Expectations<T> expectations)
		: base(expectations) { }
}