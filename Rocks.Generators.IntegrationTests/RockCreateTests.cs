using NUnit.Framework;

namespace Rocks.IntegrationTests
{
	public static class RockCreateTests
	{
		[Test]
		public static void CreateHappyPathForValue()
		{
			var rock = Rock.Create<IFoo>();
			rock.Methods().Foo(Arg.Is(3), Arg.Is("b"));
			rock.Methods().Bar(Arg.Is(3), Arg.Is("b"));
			rock.Methods().Baz();

			var chunk = rock.Instance();
			chunk.Foo(3, "b");
			chunk.Bar(3, "b");
			chunk.Baz();

			rock.Verify();
		}
	}

	public interface IFoo
	{
		string Bar(int a, string b);
		void Foo(int a, string b);
		void Baz();
	}
}