using System.CodeDom.Compiler;

namespace Rocks.Extensions;

internal static class IndentedTextWriterExtensions
{
	internal static void WriteLines(this IndentedTextWriter self, string content, int indentation = 0)
	{
		if (indentation > 0)
		{
			self.Indent += indentation;
		}

		foreach (var line in content.Split(new[] { self.NewLine }, StringSplitOptions.None))
		{
			self.WriteLine(line);
		}

		if (indentation > 0)
		{
			self.Indent -= indentation;
		}
	}
}