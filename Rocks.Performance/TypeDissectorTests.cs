using BenchmarkDotNet.Attributes;

namespace Rocks.Performance
{
	[MemoryDiagnoser]
	public class TypeDissectorTests
	{
		[Benchmark]
		public string GetSafeNameWithCache() => 
			TypeDissector.Create(typeof(NestedGenerics.IHaveGenerics<string>)).SafeName;
	}

	public class NestedGenerics
	{
		public interface IHaveGenerics<T> { }
	}
}
