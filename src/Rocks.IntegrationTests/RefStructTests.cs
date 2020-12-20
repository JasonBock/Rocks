using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IHaveSpan
	{
		void Foo(Span<int> values);
		void Bar<T>(Span<T> values);
	}

	public static class RefStructTests
	{
		[Test]
		public static void CreateWithSpanOfInt()
		{
			var rock = Rock.Create<IHaveSpan>();
			rock.Methods().Foo(new());

			var chunk = rock.Instance();
			var buffer = new int[] { 3 };

			chunk.Foo(new Span<int>(buffer));

			rock.Verify();
		}

		[Test]
		public static void CreateWithSpanOfIntAndValidation()
		{
			static bool FooEvaluation(Span<int> value) => 
				value.Length == 1 && value[0] == 3;
			
			var rock = Rock.Create<IHaveSpan>();
			rock.Methods().Foo(new(FooEvaluation));

			var chunk = rock.Instance();
			var buffer = new int[] { 3 };

			chunk.Foo(new Span<int>(buffer));

			rock.Verify();
		}

		[Test]
		public static void CreateWithSpanOfT()
		{
			var rock = Rock.Create<IHaveSpan>();
			rock.Methods().Bar<int>(new());

			var chunk = rock.Instance();
			var buffer = new int[] { 3 };

			chunk.Bar(new Span<int>(buffer));

			rock.Verify();
		}

		[Test]
		public static void CreateWithSpanOfTAndValidation()
		{
			static bool BarEvaluation(Span<int> value) =>
				value.Length == 1 && value[0] == 3;

			var rock = Rock.Create<IHaveSpan>();
			rock.Methods().Bar<int>(new(BarEvaluation));

			var chunk = rock.Instance();
			var buffer = new int[] { 3 };

			chunk.Bar(new Span<int>(buffer));

			rock.Verify();
		}
	}
}