namespace Rocks.Expectations;

public sealed class ExplicitIndexerIniterExpectations<T, TContainingType>
	: ExplicitIndexerExpectations<T, TContainingType>
	where T : class, TContainingType
{
	public ExplicitIndexerIniterExpectations(ExplicitIndexerExpectations<T, TContainingType> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}