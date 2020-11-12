namespace Rocks
{
   public sealed class PropertyAdornments<T, TResult>
		: PropertyAdornments<T>
		where T : class
   {
		public PropertyAdornments(HandlerInformation<TResult> handler)
			: base(handler)
		{ }

		public PropertyAdornments<T, TResult> Returns(TResult returnValue)
		{
			((HandlerInformation<TResult>)this.Handler).ReturnValue = returnValue;
			return this;
		}
   }
}