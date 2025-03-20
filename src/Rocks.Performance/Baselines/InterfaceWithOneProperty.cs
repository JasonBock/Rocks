using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithOneProperty
	: BaselineTest
{
	public InterfaceWithOneProperty()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IOneProperty), BuildType.Create)]
			
			public interface IOneProperty
			{
				string Name { get; set; }
			}
			""")
	{ }
}