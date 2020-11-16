using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class SpanLikeTests
	{
		[Test]
		public static void MockWhenTypeIsPartiallySpanLike()
		{
			var rock = Rock.Create<IPartiallySpanLike>();
			rock.Handle(_ => _.Foo());

			var chunk = rock.Make();
			chunk.Foo();

			rock.Verify();
		}
	}

	public interface IPartiallySpanLike
	{
		void Foo();
		void SpanFoo(Span<char> value);
		void ReadOnlySpanFoo(ReadOnlySpan<char> value);
		Span<T> ReturnSpanFoo<T>();
		ReadOnlySpan<T> ReturnReadOnlySpanFoo<T>();
	}
}
