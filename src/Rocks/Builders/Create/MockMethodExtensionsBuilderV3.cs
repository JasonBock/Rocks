using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockMethodExtensionsBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeModel type)
	{
		if (type.Methods.Length > 0)
		{
			var typeToMockName = type.FullyQualifiedName;

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.MethodExpectations<{{typeToMockName}}> Methods(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach (var typeGroup in type.Methods
					.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => (_.ContainingTypeFullyQualifiedName, _.ContainingTypeFlattenedName)))
				{
					var containingTypeName = typeGroup.Key.ContainingTypeFullyQualifiedName;
					var flattenedContainingTypeName = typeGroup.Key.ContainingTypeFlattenedName;
					writer.WriteLines(
						$$"""
						internal static global::Rocks.Expectations.ExplicitMethodExpectations<{{typeToMockName}}, {{containingTypeName}}> ExplicitMethodsFor{{flattenedContainingTypeName}}(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
							new(@self);

						""");
				}
			}
		}
	}
}