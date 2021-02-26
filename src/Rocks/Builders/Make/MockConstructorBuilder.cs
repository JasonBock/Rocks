using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Make
{
	internal static class MockConstructorBuilder
	{
		internal static void Build(IndentedTextWriter writer, ITypeSymbol typeToMock,
			ImmutableArray<IParameterSymbol> parameters)
		{
			var typeToMockName = typeToMock.GetName();

			if(parameters.Length > 0)
			{
				var instanceParameters = string.Join(", ", parameters.Select(_ => $"{_.Type.GetName()} {_.Name}"));
				writer.WriteLine($"public {nameof(Rock)}{typeToMock.GetName(TypeNameOption.Flatten)}({instanceParameters})");
				writer.Indent++;
				writer.WriteLine($": base({string.Join(", ", parameters.Select(_ => $"{_.Name}"))})");
				writer.Indent--;
				writer.WriteLine("{ }");
			}
			else
			{
				writer.WriteLine($"public {nameof(Rock)}{typeToMock.GetName(TypeNameOption.Flatten)}() {{ }}");
			}
		}
	}
}