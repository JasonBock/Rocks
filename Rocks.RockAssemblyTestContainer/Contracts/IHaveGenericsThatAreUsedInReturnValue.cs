namespace Rocks.RockAssemblyTestContainer.Contracts
{
	public interface IHaveGenericsThatAreUsedInReturnValue<T>
	{
		T this[int a] { get; }
	}
}
