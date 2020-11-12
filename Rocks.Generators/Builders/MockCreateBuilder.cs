using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks.Builders
{
	internal static class MockCreateBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information, SortedSet<string> usings)
		{
			var typeToMockName = information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

			writer.WriteLine($"private sealed class Rock{typeToMockName}");
			writer.Indent++;
			writer.WriteLine($": {typeToMockName}, {(information.Events.Length > 0 ? nameof(IMockWithEvents) : nameof(IMock))}");
			writer.Indent--;

			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("private readonly ImmutableDictionary<int, ImmutableArray<HandlerInformation>> handlers;");
			writer.WriteLine();

			if (information.Constructors.Length > 0)
			{
				foreach (var constructor in information.Constructors)
				{
					MockConstructorBuilder.Build(writer, information.TypeToMock, constructor.Parameters);
				}
			}
			else
			{
				MockConstructorBuilder.Build(writer, information.TypeToMock, ImmutableArray<IParameterSymbol>.Empty);
			}

			writer.WriteLine();

			var memberIdentifier = 0u;

			foreach(var method in information.Methods)
			{
				if(method.Value.ReturnsVoid)
				{
					MockMethodVoidBuilder.Build(writer, method, information.Events.Length > 0);
				}
				else
				{
					MockMethodValueBuilder.Build(writer, method, information.Events.Length > 0);
				}

				memberIdentifier++;
			}

			foreach(var property in information.Properties)
			{
				MockPropertyBuilder.Build(writer, property, information.Events.Length > 0);
			}

			if (information.Events.Length > 0)
			{
				writer.WriteLine();
				MockEventsBuilder.Build(writer, information.Events, usings);
			}

			writer.WriteLine();
			writer.WriteLine("ImmutableDictionary<int, ImmutableArray<HandlerInformation>> IMock.Handlers => this.handlers;");
			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}