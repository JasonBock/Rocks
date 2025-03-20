using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithOutParameter
	: BaselineTest
{
	public InterfaceWithOutParameter()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IHaveOut), BuildType.Create)]
			
			public interface IHaveOut
			{
			    void Work(out string value);   
			}			
			""")
	{ }
}