using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockConstructorExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Constructors.Length > 0)
		{
			foreach (var constructor in information.Constructors)
			{
				MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
					constructor.Parameters);
			}
		}
		else
		{
			MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
				ImmutableArray<IParameterSymbol>.Empty);
		}
	}

	private static void Build(IndentedTextWriter writer, MockedType typeToMock, ImmutableArray<IParameterSymbol> parameters)
	{
		var instanceParameters = parameters.Length == 0 ?
			$"this {WellKnownNames.Expectations}<{typeToMock.GenericName}> self" :
			string.Join(", ", $"this {WellKnownNames.Expectations}<{typeToMock.GenericName}> self",
				string.Join(", ", parameters.Select(_ => $"{_.Type.GetName()} {_.Name}")));
		var isUnsafe = false;
		var rockInstanceParameters = parameters.Length == 0 ? "self" :
			string.Join(", ", "self", string.Join(", ", parameters.Select(_ =>
			{
				isUnsafe |= _.Type.IsPointer();
				return $"{_.Name}";
			})));

		writer.WriteLine($"internal {(isUnsafe ? "unsafe " : string.Empty)}static {typeToMock.GenericName} {WellKnownNames.Instance}({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine("if (self.Mock is null)");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"var mock = new {nameof(Rock)}{typeToMock.FlattenedName}({rockInstanceParameters});");
		writer.WriteLine("self.Mock = mock;");
		writer.WriteLine("return mock;");

		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine("else");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("throw new NewMockInstanceException(\"Can only create a new mock once.\");");
		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
	}
}