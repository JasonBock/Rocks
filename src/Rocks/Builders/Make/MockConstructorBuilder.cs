using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockConstructorBuilder
{
	internal static void Build(IndentedTextWriter writer, MockedType typeToMock,
		ImmutableArray<IParameterSymbol> parameters)
	{
		var typeToMockName = typeToMock.GenericName;

		if (parameters.Length > 0)
		{
			var instanceParameters = string.Join(", ", parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}";
			}));
			var isUnsafe = parameters.Any(_ => _.Type.IsPointer()) ? "unsafe " : string.Empty;
			writer.WriteLine($"public {isUnsafe}{nameof(Rock)}{typeToMock.FlattenedName}({instanceParameters})");
			writer.Indent++;
			writer.WriteLine(@$": base({string.Join(", ", parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}{_.Name}";
			}))})");
			writer.Indent--;
			writer.WriteLine("{ }");
		}
		else
		{
			writer.WriteLine($"public {nameof(Rock)}{typeToMock.FlattenedName}() {{ }}");
		}
	}
}