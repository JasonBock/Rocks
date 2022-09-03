namespace Rocks.Expectations;

public class ExplicitIndexerExpectations<T, TContainingType>
	: Expectations<T>
	where T : class, TContainingType
{
	public ExplicitIndexerExpectations(Expectations<T> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}