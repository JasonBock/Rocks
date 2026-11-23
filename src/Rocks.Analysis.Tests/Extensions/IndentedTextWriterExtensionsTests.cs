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

		var expected = string.Join(Environment.NewLine, "First Line", "Second Line", "Third Line") + Environment.NewLine;
		Assert.That(writer.ToString(), Is.EqualTo(expected));
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

		var expected = string.Join(Environment.NewLine, "\t\tFirst Line", "\t\tSecond Line", "\t\tThird Line") + Environment.NewLine;
		Assert.That(writer.ToString(), Is.EqualTo(expected));
	}
}