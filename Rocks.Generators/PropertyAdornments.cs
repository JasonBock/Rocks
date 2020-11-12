namespace Rocks
{
	public class PropertyAdornments<T>
		where T : class
	{
		public PropertyAdornments(HandlerInformation handler) => 
			this.Handler = handler;

		public PropertyAdornments<T> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public HandlerInformation Handler { get; }
	}
}