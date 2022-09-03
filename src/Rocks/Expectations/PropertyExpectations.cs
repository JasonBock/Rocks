namespace Rocks.Expectations;

public class PropertyExpectations<T>
	: Expectations<T>
	where T : class
{
   public PropertyExpectations(Expectations<T> expectations)
	   : base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}