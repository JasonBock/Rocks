namespace Rocks.Expectations;

public sealed class ExplicitPropertyInitializerExpectations<T, TContainingType>
	: ExplicitPropertyExpectations<T, TContainingType>
	where T : class, TContainingType
{
	public ExplicitPropertyInitializerExpectations(ExplicitPropertyExpectations<T, TContainingType> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}