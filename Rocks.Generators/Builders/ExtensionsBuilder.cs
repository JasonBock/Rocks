using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;

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

		internal static void Build(IndentedTextWriter writer, MockInformation information, SortedSet<string> namespaces, ref uint memberIdentifier)
		{
			writer.WriteLine($"internal static class ExpectationsOf{information.TypeToMock.Name}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			if(information.Constructors.Length > 0)
			{
				foreach(var constructor in information.Constructors)
				{
					ExpectationsExtensionsConstructorBuilder.Build(writer, information.TypeToMock,
						constructor.Parameters, namespaces);
				}
			}
			else
			{
				ExpectationsExtensionsConstructorBuilder.Build(writer, information.TypeToMock,
					ImmutableArray<IParameterSymbol>.Empty, namespaces);
			}

			MockCreateBuilder.Build(writer, information);

			writer.Indent--;
			writer.WriteLine("}");

			if (information.Methods.Length > 0)
			{
				writer.WriteLine();
				MethodExpectationsExtensionsBuilder.Build(writer, information, namespaces, ref memberIdentifier);
			}
		}
	}
}