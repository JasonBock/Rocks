using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks
{
	public sealed class MethodExpectations<T> 
		where T : class
	{
		private readonly Expectations<T> expectations;

		public MethodExpectations(Expectations<T> expectations) =>
			this.expectations = expectations;

		public HandlerInformation Add(int memberIdentifier, List<Arg> arguments)
		{
			var information = new HandlerInformation(arguments.ToImmutableArray());
			this.expectations.Handlers.AddOrUpdate(memberIdentifier,
				() => new List<HandlerInformation> { information }, _ => _.Add(information));
			return information;
		}
	}
}