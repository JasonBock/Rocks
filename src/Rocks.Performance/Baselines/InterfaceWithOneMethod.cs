using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithOneMethod
	: BaselineTest
{
	public InterfaceWithOneMethod()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IOneMethod), BuildType.Create)]
			
			public interface IOneMethod
			{
				void Work();
			}
			""")
	{ }
}