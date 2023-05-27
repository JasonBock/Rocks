using Microsoft.CodeAnalysis;
using Rocks.Builders.Shim;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockTypeBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeModel type, Compilation compilation)
	{
		var kind = type.IsRecord ? "record" : "class";
		var mockTypeName = $"Rock{type.FlattenedName}";

		writer.WriteLine($"private sealed {kind} {mockTypeName}");
		writer.Indent++;

		var canRaiseEvents = type.Events.Length > 0;

		writer.WriteLine($": {type.FullyQualifiedName}{(canRaiseEvents ? $", global::Rocks.IRaiseEvents" : string.Empty)}");
		writer.Indent--;

		writer.WriteLine("{");
		writer.Indent++;

		// TODO: Need to put these back on.
		//MockTypeBuilder.BuildShimFields(writer, information);
		//MockTypeBuilder.BuildRefReturnFields(writer, information);

		writer.WriteLine($"private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;");
		writer.WriteLine();

		if (type.Constructors.Length > 0)
		{
			foreach (var constructor in type.Constructors)
			{
				MockConstructorBuilderV3.Build(writer, type, compilation, constructor.Parameters/*, type.Shims*/);
			}
		}
		else
		{
			MockConstructorBuilderV3.Build(writer, type, compilation, ImmutableArray<ParameterModel>.Empty/*, type.Shims*/);
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

		//MockTypeBuilder.BuildShimTypes(writer, type, mockTypeName, compilation);

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildShimTypes(IndentedTextWriter writer, MockInformation information, string mockTypeName,
		Compilation compilation)
	{
		foreach (var shimType in information.Shims)
		{
			writer.WriteLine();
			ShimBuilder.Build(writer, shimType, mockTypeName, compilation, information);
		}
	}

	private static void BuildShimFields(IndentedTextWriter writer, MockInformation information)
	{
		foreach (var shimType in information.Shims)
		{
			writer.WriteLine($"private readonly {shimType.GetFullyQualifiedName()} shimFor{shimType.GetName(TypeNameOption.Flatten)};");
		}
	}

	private static void BuildRefReturnFields(IndentedTextWriter writer, MockInformation information)
	{
		foreach (var method in information.Methods.Results.Where(_ => _.Value.ReturnsByRef || _.Value.ReturnsByRefReadonly))
		{
			writer.WriteLine($"private {method.Value.ReturnType.GetFullyQualifiedName()} rr{method.MemberIdentifier};");
		}

		foreach (var property in information.Properties.Results.Where(_ => _.Value.ReturnsByRef || _.Value.ReturnsByRefReadonly))
		{
			writer.WriteLine($"private {property.Value.Type.GetFullyQualifiedName()} rr{property.MemberIdentifier};");
		}
	}
}