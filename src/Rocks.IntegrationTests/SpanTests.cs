using System;

namespace Rocks.IntegrationTests
{
	public interface IHaveSpans
	{
		void Foo(Span<int> data);
	}

	public static class SpanTests
	{

	}
}