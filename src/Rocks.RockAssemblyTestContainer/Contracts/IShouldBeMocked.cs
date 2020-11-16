using System;
using System.Collections.Generic;

namespace Rocks.RockAssemblyTestContainer.Contracts
{
	public interface IShouldBeMocked<T>
	{
		void Method1();
		string Method2(T a);
		Guid Method3(string a, int b);
		IEnumerable<T> Values { get; }
	}
}
