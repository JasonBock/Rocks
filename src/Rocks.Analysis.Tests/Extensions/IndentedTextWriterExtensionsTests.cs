using NUnit.Framework;
using Rocks.Analysis.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Tests.Extensions;

internal static class IndentedTextWriterExtensionsTests
{
	[Test]
	public static void WriteLines()
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

		indentWriter.WriteLines(
			"""
			First Line
			Second Line
			Third Line
			""");

		Assert.That(writer.ToString(), Is.EqualTo("First Line\r\nSecond Line\r\nThird Line\r\n"));
	}

	[Test]
	public static void WriteLinesWithIndentation()
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

		indentWriter.WriteLines(
			"""
			First Line
			Second Line
			Third Line
			""", 2);

		Assert.That(writer.ToString(), Is.EqualTo("\t\tFirst Line\r\n\t\tSecond Line\r\n\t\tThird Line\r\n"));
	}
}