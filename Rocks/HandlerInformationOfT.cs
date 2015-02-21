using System;

namespace Rocks
{
	public sealed class HandlerInformation<T>
		: HandlerInformation
	{
		internal HandlerInformation()
			: base(null, 1)
		{
			this.ReturnValue = default(T);
		}

		internal HandlerInformation(Delegate method)
			: base(method, 1)
		{
			this.ReturnValue = default(T);
		}

		internal HandlerInformation(uint expectedCallCount)
			: base(null, expectedCallCount)
		{
			this.ReturnValue = default(T);
		}

		internal HandlerInformation(Delegate method, uint expectedCallCount)
			: base(method, expectedCallCount)
		{
			this.ReturnValue = default(T);
		}

		internal T ReturnValue{ get; set; }
	}
}
