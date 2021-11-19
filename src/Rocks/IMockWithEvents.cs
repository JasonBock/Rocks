namespace Rocks;

public interface IMockWithEvents
	: IMock
{
	void Raise(string eventName, EventArgs args);
}