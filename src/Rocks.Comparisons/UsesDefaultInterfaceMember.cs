namespace Rocks.Comparisons;

public sealed class UsesDefaultInterfaceMember
{
	public UsesDefaultInterfaceMember(IHaveDefaultInterfaceMember haveDefaultInterfaceMember) =>
		this.HaveDefaultInterfaceMember = haveDefaultInterfaceMember;

	public int Execute() =>
		this.HaveDefaultInterfaceMember.DefaultWork() +
		this.HaveDefaultInterfaceMember.Work();
 
	private IHaveDefaultInterfaceMember HaveDefaultInterfaceMember { get; }
}