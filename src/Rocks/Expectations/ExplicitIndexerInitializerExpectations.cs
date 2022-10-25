namespace Rocks.Expectations;

public sealed class ExplicitIndexerInitializerExpectations<T, TContainingType>
	: ExplicitIndexerExpectations<T, TContainingType>
	where T : class, TContainingType
{
	public ExplicitIndexerInitializerExpectations(ExplicitIndexerExpectations<T, TContainingType> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}