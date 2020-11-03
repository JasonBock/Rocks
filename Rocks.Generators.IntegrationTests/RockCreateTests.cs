using NUnit.Framework;

namespace Rocks.IntegrationTests
{
	public static class RockCreateTests
	{
		[Test]
		public static void CreateHappyPath()
		{
			var rock = Rock.Create<IFoo>();
			rock.Methods().Foo(Arg.Is(3), Arg.Is("b"));

			var chunk = rock.Instance();
			chunk.Foo(3, "b");

			rock.Verify();
		}
	}

	public interface IFoo
	{
		void Foo(int a, string b);
	}
}