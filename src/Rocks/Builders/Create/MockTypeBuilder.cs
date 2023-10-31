using Microsoft.CodeAnalysis;
using Rocks.Builders.Shim;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockTypeBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		var kind = type.Type.IsRecord ? "record" : "class";
		var mockTypeName = $"Rock{type.Type.FlattenedName}";

		if(type.Type.AttributesDescription.Length > 0)
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

		MockTypeBuilder.BuildShimFields(writer, type);
		MockTypeBuilder.BuildRefReturnFields(writer, type);

		writer.WriteLine($"private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;");
		writer.WriteLine();

		if (type.Constructors.Length > 0)
		{
			foreach (var constructor in type.Constructors)
			{
				MockConstructorBuilder.Build(writer, type, constructor.Parameters, type.Shims);
			}
		}
		else
		{
			MockConstructorBuilder.Build(writer, type, ImmutableArray<ParameterModel>.Empty, type.Shims);
		}

		writer.WriteLine();

		foreach (var method in type.Methods)
		{
			if (method.ReturnsVoid)
			{
				MockMethodVoidBuilder.Build(writer, method, canRaiseEvents);
			}
			else
			{
				MockMethodValueBuilder.Build(writer, method, canRaiseEvents);
			}
		}

		foreach (var property in type.Properties.Where(_ => !_.IsIndexer))
		{
			MockPropertyBuilder.Build(writer, property, canRaiseEvents);
		}

		foreach (var indexer in type.Properties.Where(_ => _.IsIndexer))
		{
			MockIndexerBuilder.Build(writer, indexer, canRaiseEvents);
		}

		if (canRaiseEvents)
		{
			writer.WriteLine();
			MockEventsBuilder.Build(writer, type.Events);
		}

		MockTypeBuilder.BuildShimTypes(writer, type, mockTypeName);

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