namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var rockOne = Rock.Create<IDoTwoThings>();
			rockOne.Handle(_ => _.MethodOne(1));

			var rockTwo = Rock.Create<IDoTwoThings>();
			rockTwo.Handle(_ => _.MethodTwo(2));

			var chunkOne = rockOne.Make();
			var chunkTwo = rockTwo.Make();

			chunkOne.MethodOne(1);
			chunkTwo.MethodTwo(2);

			rockOne.Verify();
			rockTwo.Verify();
		}
	}

	public interface IDoTwoThings
	{
		void MethodOne(int x);
		void MethodTwo(int x);
	}
}
