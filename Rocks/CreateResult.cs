namespace Rocks
{
	public sealed class CreateResult<T>
		where T : class
	{
		internal CreateResult(bool isSuccessful, Rock<T> result)
		{
			this.IsSuccessful = IsSuccessful;
			this.Result = result;
		}

		public bool IsSuccessful { get; private set; }
		public Rock<T> Result { get; private set; }
	}
}
