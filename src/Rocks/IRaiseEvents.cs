namespace Rocks;

public interface IRaiseEvents
{
	void Raise(string eventName, EventArgs args);
}