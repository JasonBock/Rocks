namespace Rocks.Comparisons;

public class AccessModifiers
{
	internal virtual int InternalWork() =>
		this.ProtectedWork();
	protected virtual int ProtectedWork() => 1;
}