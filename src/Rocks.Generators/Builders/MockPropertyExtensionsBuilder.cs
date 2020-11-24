using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockPropertyExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Properties.Length > 0)
			{
				var typeToMockName = information.TypeToMock.GetName();
				MockPropertyExtensionsBuilder.BuildProperties(writer, information, typeToMockName);
				MockPropertyExtensionsBuilder.BuildIndexers(writer, information, typeToMockName);
			}
		}

		private static void BuildIndexers(IndentedTextWriter writer, MockInformation information, string typeToMockName)
		{
			if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal static IndexerExpectations<{typeToMockName}> Indexers(this Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new IndexerExpectations<{typeToMockName}>(self);");
				writer.Indent--;
				writer.WriteLine();

				if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static IndexerGetterExpectations<{typeToMockName}> Getters(this IndexerExpectations<{typeToMockName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new IndexerGetterExpectations<{typeToMockName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}

				if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static IndexerSetterExpectations<{typeToMockName}> Setters(this IndexerExpectations<{typeToMockName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new IndexerSetterExpectations<{typeToMockName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}
			}

			foreach (var typeGroup in information.Properties
				.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					_.Value.IsIndexer)
				.GroupBy(_ => _.Value.ContainingType))
			{
				var containingTypeName = typeGroup.Key.GetName();
				writer.WriteLine($"internal static ExplicitIndexerExpectations<{typeToMockName}, {containingTypeName}> ExplicitIndexersFor{containingTypeName}(this Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new ExplicitIndexerExpectations<{typeToMockName}, {containingTypeName}>(self);");
				writer.Indent--;
				writer.WriteLine();

				if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet))
				{
					writer.WriteLine($"internal static ExplicitIndexerGetterExpectations<{typeToMockName}, {containingTypeName}> Getters(this ExplicitIndexerExpectations<{typeToMockName}, {containingTypeName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new ExplicitIndexerGetterExpectations<{typeToMockName}, {containingTypeName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}

				if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
				{
					writer.WriteLine($"internal static ExplicitIndexerSetterExpectations<{typeToMockName}, {containingTypeName}> Setters(this ExplicitIndexerExpectations<{typeToMockName}, {containingTypeName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new ExplicitIndexerSetterExpectations<{typeToMockName}, {containingTypeName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}
			}
		}

		private static void BuildProperties(IndentedTextWriter writer, MockInformation information, string typeToMockName)
		{
			if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal static PropertyExpectations<{typeToMockName}> Properties(this Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new PropertyExpectations<{typeToMockName}>(self);");
				writer.Indent--;
				writer.WriteLine();

				if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static PropertyGetterExpectations<{typeToMockName}> Getters(this PropertyExpectations<{typeToMockName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new PropertyGetterExpectations<{typeToMockName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}

				if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static PropertySetterExpectations<{typeToMockName}> Setters(this PropertyExpectations<{typeToMockName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new PropertySetterExpectations<{typeToMockName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}
			}

			foreach (var typeGroup in information.Properties
				.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					!_.Value.IsIndexer)
				.GroupBy(_ => _.Value.ContainingType))
			{
				var containingTypeName = typeGroup.Key.GetName();
				writer.WriteLine($"internal static ExplicitPropertyExpectations<{typeToMockName}, {containingTypeName}> ExplicitPropertiesFor{containingTypeName}(this Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new ExplicitPropertyExpectations<{typeToMockName}, {containingTypeName}>(self);");
				writer.Indent--;
				writer.WriteLine();

				if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet))
				{
					writer.WriteLine($"internal static ExplicitPropertyGetterExpectations<{typeToMockName}, {containingTypeName}> Getters(this ExplicitPropertyExpectations<{typeToMockName}, {containingTypeName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new ExplicitPropertyGetterExpectations<{typeToMockName}, {containingTypeName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}

				if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
				{
					writer.WriteLine($"internal static ExplicitPropertySetterExpectations<{typeToMockName}, {containingTypeName}> Setters(this ExplicitPropertyExpectations<{typeToMockName}, {containingTypeName}> self) =>");
					writer.Indent++;
					writer.WriteLine($"new ExplicitPropertySetterExpectations<{typeToMockName}, {containingTypeName}>(self.Expectations);");
					writer.Indent--;
					writer.WriteLine();
				}
			}
		}
	}
}
