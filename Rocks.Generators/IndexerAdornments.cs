namespace Rocks
{
	public class IndexerAdornments<T>
		where T : class
	{
		public IndexerAdornments(HandlerInformation handler) => 
			this.Handler = handler;

		public IndexerAdornments<T> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public HandlerInformation Handler { get; }
	}
}