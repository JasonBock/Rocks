using Microsoft.CodeAnalysis;
using Rocks.Builders.Shim;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockTypeBuilderV4
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, string expectationsFullyQualifiedName)
	{
		var kind = type.Type.IsRecord ? "record" : "class";
		var mockTypeName = $"Rock{type.Type.FlattenedName}";

		if (type.Type.AttributesDescription.Length > 0)
		{
			writer.WriteLine(type.Type.AttributesDescription);
		}

		writer.WriteLine($"private sealed {kind} {mockTypeName}");
		writer.Indent++;

		var canRaiseEvents = type.Events.Length > 0;

		writer.WriteLine($": {type.Type.FullyQualifiedName}{(canRaiseEvents ? $", global::Rocks.IRaiseEvents" : string.Empty)}");
		writer.Indent--;

		writer.WriteLine("{");
		writer.Indent++;

		MockTypeBuilderV4.BuildShimFields(writer, type);
		MockTypeBuilderV4.BuildRefReturnFields(writer, type);

		if (type.Constructors.Length > 0)
		{
			foreach (var constructor in type.Constructors)
			{
				MockConstructorBuilderV4.Build(writer, type, constructor.Parameters, type.Shims, expectationsFullyQualifiedName);
			}
		}
		else
		{
			MockConstructorBuilderV4.Build(writer, type, ImmutableArray<ParameterModel>.Empty, type.Shims, expectationsFullyQualifiedName);
		}

		writer.WriteLine();

		foreach (var method in type.Methods)
		{
			if (method.ReturnsVoid)
			{
				MockMethodVoidBuilderV4.Build(writer, method, canRaiseEvents);
			}
			else
			{
				MockMethodValueBuilderV4.Build(writer, method, canRaiseEvents);
			}
		}

		var hasProperties = false;

		foreach (var property in type.Properties.Where(_ => !_.IsIndexer))
		{
			hasProperties = true;
			MockPropertyBuilderV4.Build(writer, property, canRaiseEvents);
		}

		if (hasProperties)
		{
			writer.WriteLine();
		}

		var hasIndexers = false;

		foreach (var indexer in type.Properties.Where(_ => _.IsIndexer))
		{
			hasIndexers = true;
			MockIndexerBuilderV4.Build(writer, indexer, canRaiseEvents);
		}

		if (hasIndexers)
		{
			writer.WriteLine();
		}

		if (canRaiseEvents)
		{
			MockEventsBuilderV4.Build(writer, type.Events);
			writer.WriteLine();
		}

		MockTypeBuilderV4.BuildShimTypes(writer, type, mockTypeName);

		writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildShimTypes(IndentedTextWriter writer, TypeMockModel type, string mockTypeName)
	{
		foreach (var shimType in type.Shims)
		{
			writer.WriteLine();
			ShimBuilder.Build(writer, shimType, mockTypeName);
		}
	}

	private static void BuildShimFields(IndentedTextWriter writer, TypeMockModel type)
	{
		foreach (var shimType in type.Shims)
		{
			writer.WriteLine($"private readonly {shimType.Type.FullyQualifiedName} shimFor{shimType.Type.FlattenedName};");
		}
	}

	private static void BuildRefReturnFields(IndentedTextWriter writer, TypeMockModel type)
	{
		foreach (var method in type.Methods.Where(_ => _.ReturnsByRef || _.ReturnsByRefReadOnly))
		{
			writer.WriteLine($"private {method.ReturnType.FullyQualifiedName} rr{method.MemberIdentifier};");
		}

		foreach (var property in type.Properties.Where(_ => _.ReturnsByRef || _.ReturnsByRefReadOnly))
		{
			writer.WriteLine($"private {property.Type.FullyQualifiedName} rr{property.MemberIdentifier};");
		}
	}
}