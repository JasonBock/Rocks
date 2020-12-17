using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class MockTypeBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			var typeToMock = information.TypeToMock;
			writer.WriteLine($"private sealed class Rock{typeToMock.GetName(TypeNameOption.Flatten)}");
			writer.Indent++;
			writer.WriteLine($": {typeToMock.GetName(TypeNameOption.IncludeGenerics)}, {(information.Events.Length > 0 ? nameof(IMockWithEvents) : nameof(IMock))}");
			writer.Indent--;

			writer.WriteLine("{");
			writer.Indent++;

			MockTypeBuilder.BuildRefReturnFields(writer, information);
			writer.WriteLine("private readonly ImmutableDictionary<int, ImmutableArray<HandlerInformation>> handlers;");
			writer.WriteLine();

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

			var raiseEvents = information.Events.Length > 0;

			foreach (var method in information.Methods)
			{
				if (method.Value.ReturnsVoid)
				{
					MockMethodVoidBuilder.Build(writer, method, raiseEvents);
				}
				else
				{
					MockMethodValueBuilder.Build(writer, method, raiseEvents);
				}
			}

			foreach (var property in information.Properties.Where(_ => !_.Value.IsIndexer))
			{
				MockPropertyBuilder.Build(writer, property, raiseEvents);
			}

			foreach (var indexer in information.Properties.Where(_ => _.Value.IsIndexer))
			{
				MockIndexerBuilder.Build(writer, indexer, raiseEvents);
			}

			if (information.Events.Length > 0)
			{
				writer.WriteLine();
				MockEventsBuilder.Build(writer, information.Events);
			}

			writer.WriteLine();
			writer.WriteLine("ImmutableDictionary<int, ImmutableArray<HandlerInformation>> IMock.Handlers => this.handlers;");
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