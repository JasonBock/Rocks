namespace Rocks
{
   public sealed class MethodAdornments<T, TResult>
	   : MethodAdornments<T>
		where T : class
   {
		public MethodAdornments(HandlerInformation<TResult> handler)
			: base(handler)
		{ }

		public MethodAdornments<T, TResult> Returns(TResult returnValue)
		{
			((HandlerInformation<TResult>)this.Handler).ReturnValue = returnValue;
			return this;
		}
   }
}