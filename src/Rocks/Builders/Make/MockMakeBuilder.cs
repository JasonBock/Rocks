using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockMakeBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, NamespaceGatherer namespaces,
		Compilation compilation)
	{
		var typeToMock = information.TypeToMock!;
		var kind = typeToMock.Type.IsRecord ? "record" : "class";
		writer.WriteLine($"private sealed {kind} {nameof(Rock)}{typeToMock.FlattenedName}");
		writer.Indent++;
		writer.WriteLine($": {typeToMock.GenericName}");
		writer.Indent--;

		writer.WriteLine("{");
		writer.Indent++;

		MockMakeBuilder.BuildRefReturnFields(writer, information);

		if (information.Constructors.Length > 0)
		{
			foreach (var constructor in information.Constructors)
			{
				MockConstructorBuilder.Build(writer, typeToMock, constructor.Parameters);
			}
		}
		else
		{
			MockConstructorBuilder.Build(writer, typeToMock, ImmutableArray<IParameterSymbol>.Empty);
		}

		writer.WriteLine();

		var memberIdentifier = 0u;

		foreach (var method in information.Methods)
		{
			if (method.Value.ReturnsVoid)
			{
				MockMethodVoidBuilder.Build(writer, method, namespaces, compilation);
			}
			else
			{
				MockMethodValueBuilder.Build(writer, method, information.Model, namespaces, compilation);
			}

			memberIdentifier++;
		}

		foreach (var property in information.Properties.Where(_ => !_.Value.IsIndexer))
		{
			MockPropertyBuilder.Build(writer, property, compilation);
		}

		foreach (var indexer in information.Properties.Where(_ => _.Value.IsIndexer))
		{
			MockIndexerBuilder.Build(writer, indexer, compilation);
		}

		if (information.Events.Length > 0)
		{
			writer.WriteLine();
			MockEventsBuilder.Build(writer, information.Events, compilation);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildRefReturnFields(IndentedTextWriter writer, MockInformation information)
	{
		foreach (var method in information.Methods.Where(_ => _.Value.ReturnsByRef || _.Value.ReturnsByRefReadonly))
		{
			writer.WriteLine($"private {method.Value.ReturnType.GetName()} rr{method.MemberIdentifier};");
		}

		foreach (var property in information.Properties.Where(_ => _.Value.ReturnsByRef || _.Value.ReturnsByRefReadonly))
		{
			writer.WriteLine($"private {property.Value.Type.GetName()} rr{property.MemberIdentifier};");
		}
	}
}