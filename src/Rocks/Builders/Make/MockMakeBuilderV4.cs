using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockMakeBuilderV4
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var typeToMock = mockType.Type;
		var kind = typeToMock.IsRecord ? "record" : "class";
		writer.WriteLine($"private sealed {kind} Rock{typeToMock.FlattenedName}");
		writer.Indent++;
		writer.WriteLine($": {typeToMock.FullyQualifiedName}");
		writer.Indent--;

		writer.WriteLine("{");
		writer.Indent++;

		MockMakeBuilderV4.BuildRefReturnFields(writer, mockType);

		if (mockType.Constructors.Length > 0)
		{
			foreach (var constructor in mockType.Constructors)
			{
				MockConstructorBuilderV4.Build(writer, mockType, constructor.Parameters);
			}
		}
		else
		{
			MockConstructorBuilderV4.Build(writer, mockType, ImmutableArray<ParameterModel>.Empty);
		}

		writer.WriteLine();

		foreach (var method in mockType.Methods)
		{
			if (method.ReturnsVoid)
			{
				MockMethodVoidBuilderV4.Build(writer, method);
			}
			else
			{
				MockMethodValueBuilderV4.Build(writer, method);
			}
		}

		foreach (var property in mockType.Properties.Where(_ => !_.IsIndexer))
		{
			MockPropertyBuilderV4.Build(writer, property);
		}

		foreach (var indexer in mockType.Properties.Where(_ => _.IsIndexer))
		{
			MockIndexerBuilderV4.Build(writer, indexer);
		}

		if (mockType.Events.Length > 0)
		{
			writer.WriteLine();
			MockEventsBuilderV4.Build(writer, mockType.Events);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildRefReturnFields(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var method in mockType.Methods.Where(_ => _.ReturnsByRef || _.ReturnsByRefReadOnly))
		{
			writer.WriteLine($"private {method.ReturnType.FullyQualifiedName} rr{method.MemberIdentifier};");
		}

		foreach (var property in mockType.Properties.Where(_ => _.ReturnsByRef || _.ReturnsByRefReadOnly))
		{
			writer.WriteLine($"private {property.Type.FullyQualifiedName} rr{property.MemberIdentifier};");
		}
	}
}