using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithPointerParameter
	: BaselineTest
{
	public InterfaceWithPointerParameter()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IHavePointer), BuildType.Create)]
			
			public unsafe interface IHavePointer
			{
				void PointerParameter(int* value);
			}			
			""")
	{ }
}