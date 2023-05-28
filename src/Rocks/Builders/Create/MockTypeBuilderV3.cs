using Microsoft.CodeAnalysis;
using Rocks.Builders.Shim;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockTypeBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, Compilation compilation)
	{
		var kind = type.Type.IsRecord ? "record" : "class";
		var mockTypeName = $"Rock{type.Type.FlattenedName}";

		writer.WriteLine($"private sealed {kind} {mockTypeName}");
		writer.Indent++;

		var canRaiseEvents = type.Events.Length > 0;

		writer.WriteLine($": {type.Type.FullyQualifiedName}{(canRaiseEvents ? $", global::Rocks.IRaiseEvents" : string.Empty)}");
		writer.Indent--;

		writer.WriteLine("{");
		writer.Indent++;

		MockTypeBuilderV3.BuildShimFields(writer, type);
		MockTypeBuilderV3.BuildRefReturnFields(writer, type);

		writer.WriteLine($"private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;");
		writer.WriteLine();

		if (type.Constructors.Length > 0)
		{
			foreach (var constructor in type.Constructors)
			{
				MockConstructorBuilderV3.Build(writer, type, compilation, constructor.Parameters, type.Shims);
			}
		}
		else
		{
			MockConstructorBuilderV3.Build(writer, type, compilation, ImmutableArray<ParameterModel>.Empty, type.Shims);
		}

		writer.WriteLine();

		foreach (var method in type.Methods)
		{
			if (method.ReturnsVoid)
			{
				MockMethodVoidBuilderV3.Build(writer, method, canRaiseEvents, compilation);
			}
			else
			{
				MockMethodValueBuilderV3.Build(writer, method, canRaiseEvents, compilation);
			}
		}

		// TODO: Revisit later.
		//foreach (var property in type.Properties.Where(_ => !_.Value.IsIndexer))
		//{
		//	MockPropertyBuilder.Build(writer, property, canRaiseEvents, compilation);
		//}

		//foreach (var indexer in type.Properties.Where(_ => _.Value.IsIndexer))
		//{
		//	MockIndexerBuilder.Build(writer, indexer, canRaiseEvents, compilation);
		//}

		//if (canRaiseEvents)
		//{
		//	writer.WriteLine();
		//	MockEventsBuilder.Build(writer, type.Events, compilation);
		//}

		MockTypeBuilderV3.BuildShimTypes(writer, type, mockTypeName);

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildShimTypes(IndentedTextWriter writer, TypeMockModel type, string mockTypeName)
	{
		foreach (var shimType in type.Shims)
		{
			writer.WriteLine();
			ShimBuilderV3.Build(writer, shimType, mockTypeName);
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