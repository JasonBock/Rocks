namespace Rocks.Expectations;

public sealed class ExplicitPropertyIniterExpectations<T, TContainingType>
	: ExplicitPropertyExpectations<T, TContainingType>
	where T : class, TContainingType
{
	public ExplicitPropertyIniterExpectations(ExplicitPropertyExpectations<T, TContainingType> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}