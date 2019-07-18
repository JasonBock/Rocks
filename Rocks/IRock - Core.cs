namespace Rocks
{
	public interface IRock
	{
		void Verify();
	}

	public partial interface IRock<T> 
		: IRock
		where T : class
	{
		T Make();
		T Make(object[] constructorArguments);
	}
}