using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Create
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
				$"this Expectations<{typeToMock.GetName()}> self" :
				string.Join(", ", $"this Expectations<{typeToMock.GetName()}> self",
					string.Join(", ", parameters.Select(_ => $"{_.Type.GetName()} {_.Name}")));
			var isUnsafe = false;
			var rockInstanceParameters = parameters.Length == 0 ? "self" :
				string.Join(", ", "self", string.Join(", ", parameters.Select(_ =>
				{
					isUnsafe |= _.Type.IsPointer();
					return $"{_.Name}";
				})));

			writer.WriteLine($"internal {(isUnsafe ? "unsafe " : string.Empty)}static {typeToMock.GetName(TypeNameOption.IncludeGenerics)} Instance({instanceParameters})");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"var mock = new Rock{typeToMock.GetName(TypeNameOption.Flatten)}({rockInstanceParameters});");
			writer.WriteLine("self.Mocks.Add(mock);");
			writer.WriteLine("return mock;");

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}