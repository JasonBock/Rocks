﻿using Microsoft.CodeAnalysis.Text;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Rocks.Builders.Make;

internal sealed class RockMakeBuilder
{
	internal RockMakeBuilder(TypeMockModel mockType)
	{
		this.MockType = mockType;
		(this.Name, this.Text) = this.Build();
	}

	private (string, SourceText) Build()
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

		indentWriter.WriteLines(
			"""
			// <auto-generated/>

			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775

			#nullable enable

			""");

		if (this.MockType.Aliases.Length > 0)
		{
			var requiredAliases = this.MockType.Aliases
				.Select(_ => $"extern alias {_};").ToArray();
			foreach (var requiredAlias in requiredAliases)
			{
				indentWriter.WriteLine(requiredAlias);
			}

			indentWriter.WriteLine();
		}

		var mockNamespace = this.MockType.Type.Namespace;

		if (mockNamespace is not null)
		{
			indentWriter.WriteLine($"namespace {mockNamespace}");
			indentWriter.WriteLine("{");
			indentWriter.Indent++;
		}

		MockExpectationBuilder.Build(indentWriter, this.MockType);

		if (mockNamespace is not null)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		indentWriter.WriteLine();
		indentWriter.WriteLines(
			"""
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			""");

		var text = SourceText.From(writer.ToString(), Encoding.UTF8);
		var name = $"{this.MockType.Type.FullyQualifiedName.GenerateFileName()}_Rock_Make.g.cs";
		return (name, text);
	}

	public string Name { get; private set; }
	public SourceText Text { get; private set; }
	private TypeMockModel MockType { get; }
}