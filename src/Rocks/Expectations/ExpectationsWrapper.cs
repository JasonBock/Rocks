using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks.Expectations
{
	public abstract class ExpectationsWrapper<T>
		where T : class
	{
		protected ExpectationsWrapper(Expectations<T> expectations) =>
			this.Expectations = expectations;

		public HandlerInformation Add(int memberIdentifier, List<Argument> arguments)
		{
			var information = new HandlerInformation(arguments.ToImmutableArray());
			this.Expectations.Handlers.AddOrUpdate(memberIdentifier,
				() => new List<HandlerInformation>(1) { information }, _ => _.Add(information));
			return information;
		}

		public HandlerInformation<TReturn> Add<TReturn>(int memberIdentifier, List<Argument> arguments)
		{
			var information = new HandlerInformation<TReturn>(arguments.ToImmutableArray());
			this.Expectations.Handlers.AddOrUpdate(memberIdentifier,
				() => new List<HandlerInformation>(1) { information }, _ => _.Add(information));
			return information;
		}

		public Expectations<T> Expectations { get; }
	}
}