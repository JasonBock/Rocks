using NUnit.Framework;

namespace Rocks.Tests
{
	public static class NamespaceTests
	{
		[Test]
		public static void CreateWhenTypeHasNoNamespace()
		{
			var rock = Rock.Create<IHaveNoNamespace>();
			rock.Handle(_ => _.Foo());

			var chunk = rock.Make();
			chunk.Foo();

			rock.Verify();
		}
	}
}

public interface IHaveNoNamespace
{
	void Foo();
}