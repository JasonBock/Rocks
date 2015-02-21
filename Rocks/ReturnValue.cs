namespace Rocks
{
	public sealed class ReturnValue<TResult>
	{
		private HandlerInformation<TResult> handler;

		internal ReturnValue(HandlerInformation<TResult> handler)
		{
			this.handler = handler;
		}

		public void Returns(TResult returnValue)
		{
			this.handler.ReturnValue = returnValue;
		}
	}
}
