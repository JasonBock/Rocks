namespace Rocks.Comparisons;

public class HasInitAndRequired
{
	public int Calculate() =>
		(this.InitValue + this.RequiredValue) / this.ProvideDenominator();

	public virtual int ProvideDenominator() => 1;

	public int InitValue { get; init; }
	public required int RequiredValue { get; init; }
}