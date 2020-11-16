using BenchmarkDotNet.Attributes;
using System.Globalization;

namespace Rocks.Performance
{
	[MemoryDiagnoser]
	public class FormattingAndBoxing
	{
		private readonly uint value1 = 324;
		private readonly int value2 = 43219;

		[Benchmark]
		public string FormatStringWithNoToString() =>
			string.Format("The expected call count is incorrect. Expected: {0}, received: {1}.", this.value1, this.value2);

		[Benchmark]
		public string FormatStringWithToString() =>
			string.Format("The expected call count is incorrect. Expected: {0}, received: {1}.", this.value1.ToString(), this.value2.ToString());

		[Benchmark]
		public string InterpolateStringWithNoToString() =>
			$"The expected call count is incorrect. Expected: {this.value1}, received: {this.value2}.";

		[Benchmark]
		public string InterpolateStringWithToString() =>
			$"The expected call count is incorrect. Expected: {this.value1.ToString("N")}, received: {this.value2.ToString("N")}.";

		[Benchmark]
		public string InterpolateStringWithToStringNoFormatStrings() =>
			$"The expected call count is incorrect. Expected: {this.value1.ToString()}, received: {this.value2.ToString()}.";

		[Benchmark]
		public string InterpolateStringWithToStringNoFormatStringsAndCulture() =>
			$"The expected call count is incorrect. Expected: {this.value1.ToString(CultureInfo.CurrentCulture)}, received: {this.value2.ToString(CultureInfo.CurrentCulture)}.";

		[Benchmark]
		public string ConcatenateWithNoToString() =>
			"The expected call count is incorrect. Expected: " + this.value1 + ", received: " + this.value2 + ".";

		[Benchmark]
		public string ConcatenateWithToString() =>
			"The expected call count is incorrect. Expected: " + this.value1.ToString() + ", received: " + this.value2.ToString() + ".";
	}
}