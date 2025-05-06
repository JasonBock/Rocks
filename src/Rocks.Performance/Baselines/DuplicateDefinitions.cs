using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class DuplicateDefinitions
	: BaselineTest
{
	public DuplicateDefinitions()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IOneMethod), BuildType.Create)]
			[assembly: Rock(typeof(ITwoMethod), BuildType.Create)]
			[assembly: Rock(typeof(IOneMethod), BuildType.Create)]
						
			public interface IOneMethod
			{
				void Work();
			}

			public interface ITwoMethod
			{
				void Work();
			}
			""")
	{ }
}