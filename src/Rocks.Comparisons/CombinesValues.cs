namespace Rocks.Comparisons;

public class CombinesValues
{
	public CombinesValues(Guid identifier, int number) =>
		(this.Identifier, this.Number) = (identifier, number);

	public string Combine() =>
		$"{this.Identifier} | {this.Number} | {this.Compute()}";

	public virtual string Compute() => "string";

	public Guid Identifier { get; }
	public int Number { get; }
}