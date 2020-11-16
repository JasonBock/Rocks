namespace Rocks
{
	public static class Rock
	{
		// TODO: Need to eventually add serialization option back in.
		public static Expectations<T> Create<T>()
			where T : class => new Expectations<T>();
	}
}