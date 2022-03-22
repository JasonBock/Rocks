using Rocks.Exceptions;
using Rocks.Extensions;
using System.ComponentModel;

namespace Rocks.Expectations;

// TODO: I REALLY do not want to suppress this,
// but it's so ingrained into things. Eventually
// I'll address it...once I can think of a suitable name replacement.
#pragma warning disable CA1724
public class Expectations<T>
	: IExpectations
	where T : class
#pragma warning restore CA1724
{
	internal Expectations() { }

	internal Expectations(Dictionary<int, List<HandlerInformation>> handlers, IMock? mock) =>
		(this.Handlers, this.Mock) = (handlers, mock);

	public void Verify()
	{
		var failures = new List<string>();
		failures.AddRange(this.Mock?.GetVerificationFailures());

		if (failures.Count > 0)
		{
			throw new VerificationException(failures);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	/// <summary>
	/// This property is used by Rocks and is not intented to be used by developers.
	/// </summary>
	public Dictionary<int, List<HandlerInformation>> Handlers { get; } = new();

	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	/// <summary>
	/// This method is used by Rocks and is not intented to be used by developers.
	/// </summary>
	public IMock? Mock { get; set; }
}