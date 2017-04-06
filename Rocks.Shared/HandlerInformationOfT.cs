using System;
using System.Collections.ObjectModel;

namespace Rocks
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public sealed class HandlerInformation<T>
		: HandlerInformation
	{
		internal HandlerInformation(ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(null, 1, expectations)
		{
			this.ReturnValue = default(T);
		}

		internal HandlerInformation(Delegate method, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(method, 1, expectations)
		{
			this.ReturnValue = default(T);
		}

		internal HandlerInformation(uint expectedCallCount, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(null, expectedCallCount, expectations)
		{
			this.ReturnValue = default(T);
		}

		internal HandlerInformation(Delegate method, uint expectedCallCount, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(method, expectedCallCount, expectations)
		{
			this.ReturnValue = default(T);
		}

		public T ReturnValue{ get; internal set; }
	}
}
