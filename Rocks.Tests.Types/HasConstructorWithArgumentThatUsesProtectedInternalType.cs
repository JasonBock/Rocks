namespace Rocks.Tests.Types
{
	public abstract class HasConstructorWithArgumentThatUsesProtectedInternalType
	{
		protected HasConstructorWithArgumentThatUsesProtectedInternalType(ProtectedInternalType a) { }

		protected internal class ProtectedInternalType { }
	}
}
