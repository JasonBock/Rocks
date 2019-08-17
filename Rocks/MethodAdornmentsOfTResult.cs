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
			((HandlerInformation<TResult>)this.Handler).ReturnValue = returnValue;
			return this;
		}
   }
}