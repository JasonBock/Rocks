using Rocks.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Rocks.Extensions.IMockExtensions;

namespace Rocks
{
	internal abstract partial class RockCore<T>
		: IRock<T>
		where T : class
	{
		protected RockCore() { }

		protected ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> CreateReadOnlyHandlerDictionary() =>
			new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
				this.Handlers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly()));

		public abstract T Make();

		public abstract T Make(object[] constructorArguments);

		public void Verify()
		{
			var failures = new List<string>();

			foreach (var rock in this.Rocks)
			{
				failures.AddRange(rock.GetVerificationFailures());
			}

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}

		protected Dictionary<int, List<HandlerInformation>> Handlers { get; } = new Dictionary<int, List<HandlerInformation>>();
		protected SortedSet<string> Namespaces { get; } = new SortedSet<string>();
		protected List<IMock> Rocks { get; } = new List<IMock>();
	}
}
