using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Make
{
	internal static class MockMakeBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information, NamespaceGatherer namespaces)
		{
			var typeToMock = information.TypeToMock;
			writer.WriteLine($"private sealed class Rock{typeToMock.GetName(TypeNameOption.Flatten)}");
			writer.Indent++;
			writer.WriteLine($": {typeToMock.GetName(TypeNameOption.IncludeGenerics)}");
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
					MockMethodVoidBuilder.Build(writer, method);
				}
				else
				{
					MockMethodValueBuilder.Build(writer, method, information.Model, namespaces);
				}

				memberIdentifier++;
			}

			foreach (var property in information.Properties.Where(_ => !_.Value.IsIndexer))
			{
				MockPropertyBuilder.Build(writer, property);
			}

			foreach (var indexer in information.Properties.Where(_ => _.Value.IsIndexer))
			{
				MockIndexerBuilder.Build(writer, indexer);
			}

			if (information.Events.Length > 0)
			{
				writer.WriteLine();
				MockEventsBuilder.Build(writer, information.Events);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildRefReturnFields(IndentedTextWriter writer, MockInformation information)
		{
			foreach(var method in information.Methods.Where(_ => _.Value.ReturnsByRef || _.Value.ReturnsByRefReadonly))
			{
				writer.WriteLine($"private {method.Value.ReturnType.GetName()} rr{method.MemberIdentifier};");
			}

			foreach (var property in information.Properties.Where(_ => _.Value.ReturnsByRef || _.Value.ReturnsByRefReadonly))
			{
				writer.WriteLine($"private {property.Value.Type.GetName()} rr{property.MemberIdentifier};");
			}
		}
	}
}