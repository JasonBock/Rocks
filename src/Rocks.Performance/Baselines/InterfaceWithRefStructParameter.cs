using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithRefStructParameter
	: BaselineTest
{
	public InterfaceWithRefStructParameter()
		: base(
			"""
			using Rocks;
			using System;

			[assembly: Rock(typeof(IHaveRefStruct), BuildType.Create)]
			
			public interface IHaveRefStruct
			{
				void Work(Span<int> data);
			}
			""")
	{ }
}