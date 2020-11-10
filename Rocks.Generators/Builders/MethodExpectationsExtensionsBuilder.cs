using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

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
			if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal static class MethodExpectationsOf{information.TypeToMock.Name}Extensions");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Methods.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					MethodExpectationsExtensionsMethodBuilder.Build(writer, result, namespaces);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach(var explicitMethodGroups in information.Methods.GroupBy(_ => _.Value.ContainingType, _ => _))
				{
					var baseTypeName = explicitMethodGroups.Key.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
					writer.WriteLine($"internal static class ExplicitMethodExpectationsOf{baseTypeName}Extensions");
					writer.WriteLine("{");
					writer.Indent++;

					foreach(var explicitMethodResult in explicitMethodGroups)
					{
						MethodExpectationsExtensionsMethodBuilder.Build(writer, explicitMethodResult, namespaces);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}
			}
		}
	}
}