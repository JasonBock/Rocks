namespace Rocks.Expectations;

public sealed class ExplicitPropertySetterExpectations<T, TContainingType>
	: ExplicitPropertyExpectations<T, TContainingType>
	where T : class, TContainingType
{
	public ExplicitPropertySetterExpectations(ExplicitPropertyExpectations<T, TContainingType> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}