using System.Collections.ObjectModel;

namespace Rocks
{
	public interface IMock
	{
		ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> Handlers { get; }
	}
}
