using System;
using System.Collections.Generic;

namespace Rocks
{
	internal sealed class AssemblyRock<T>
		: RockCore<T>
		where T : class
	{
		internal AssemblyRock() { }

		public override T Make()
		{
			var readOnlyHandlers = this.CreateReadOnlyHandlerDictionary();

			var rock = Activator.CreateInstance(typeof(T), readOnlyHandlers);
			this.Rocks.Add(rock as IMock);
			return rock as T;
		}

		public override T Make(object[] constructorArguments)
		{
			var readOnlyHandlers = this.CreateReadOnlyHandlerDictionary();

			var arguments = new List<object> { readOnlyHandlers };
			arguments.AddRange(constructorArguments);

			var rock = Activator.CreateInstance(typeof(T), arguments.ToArray(), null);
			this.Rocks.Add(rock as IMock);
			return rock as T;
		}
	}
}
