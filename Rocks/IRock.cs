using System.Collections.ObjectModel;

namespace Rocks
{
	public interface IRock
	{
		ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> Handlers { get; }
	}
}
