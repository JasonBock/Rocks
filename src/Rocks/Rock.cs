namespace Rocks
{
	public static class Rock
	{
		public static Expectations<T> Create<T>()
			where T : class => new Expectations<T>();

		public static MakeGeneration<T> Make<T>()
			where T : class => new MakeGeneration<T>();
	}
}