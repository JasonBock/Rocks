using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	/*
		public RockIMockable(Expectations<IMockable> expectations) => 
			this.handlers = expectations.CreateHandlers();

		public RockIMockable(Expectations<IMockable> expectations, int a)
			: base(a) => 
				this.handlers = expectations.CreateHandlers();
	*/

	internal static class MockConstructorBuilder
	{
		internal static void Build(IndentedTextWriter writer, ITypeSymbol typeToMock,
			ImmutableArray<IParameterSymbol> parameters)
		{
			var typeToMockName = typeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var instanceParameters = parameters.Length == 0 ?
				$"Expectations<{typeToMockName}> expectations" :
				string.Join(", ", $"Expectations<{typeToMockName}> expectations",
					string.Join(", ", 
						parameters.Select(_ => $"{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {_.Name}")));

			if(parameters.Length > 0)
			{
				writer.WriteLine($"public Rock{typeToMockName}({instanceParameters})");
				writer.Indent++;
				writer.WriteLine($": base({string.Join(", ", parameters.Select(_ => $"{_.Name}"))}) =>");
				writer.Indent++;
				writer.WriteLine("this.handlers = expectations.CreateHandlers();");
				writer.Indent--;
				writer.Indent--;
			}
			else
			{
				writer.WriteLine($"public Rock{typeToMockName}({instanceParameters}) =>");
				writer.Indent++;
				writer.WriteLine("this.handlers = expectations.CreateHandlers();");
				writer.Indent--;
			}
		}
	}
}