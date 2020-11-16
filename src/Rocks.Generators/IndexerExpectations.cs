using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks
{
	public sealed class IndexerExpectations<T> 
		where T : class
	{
		private readonly Expectations<T> expectations;

		public IndexerExpectations(Expectations<T> expectations) =>
			this.expectations = expectations;

		// TODO: Same Add() methods exist in MethodExpectations. Potentially share this.
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