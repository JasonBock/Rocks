using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks
{
	public abstract class ExpectationsWrapper<T>
		where T : class
	{
		private readonly Expectations<T> expectations;

		protected ExpectationsWrapper(Expectations<T> expectations) =>
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