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
			var typeToMockName = information.TypeToMock!.GenericName;
			MockPropertyExtensionsBuilder.BuildProperties(writer, information, typeToMockName);
			MockPropertyExtensionsBuilder.BuildIndexers(writer, information, typeToMockName);
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, MockInformation information, string typeToMockName)
	{
		if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal static {WellKnownNames.Indexer}{WellKnownNames.Expectations}<{typeToMockName}> {WellKnownNames.Indexers}(this {WellKnownNames.Expectations}<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				writer.WriteLine($"internal static {WellKnownNames.Indexer}{WellKnownNames.Getter}{WellKnownNames.Expectations}<{typeToMockName}> {WellKnownNames.Getters}(this {WellKnownNames.Indexer}{WellKnownNames.Expectations}<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static {WellKnownNames.Indexer}{WellKnownNames.Setter}{WellKnownNames.Expectations}<{typeToMockName}> {WellKnownNames.Setters}(this {WellKnownNames.Indexer}{WellKnownNames.Expectations}<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
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
			var flattenedContainingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static {WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> {WellKnownNames.Explicit}{WellKnownNames.Indexers}For{flattenedContainingTypeName}(this {WellKnownNames.Expectations}<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit))
			{
				writer.WriteLine($"internal static {WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Getter}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> {WellKnownNames.Getters}(this {WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
				writer.Indent--;
				writer.WriteLine();
			}

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			{
				writer.WriteLine($"internal static {WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Setter}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> {WellKnownNames.Setters}(this {WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
				writer.Indent--;
				writer.WriteLine();
			}
		}
	}

	private static void BuildProperties(IndentedTextWriter writer, MockInformation information, string typeToMockName)
	{
		if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal static {WellKnownNames.Property}{WellKnownNames.Expectations}<{typeToMockName}> {WellKnownNames.Properties}(this {WellKnownNames.Expectations}<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				writer.WriteLine($"internal static {WellKnownNames.Property}{WellKnownNames.Getter}{WellKnownNames.Expectations}<{typeToMockName}> {WellKnownNames.Getters}(this {WellKnownNames.Property}{WellKnownNames.Expectations}<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static {WellKnownNames.Property}{WellKnownNames.Setter}{WellKnownNames.Expectations}<{typeToMockName}> {WellKnownNames.Setters}(this {WellKnownNames.Property}{WellKnownNames.Expectations}<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
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
			writer.WriteLine($"internal static {WellKnownNames.Explicit}{WellKnownNames.Property}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> {WellKnownNames.Explicit}{WellKnownNames.Properties}For{flattenedContainingTypeName}(this {WellKnownNames.Expectations}<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new(self);");
			writer.Indent--;
			writer.WriteLine();

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit))
			{
				writer.WriteLine($"internal static {WellKnownNames.Explicit}{WellKnownNames.Property}{WellKnownNames.Getter}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> {WellKnownNames.Getters}(this {WellKnownNames.Explicit}{WellKnownNames.Property}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
				writer.Indent--;
				writer.WriteLine();
			}

			if (typeGroup.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			{
				writer.WriteLine($"internal static {WellKnownNames.Explicit}{WellKnownNames.Property}{WellKnownNames.Setter}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> {WellKnownNames.Setters}(this {WellKnownNames.Explicit}{WellKnownNames.Property}{WellKnownNames.Expectations}<{typeToMockName}, {containingTypeName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new(self.{WellKnownNames.Expectations});");
				writer.Indent--;
				writer.WriteLine();
			}
		}
	}
}