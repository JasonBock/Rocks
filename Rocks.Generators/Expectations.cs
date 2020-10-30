using Rocks.Exceptions;
using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks
{
	public class Expectations<T> where T : class
	{
		public void Verify()
		{
			var failures = new List<string>();

			foreach (var rock in this.Mocks)
			{
				failures.AddRange(rock.GetVerificationFailures());
			}

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}

		internal ImmutableDictionary<int, ImmutableArray<HandlerInformation>> CreateHandlers() =>
			this.Handlers.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value.ToImmutableArray());

		internal Dictionary<int, List<HandlerInformation>> Handlers { get; } = new();
		internal List<IMock> Mocks { get; } = new();
	}
}