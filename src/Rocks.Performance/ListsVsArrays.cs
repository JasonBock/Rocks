using BenchmarkDotNet.Attributes;

namespace Rocks.Performance;

#pragma warning disable CS8618
[MemoryDiagnoser]
public class ListsVsArrays
{
	private readonly string[] names = ["Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King", "Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker", "Gonzalez", "Nelson", "Carter", "Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez", "Morris", "Rogers", "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez", "James", "Watson", "Brooks", "Kelly", "Sanders", "Price"];
   private IEnumerable<string> fiveLengthNames;

   [GlobalSetup]
   public void GlobalSetup() => 
		this.fiveLengthNames = this.names.Where(_ => _.Length == 5);

	[Benchmark]
	public int GetSizeViaToList() => this.fiveLengthNames.ToList().Count;

	[Benchmark]
	public int GetSizeViaToArray() => this.fiveLengthNames.ToArray().Length;
}