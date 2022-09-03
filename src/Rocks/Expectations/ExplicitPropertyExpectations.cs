namespace Rocks.Expectations;

public class ExplicitPropertyExpectations<T, TContainingType>
	: Expectations<T>
	where T : class, TContainingType
{
	public ExplicitPropertyExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}