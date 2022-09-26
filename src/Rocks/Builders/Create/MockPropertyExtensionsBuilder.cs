using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockPropertyExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Properties.Length > 0)
		{
			var typeToMockName = information.TypeToMock!.ReferenceableName;
			MockPropertyExtensionsBuilder.BuildProperties(writer, information, typeToMockName);
			MockPropertyExtensionsBuilder.BuildIndexers(writer, information, typeToMockName);
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, MockInformation information, string typeToMockName)
	{
		if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal static global::Rocks.Expectations.IndexerExpectations<{typeToMockName}> Indexers(this global::Rocks.Expectations.Expectations<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.IndexerGetterExpectations<{typeToMockName}> Getters(this global::Rocks.Expectations.IndexerExpectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.IndexerSetterExpectations<{typeToMockName}> Setters(this global::Rocks.Expectations.IndexerExpectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
				writer.Indent--;
				writer.WriteLine();
			}
		}

		foreach (var typeGroup in information.Properties
			.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				_.Value.IsIndexer)
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetReferenceableName();
			var flattenedContainingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static global::Rocks.Expectations.ExplicitIndexerExpectations<{typeToMockName}, {containingTypeName}> ExplicitIndexersFor{flattenedContainingTypeName}(this global::Rocks.Expectations.Expectations<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.ExplicitIndexerGetterExpectations<{typeToMockName}, {containingTypeName}> Getters(this global::Rocks.Expectations.ExplicitIndexerExpectations<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.ExplicitIndexerSetterExpectations<{typeToMockName}, {containingTypeName}> Setters(this global::Rocks.Expectations.ExplicitIndexerExpectations<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
				writer.Indent--;
				writer.WriteLine();
			}
		}
	}

	private static void BuildProperties(IndentedTextWriter writer, MockInformation information, string typeToMockName)
	{
		if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal static global::Rocks.Expectations.PropertyExpectations<{typeToMockName}> Properties(this global::Rocks.Expectations.Expectations<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.PropertyGetterExpectations<{typeToMockName}> Getters(this global::Rocks.Expectations.PropertyExpectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.PropertySetterExpectations<{typeToMockName}> Setters(this global::Rocks.Expectations.PropertyExpectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
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
			var flattenedContainingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static global::Rocks.Expectations.ExplicitPropertyExpectations<{typeToMockName}, {containingTypeName}> ExplicitPropertiesFor{flattenedContainingTypeName}(this global::Rocks.Expectations.Expectations<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.ExplicitPropertyGetterExpectations<{typeToMockName}, {containingTypeName}> Getters(this global::Rocks.Expectations.ExplicitPropertyExpectations<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			{
				writer.WriteLine($"internal static global::Rocks.Expectations.ExplicitPropertySetterExpectations<{typeToMockName}, {containingTypeName}> Setters(this global::Rocks.Expectations.ExplicitPropertyExpectations<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine("new(self);");
				writer.Indent--;
				writer.WriteLine();
			}
		}
	}
}