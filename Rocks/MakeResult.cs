namespace Rocks
{
	public sealed class MakeResult<T>
		where T : class
	{
		internal MakeResult(bool isSuccessful, T result)
		{
			this.IsSuccessful = isSuccessful;
			this.Result = result;
		}

		public bool IsSuccessful { get; }
		public T Result { get; }
	}
}
