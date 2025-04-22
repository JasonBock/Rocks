using Rocks.Runtime;

namespace Rocks.Performance;

public sealed class HandlerInt
	: Handler<Func<int, int>, int>
{
#pragma warning disable CS8618
	public Argument<int> input { get; set; }
#pragma warning restore CS8618
}