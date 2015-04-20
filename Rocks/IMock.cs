using System.Collections.ObjectModel;

namespace Rocks
{
	public interface IMock
	{
		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> Handlers { get; }
	}
}
