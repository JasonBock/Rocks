namespace Rocks.PackageTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var rock = Rock.Create<ITest>();
			rock.Handle(_ => _.Foo());

			var chunk = rock.Make();
			chunk.Foo();

			rock.Verify();
		}
	}

	public interface ITest
	{
		void Foo();
	}
}
