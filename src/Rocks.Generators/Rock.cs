namespace Rocks
{
	public static class Rock
	{
		public static Expectations<T> Create<T>()
			where T : class => new Expectations<T>();
	}
}