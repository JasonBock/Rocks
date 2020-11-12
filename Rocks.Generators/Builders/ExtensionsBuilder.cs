using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	internal static class ExtensionsBuilder
	{
		/*
		internal static class ExpectationsOfIMockableExtensions
		{
			// Any member extension methods, like...
			internal static MethodExpectations<IMockable> Methods(this Expectations<IMockable> self) =>
				new MethodExpectations<IMockable>(self);

			// Constructors ...

			// Mock type ...
		}

		// Any member extension classes ...
		*/

		internal static void Build(IndentedTextWriter writer, MockInformation information, SortedSet<string> usings)
		{
			var typeToMockName = information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

			writer.WriteLine($"internal static class ExpectationsOf{typeToMockName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"internal static MethodExpectations<{typeToMockName}> Methods(this Expectations<{typeToMockName}> self) =>");
			writer.Indent++;
			writer.WriteLine($"new MethodExpectations<{typeToMockName}>(self);");
			writer.Indent--;
			writer.WriteLine();

			if (information.Constructors.Length > 0)
			{
				foreach(var constructor in information.Constructors)
				{
					ExpectationsExtensionsConstructorBuilder.Build(writer, information.TypeToMock,
						constructor.Parameters, usings);
				}
			}
			else
			{
				ExpectationsExtensionsConstructorBuilder.Build(writer, information.TypeToMock,
					ImmutableArray<IParameterSymbol>.Empty, usings);
			}

			writer.WriteLine();

			MockCreateBuilder.Build(writer, information, usings);

			writer.Indent--;
			writer.WriteLine("}");

			if (information.Methods.Length > 0)
			{
				writer.WriteLine();
				MethodExpectationsExtensionsBuilder.Build(writer, information, usings);
			}

			if(information.Events.Length > 0)
			{
				writer.WriteLine();
				EventExpectationsExtensionsBuilder.Build(writer, information);
			}
		}
	}
}