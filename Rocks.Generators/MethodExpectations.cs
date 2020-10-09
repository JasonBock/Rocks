namespace Rocks
{
	internal sealed class MethodExpectations<T> 
		: Expectations<T>
		where T : class
	{
		private readonly Expectations<T> expectations;

		internal MethodExpectations(Expectations<T> expectations) =>
			this.expectations = expectations;
	}
}