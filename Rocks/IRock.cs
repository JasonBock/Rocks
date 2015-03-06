using System.Collections.ObjectModel;

namespace Rocks
{
	public interface IRock
	{
		ReadOnlyDictionary<string, HandlerInformation> Handlers { get; }
	}
}
