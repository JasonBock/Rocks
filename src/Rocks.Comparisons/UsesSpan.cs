namespace Rocks.Comparisons;

public sealed class UsesSpan
{
	public UsesSpan(IHaveSpan haveSpan) =>
		this.HaveSpan = haveSpan;

	public int Execute(Span<int> values) =>
		this.HaveSpan.Process(values);

   private IHaveSpan HaveSpan { get; }
}