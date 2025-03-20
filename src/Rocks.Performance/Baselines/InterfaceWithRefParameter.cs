using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithRefParameter
	: BaselineTest
{
	public InterfaceWithRefParameter()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IHaveRef), BuildType.Create)]
			
			public interface IHaveRef
			{
			    void Work(ref int value);   
			}			
			""")
	{ }
}