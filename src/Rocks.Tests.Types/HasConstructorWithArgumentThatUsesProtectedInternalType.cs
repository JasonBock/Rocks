namespace Rocks.Tests.Types
{
	public abstract class HasConstructorWithArgumentThatUsesProtectedInternalType
	{
#pragma warning disable CA1801 // Review unused parameters
		protected HasConstructorWithArgumentThatUsesProtectedInternalType(ProtectedInternalType a) { }
#pragma warning restore CA1801 // Review unused parameters

		protected internal class ProtectedInternalType { }
	}
}
