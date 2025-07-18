﻿using Microsoft.CodeAnalysis;
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

		foreach (var method in mockType.Methods)
		{
			if (method.ReturnsVoid)
			{
				MockMethodVoidBuilder.Build(writer, method);
			}
			else
			{
				MockMethodValueBuilder.Build(writer, method);
			}
		}

		foreach (var property in mockType.Properties.Where(_ => !_.IsIndexer))
		{
			MockPropertyBuilder.Build(writer, property);
		}

		foreach (var indexer in mockType.Properties.Where(_ => _.IsIndexer))
		{
			MockIndexerBuilder.Build(writer, indexer);
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