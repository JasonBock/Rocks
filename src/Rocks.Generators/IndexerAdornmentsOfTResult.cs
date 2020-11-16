namespace Rocks
{
   public sealed class IndexerAdornments<T, TResult>
		: IndexerAdornments<T>
		where T : class
   {
		public IndexerAdornments(HandlerInformation<TResult> handler)
			: base(handler)
		{ }

		public IndexerAdornments<T, TResult> Returns(TResult returnValue)
		{
			((HandlerInformation<TResult>)this.Handler).ReturnValue = returnValue;
			return this;
		}
   }
}