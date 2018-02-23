using System;
using System.Collections.ObjectModel;

namespace Rocks
{
	[Serializable]
	public sealed class HandlerInformation<T>
		: HandlerInformation
	{
		internal HandlerInformation(ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(null, 1, expectations) => this.ReturnValue = default;

		internal HandlerInformation(Delegate method, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(method, 1, expectations) => this.ReturnValue = default;

		internal HandlerInformation(uint expectedCallCount, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(null, expectedCallCount, expectations) => this.ReturnValue = default;

		internal HandlerInformation(Delegate method, uint expectedCallCount, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: base(method, expectedCallCount, expectations) => this.ReturnValue = default;

		public T ReturnValue { get; internal set; }
	}
}
