using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IHaveParams
	{
		void Foo(int a, params string[] b);
		int this[int a, params string[] b] { get; }
	}

	public static class ParamsTests
	{
		[Test]
		public static void MockMembersWithParamsArgumentsSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveParams>();
			rock.Methods().Foo(1, new[] { "b" });
			rock.Indexers().Getters().This(1, new[] { "b" }).Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1, "b");
			var value = chunk[1, "b"];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockMembersWithParamsArgumentsNotSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveParams>();
			rock.Methods().Foo(1, Array.Empty<string>());
			rock.Indexers().Getters().This(1, Array.Empty<string>()).Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1);
			var value = chunk[1];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}
	}
}