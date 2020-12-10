using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Make
{
	internal static class MockConstructorExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Constructors.Length > 0)
			{
				foreach (var constructor in information.Constructors)
				{
					MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock,
						constructor.Parameters);
				}
			}
			else
			{
				MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock,
					ImmutableArray<IParameterSymbol>.Empty);
			}
		}

		private static void Build(IndentedTextWriter writer, ITypeSymbol typeToMock, ImmutableArray<IParameterSymbol> parameters)
		{
			var instanceParameters = parameters.Length == 0 ?
				$"this MakeGeneration<{typeToMock.GetName()}> self" :
				string.Join(", ", $"this MakeGeneration<{typeToMock.GetName()}> self",
					string.Join(", ", parameters.Select(_ => $"{_.Type.GetName()} {_.Name}")));
			var rockInstanceParameters = string.Join(", ", parameters.Select(_ => $"{_.Name}"));

			writer.WriteLine($"internal static {typeToMock.GetName(TypeNameOption.IncludeGenerics)} Instance({instanceParameters}) =>");
			writer.Indent++;
			writer.WriteLine($"new Rock{typeToMock.GetName(TypeNameOption.FlattenGenerics)}({rockInstanceParameters});");
			writer.Indent--;
		}
	}
}