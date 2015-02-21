using System.Collections.ObjectModel;

namespace Rocks
{
	internal interface IRock
	{
		ReadOnlyDictionary<string, HandlerInformation> Handlers { get; }
	}
}
