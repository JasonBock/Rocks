using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockMethodExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if(information.Methods.Length > 0)
			{
				var typeToMockName = information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

				if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					writer.WriteLine($"internal static MethodExpectations<{typeToMockName}> Methods(this Expectations<{typeToMockName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new MethodExpectations<{typeToMockName}>(self);");
					writer.Indent--;
					writer.WriteLine();
				}

				if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
				{
					foreach (var typeGroup in information.Methods
						.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
						.GroupBy(_ => _.Value.ContainingType))
					{
						var containingTypeName = typeGroup.Key.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
						writer.WriteLine($"internal static ExplicitMethodExpectations<{typeToMockName}, {containingTypeName }> ExplicitMethodsFor{containingTypeName}(this Expectations<{typeToMockName}> self) =>");
						writer.Indent++;
						writer.WriteLine($"new ExplicitMethodExpectations<{typeToMockName}, {containingTypeName}> (self);");
						writer.Indent--;
						writer.WriteLine();
					}
				}
			}
		}
	}
}