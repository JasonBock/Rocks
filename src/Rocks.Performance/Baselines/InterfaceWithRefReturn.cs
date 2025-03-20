using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithRefReturn
	: BaselineTest
{
	public InterfaceWithRefReturn()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IHaveRefReturn), BuildType.Create)]
			
			public interface IHaveRefReturn
			{
			    ref int Work();   
			}			
			""")
	{ }
}