namespace Rocks;

public interface IMock
{
	Dictionary<int, List<HandlerInformation>> Handlers { get; }
}