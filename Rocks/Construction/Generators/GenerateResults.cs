namespace Rocks.Construction.Generators
{
	internal sealed class GenerateResults
	{
		internal GenerateResults(string result, bool requiresObsoleteSuppression, bool isUnsafe) =>
			(this.Result, this.RequiresObsoleteSuppression, this.IsUnsafe) =
				(result, requiresObsoleteSuppression, isUnsafe);

		internal bool IsUnsafe { get; }
		internal string Result { get; }
		internal bool RequiresObsoleteSuppression { get; }
	}
}