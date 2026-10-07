namespace Stuff;

public abstract class Holder
{
	protected Holder() { }

	protected abstract event EventHandler<DataEventArgs> Test;

	protected internal class DataEventArgs
		: EventArgs
	{ }
}

public class DerivingHolder
	: Holder
{
	protected override sealed event EventHandler<DataEventArgs>? Test { add { } remove { } }
}
