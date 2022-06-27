using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

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
			$"this MakeGeneration<{typeToMock.GenericName}> self" :
			string.Join(", ", $"this MakeGeneration<{typeToMock.GenericName}> self",
				string.Join(", ", string.Join(", ", parameters.Select(_ =>
				{
					var direction = _.RefKind switch
					{
						RefKind.Ref => "ref ",
						RefKind.Out => "out ",
						RefKind.In => "in ",
						_ => string.Empty
					};
					return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}";
				}))));
		var isUnsafe = false;
		var rockInstanceParameters = string.Join(", ", string.Join(", ", parameters.Select(_ =>
		{
			isUnsafe |= _.Type.IsPointer();
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			return $"{direction}{_.Name}";
		})));

		writer.WriteLine($"internal {(isUnsafe ? "unsafe " : string.Empty)}static {typeToMock.GenericName} Instance({instanceParameters}) =>");
		writer.Indent++;
		writer.WriteLine($"new {nameof(Rock)}{typeToMock.FlattenedName}({rockInstanceParameters});");
		writer.Indent--;
	}
}