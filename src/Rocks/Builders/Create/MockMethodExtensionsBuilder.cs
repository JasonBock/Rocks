using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockMethodExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Methods.Length > 0)
		{
			var typeToMockName = information.TypeToMock!.ReferenceableName;

			if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.MethodExpectations<{typeToMockName}> Methods(this global::Rocks.Expectations.Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach (var typeGroup in information.Methods
					.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.Value.ContainingType))
				{
					var containingTypeName = typeGroup.Key.GetReferenceableName();
					var flattenedContainingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
					writer.WriteLine($"internal static global::Rocks.Expectations.ExplicitMethodExpectations<{typeToMockName}, {containingTypeName}> ExplicitMethodsFor{flattenedContainingTypeName}(this global::Rocks.Expectations.Expectations<{typeToMockName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new(self);");
					writer.Indent--;
					writer.WriteLine();
				}
			}
		}
	}
}