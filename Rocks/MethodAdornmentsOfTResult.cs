using System;

namespace Rocks
{
	public sealed class MethodAdornments<TResult>
		: MethodAdornments
	{
		internal MethodAdornments(HandlerInformation<TResult> handler)
			: base(handler)
		{ }

		public MethodAdornments<TResult> Returns(TResult returnValue)
		{
			(this.handler as HandlerInformation<TResult>).ReturnValue = returnValue;
			return this;
		}

		public MethodAdornments<TResult> Returns(Func<TResult> returnValue)
		{
			(this.handler as HandlerInformation<TResult>).ReturnValue = returnValue();
			return this;
		}
	}
}
