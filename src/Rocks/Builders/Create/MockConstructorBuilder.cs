using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class MockConstructorBuilder
	{
		internal static void Build(IndentedTextWriter writer, ITypeSymbol typeToMock,
			ImmutableArray<IParameterSymbol> parameters)
		{
			var typeToMockName = typeToMock.GetName();
			var instanceParameters = parameters.Length == 0 ?
				$"{WellKnownNames.Expectations}<{typeToMockName}> expectations" :
				string.Join(", ", $"{WellKnownNames.Expectations}<{typeToMockName}> expectations",
					string.Join(", ", 
						parameters.Select(_ => $"{_.Type.GetName()} {_.Name}")));

			if(parameters.Length > 0)
			{
				var isUnsafe = parameters.Any(_ => _.Type.IsPointer()) ? "unsafe " : string.Empty;
				writer.WriteLine($"public {isUnsafe}{nameof(Rock)}{typeToMock.GetName(TypeNameOption.Flatten)}({instanceParameters})");
				writer.Indent++;
				writer.WriteLine($": base({string.Join(", ", parameters.Select(_ => $"{_.Name}"))}) =>");
				writer.Indent++;
				writer.WriteLine("this.handlers = expectations.Handlers;");
				writer.Indent--;
				writer.Indent--;
			}
			else
			{
				writer.WriteLine($"public {nameof(Rock)}{typeToMock.GetName(TypeNameOption.Flatten)}({instanceParameters}) =>");
				writer.Indent++;
				writer.WriteLine("this.handlers = expectations.Handlers;");
				writer.Indent--;
			}
		}
	}
}