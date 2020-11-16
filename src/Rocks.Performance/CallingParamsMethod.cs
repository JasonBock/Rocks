using BenchmarkDotNet.Attributes;
using System;

namespace Rocks.Performance
{
	[MemoryDiagnoser]
	public class CallingParamsMethod
	{
		[Benchmark]
		public int CallParamsWithParameters() => this.CallMe(new object(), Guid.NewGuid(), "ABCFJLEIJAFLJ", 247538);

		[Benchmark]
		public int CallParamsWithParametersInArray() => this.CallMe(new object[] { new object(), Guid.NewGuid(), "ABCFJLEIJAFLJ", 247538 });

		private int CallMe(params object[] values) => values.Length;
	}
}