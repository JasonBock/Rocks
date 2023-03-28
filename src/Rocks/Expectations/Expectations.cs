﻿using Rocks.Exceptions;
using Rocks.Extensions;
using System.Collections.Immutable;
using System.ComponentModel;

namespace Rocks.Expectations;

#pragma warning disable CA1724
public class Expectations<T>
	: IExpectations
	where T : class
#pragma warning restore CA1724
{
	internal Expectations() =>
		this.Handlers = new();

	internal Expectations(Expectations<T> expectations) =>
		this.Handlers = expectations.Handlers;

	public HandlerInformation Add(int memberIdentifier, List<Argument> arguments)
	{
		var information = new HandlerInformation(arguments.ToImmutableArray());
		this.Handlers.AddOrUpdate(memberIdentifier,
			() => new List<HandlerInformation>(1) { information }, _ => _.Add(information));
		return information;
	}

	public HandlerInformation<TReturn> Add<TReturn>(int memberIdentifier, List<Argument> arguments)
	{
		var information = new HandlerInformation<TReturn>(arguments.ToImmutableArray());
		this.Handlers.AddOrUpdate(memberIdentifier,
			() => new List<HandlerInformation>(1) { information }, _ => _.Add(information));
		return information;
	}

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

	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	/// <summary>
	/// This property is used by Rocks and is not intented to be used by developers.
	/// </summary>
	public Dictionary<int, List<HandlerInformation>> Handlers { get; }

	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	/// <summary>
	/// This property is used by Rocks and is not intented to be used by developers.
	/// </summary>
	public Type? MockType { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	/// <summary>
	/// This property is used by Rocks and is not intented to be used by developers.
	/// </summary>
	public bool WasInstanceInvoked { get; set; }
}