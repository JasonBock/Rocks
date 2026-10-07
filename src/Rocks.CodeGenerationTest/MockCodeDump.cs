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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	protected override sealed event EventHandler<DataEventArgs> Test;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
