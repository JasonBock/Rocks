using System.CodeDom.Compiler;
using System.Reflection;

namespace Rocks.Extensions;

internal static class IndentedTextWriterExtensions
{
	// This is fragile, but hopefully we can get a public readonly property for this private field
	// in the future.
	internal static string GetTabString(this IndentedTextWriter self)
	{
		var tabStringField = typeof(IndentedTextWriter).GetField("_tabString", BindingFlags.NonPublic | BindingFlags.Instance);
		return tabStringField is not null ? (string)tabStringField.GetValue(self)! : "\t";
	}

	internal static void WriteLines(this IndentedTextWriter self, string content, string templateIndentation = "\t", int indentation = 0)
	{
		var tabString = self.GetTabString();

		if (indentation > 0)
		{
			self.Indent += indentation;
		}

		foreach (var line in content.Split(new[] { self.NewLine }, StringSplitOptions.None))
		{
			var contentLine = line;

			if (templateIndentation != tabString)
			{
				var foundTemplateIndentationCount = 0;

				while (contentLine.StartsWith(templateIndentation, StringComparison.InvariantCultureIgnoreCase))
				{
					contentLine = contentLine.Substring(templateIndentation.Length);
					foundTemplateIndentationCount++;
				}
				for (var i = 0; i < foundTemplateIndentationCount; i++)
				{
					contentLine = contentLine.Insert(0, tabString);
				}
			}

			self.WriteLine(contentLine);
		}

		if (indentation > 0)
		{
			self.Indent -= indentation;
		}
	}
}