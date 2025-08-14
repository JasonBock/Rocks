namespace Rocks.Comparisons;

public sealed class UsesAccessModifiers
{
	public UsesAccessModifiers(AccessModifiers haveAccessModifiers) =>
		this.HaveAccessModifiers = haveAccessModifiers;

	public int Execute() => this.HaveAccessModifiers.InternalWork();

   private AccessModifiers HaveAccessModifiers { get; }
}