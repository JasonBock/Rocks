namespace Rocks.Tests.Targets
{
	public interface IContainNullableReferences
	{
		void DoSomething(string data, object?[]? values);
	}
}