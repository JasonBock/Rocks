using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceWithOneIndexer
	: BaselineTest
{
	public InterfaceWithOneIndexer()
		: base(
			"""
			using Rocks;
			using System;
						
			[assembly: Rock(typeof(IOneIndexer), BuildType.Create)]
			
			public interface IOneIndexer
			{
				string this[int valueOne, Guid valueTwo] { get; set; }
			}
			""")
	{ }
}