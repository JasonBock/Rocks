using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockConstructorBuilder
	{
		internal static void Build(IndentedTextWriter writer, ITypeSymbol typeToMock,
			ImmutableArray<IParameterSymbol> parameters)
		{
			var typeToMockName = typeToMock.GetName();
			var instanceParameters = parameters.Length == 0 ?
				$"Expectations<{typeToMockName}> expectations" :
				string.Join(", ", $"Expectations<{typeToMockName}> expectations",
					string.Join(", ", 
						parameters.Select(_ => $"{_.Type.GetName()} {_.Name}")));

			if(parameters.Length > 0)
			{
				writer.WriteLine($"public Rock{typeToMock.GetName(TypeNameOption.FlattenGenerics)}({instanceParameters})");
				writer.Indent++;
				writer.WriteLine($": base({string.Join(", ", parameters.Select(_ => $"{_.Name}"))}) =>");
				writer.Indent++;
				writer.WriteLine("this.handlers = expectations.CreateHandlers();");
				writer.Indent--;
				writer.Indent--;
			}
			else
			{
				writer.WriteLine($"public Rock{typeToMock.GetName(TypeNameOption.FlattenGenerics)}({instanceParameters}) =>");
				writer.Indent++;
				writer.WriteLine("this.handlers = expectations.CreateHandlers();");
				writer.Indent--;
			}
		}
	}
}