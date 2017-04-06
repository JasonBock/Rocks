namespace Rocks
{
	public partial interface IRock<T> 
		where T : class
	{
		T Make();
		T Make(object[] constructorArguments);
		void Verify();
	}
}