using NUnit.Framework;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
	public interface IGenerics<T>
	{
		void Foo(List<string> values);
		//void Bar<TParam>(TParam value);
	}

	public static class GenericMemberTests
	{
		[Test]
		public static void MockMethodUsingGenericType()
		{
			var rock = Rock.Create<IGenerics<int>>();
			rock.Methods().Foo(Arg.Any<List<string>>());

			var chunk = rock.Instance();
			chunk.Foo(new List<string>());

			rock.Verify();
		}

		//[Test]
		//public static void MockMethodWithGenericParameterType()
		//{
		//	var rock = Rock.Create<IGenerics<int>>();
		//	rock.Methods().Bar(Arg.Any<List<string>>());

		//	var chunk = rock.Instance();
		//	chunk.Foo(new List<string>());

		//	rock.Verify();
		//}
	}
}