using System.Collections.Immutable;

namespace Rocks
{
	public interface IMock
	{
		ImmutableDictionary<int, ImmutableArray<HandlerInformation>> Handlers { get; }
	}
}