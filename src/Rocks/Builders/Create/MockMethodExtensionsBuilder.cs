using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockMethodExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		if (type.Methods.Length > 0)
		{
			var typeToMockName = type.Type.FullyQualifiedName;

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
					.GroupBy(_ => (_.ContainingType)))
				{
					var containingTypeName = typeGroup.Key.FullyQualifiedName;
					var flattenedContainingTypeName = typeGroup.Key.FlattenedName;
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