namespace Rocks.Extensions
{
	internal sealed class GenericArgumentsResult
	{
		internal GenericArgumentsResult(string arguments, string constraints)
		{
			this.Arguments = arguments;
			this.Constraints = constraints;
		}

		internal string Arguments { get; private set; }
		internal string Constraints { get; private set; }
	}
}
