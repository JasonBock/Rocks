using NUnit.Framework;

namespace Rocks.Tests
{
	public interface IA
	{
		int Foo(int a);
	}

	public interface IB
	{
		int Foo(int a);
	}

	public interface IC
		: IA, IB
	{ }

	public class C
		: IC
	{
		public int Foo(int a) => a * 2;
	}

	public static class HelpUrlBuilderTests
	{
		[Test]
		public static void Create() =>
			Assert.Multiple(() =>
			{
				Assert.That(HelpUrlBuilder.Build("a", "b"),
					Is.EqualTo("https://github.com/JasonBock/Rocks/tree/master/Rocks.Documentation/a-b.md"));
			});

		[Test]
		public static void TestFoo()
		{
			var c = new C();
			c.Foo(2);
		}
	}
}