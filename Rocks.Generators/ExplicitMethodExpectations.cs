using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks
{
	public sealed class ExplicitMethodExpectations<TBase, T>
		where T : class, TBase
		where TBase : class
	{
		private readonly Expectations<TBase> expectations;

		public ExplicitMethodExpectations(Expectations<TBase> expectations) =>
			this.expectations = expectations;

		public HandlerInformation Add(int memberIdentifier, List<Arg> arguments)
		{
			var information = new HandlerInformation(arguments.ToImmutableArray());
			this.expectations.Handlers.AddOrUpdate(memberIdentifier,
				() => new List<HandlerInformation> { information }, _ => _.Add(information));
			return information;
		}

		public HandlerInformation<TReturn> Add<TReturn>(int memberIdentifier, List<Arg> arguments)
		{
			var information = new HandlerInformation<TReturn>(arguments.ToImmutableArray());
			this.expectations.Handlers.AddOrUpdate(memberIdentifier,
				() => new List<HandlerInformation> { information }, _ => _.Add(information));
			return information;
		}
	}
}