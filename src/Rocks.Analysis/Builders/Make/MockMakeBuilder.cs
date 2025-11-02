using Microsoft.CodeAnalysis;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Make;

internal static class MockMakeBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var typeToMock = mockType.Type;
		var kind = typeToMock.IsRecord ? "record" : "class";
		writer.WriteLine($"private sealed {kind} Mock");
		writer.Indent++;
		writer.WriteLine($": {typeToMock.FullyQualifiedName}");
		writer.Indent--;

		writer.WriteLine("{");
		writer.Indent++;

		MockMakeBuilder.BuildRefReturnFields(writer, mockType);

		if (mockType.Constructors.Length > 0)
		{
			foreach (var constructor in mockType.Constructors)
			{
				MockConstructorBuilder.Build(writer, mockType, constructor);
			}
		}
		else
		{
			MockConstructorBuilder.Build(writer, mockType, null);
		}

		writer.WriteLine();

		for (var i = 0; i < mockType.Methods.Length; i++)
		{
			var method = mockType.Methods[i];

			if (method.ReturnsVoid)
			{
				MockMethodVoidBuilder.Build(writer, method);
			}
			else
			{
				MockMethodValueBuilder.Build(writer, method);
			}

			if (i != mockType.Methods.Length - 1)
			{
				writer.WriteLine();
			}
		}

		var properties = mockType.Properties.Where(_ => !_.IsIndexer).ToArray();

		if (properties.Length > 0)
		{
			if (mockType.Methods.Length > 0)
			{
				writer.WriteLine();
			}

			for (var i = 0; i < properties.Length; i++)
			{
				var property = properties[i];
				MockPropertyBuilder.Build(writer, property);

				if (i != properties.Length - 1)
				{
					writer.WriteLine();
				}
			}
		}

		var indexers = mockType.Properties.Where(_ => _.IsIndexer).ToArray();

		if (indexers.Length > 0)
		{
			if (mockType.Methods.Length > 0 || properties.Length > 0)
			{
				writer.WriteLine();
			}

			for (var i = 0; i < indexers.Length; i++)
			{
				var indexer = indexers[i];
				MockIndexerBuilder.Build(writer, indexer);

				if (i != indexers.Length - 1)
				{
					writer.WriteLine();
				}
			}
		}

		if (mockType.Events.Length > 0)
		{
			writer.WriteLine();
			MockEventsBuilder.Build(writer, mockType.Events);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildRefReturnFields(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var wereFieldsGenerated = false;

		foreach (var method in mockType.Methods.Where(_ => _.ReturnsByRef || _.ReturnsByRefReadOnly))
		{
			wereFieldsGenerated = true;
			writer.WriteLine($"private {method.ReturnType.FullyQualifiedName} rr{method.MemberIdentifier};");
		}

		foreach (var property in mockType.Properties.Where(_ => _.ReturnsByRef || _.ReturnsByRefReadOnly))
		{
			wereFieldsGenerated = true;
			writer.WriteLine($"private {property.Type.FullyQualifiedName} rr{property.MemberIdentifier};");
		}

		if (wereFieldsGenerated)
		{
			writer.WriteLine();
		}
	}
}