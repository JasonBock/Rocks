using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithRefReadonlyReturn
	: BaselineTest
{
	public InterfaceWithRefReadonlyReturn()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IHaveRefReadonlyReturn), BuildType.Create)]
			
			public interface IHaveRefReadonlyReturn
			{
			    ref readonly int Work();   
			}			
			""")
	{ }
}