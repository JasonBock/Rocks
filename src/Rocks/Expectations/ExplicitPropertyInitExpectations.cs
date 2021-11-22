namespace Rocks.Expectations;

public sealed class ExplicitPropertyInitExpectations<T, TContainingType>
	: ExpectationsWrapper<T>
	where T : class, TContainingType
{
	public ExplicitPropertyInitExpectations(Expectations<T> expectations)
		: base(expectations) { }
}