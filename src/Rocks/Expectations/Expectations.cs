using Rocks.Exceptions;
using Rocks.Extensions;
using System.Collections.Immutable;
using System.ComponentModel;

namespace Rocks.Expectations;

/// <summary>
/// The base class for all expectation types.
/// </summary>
/// <typeparam name="T">The type to mock.</typeparam>
#pragma warning disable CA1724
public class Expectations<T>
	: IExpectations
	where T : class
#pragma warning restore CA1724
{
	internal Expectations() =>
		this.Handlers = [];

	internal Expectations(Expectations<T> expectations) =>
		this.Handlers = expectations.Handlers;

	/// <summary>
	/// Adds argument expectations for a given member.
	/// </summary>
	/// <param name="memberIdentifier">The identifier of the member.</param>
	/// <param name="arguments">The argument expectataions.</param>
	/// <returns>A <see cref="HandlerInformation"/> instance to set other expectations and behaviors on the member.</returns>
	public HandlerInformation Add(int memberIdentifier, List<Argument> arguments)
	{
		var information = new HandlerInformation(arguments.ToImmutableArray());
		this.Handlers.AddOrUpdate(memberIdentifier,
			() => new List<HandlerInformation>(1) { information }, _ => _.Add(information));
		return information;
	}

	/// <summary>
	/// Adds argument expectations for a given member that returns a value.
	/// </summary>
	/// <param name="memberIdentifier">The identifier of the member.</param>
	/// <param name="arguments">The argument expectataions.</param>
	/// <returns>A <see cref="HandlerInformation{TReturn}"/> instance to set other expectations and behaviors on the member.</returns>
	public HandlerInformation<TReturn> Add<TReturn>(int memberIdentifier, List<Argument> arguments)
	{
		var information = new HandlerInformation<TReturn>(arguments.ToImmutableArray());
		this.Handlers.AddOrUpdate(memberIdentifier,
			() => new List<HandlerInformation>(1) { information }, _ => _.Add(information));
		return information;
	}

	/// <summary>
	/// Verifies the expectations set on this instance.
	/// </summary>
	/// <exception cref="VerificationException">Thrown if any expectations were not met.</exception>
	public void Verify()
	{
		if(this.WasInstanceInvoked)
		{
			var failures = new List<string>();

			foreach (var pair in this.Handlers)
			{
				foreach (var handler in pair.Value)
				{
					foreach (var failure in handler.Verify())
					{
						var member = this.MockType!.GetMemberDescription(pair.Key);

						failures.Add(
							$"Type: {typeof(T).FullName}, mock type: {this.MockType!.FullName}, member: {member}, message: {failure}");
					}
				}
			}

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}
	}

	/// <summary>
	/// This property is used by Rocks and is not intented to be used by developers.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	public Dictionary<int, List<HandlerInformation>> Handlers { get; }

	/// <summary>
	/// This property is used by Rocks and is not intented to be used by developers.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	public Type? MockType { get; set; }

	/// <summary>
	/// This property is used by Rocks and is not intented to be used by developers.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	public bool WasInstanceInvoked { get; set; }
}