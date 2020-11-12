using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Rocks.Builders
{
	/*
	internal static class MethodExpectationsOf{TypeToMock}Extensions
	{
		internal static MethodAdornments Foo(this MethodExpectations<IMockable> self, Arg<int> a) =>
			new MethodAdornments(self.Add(0, new Dictionary<string, Arg>
			{
				{ "a", a }
			}));
	}
	*/
	internal static class MethodExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information, SortedSet<string> namespaces)
		{
			writer.WriteLine($"internal static class MethodExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Methods)
			{
				MethodExpectationsExtensionsMethodBuilder.Build(writer, result, namespaces);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}