using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
			ImmutableArray<IParameterSymbol> parameters, SortedSet<string> namespaces)
		{
			var instanceParameters = string.Join(", ", $"Expectations<{typeToMock.Name}> expectations",
				string.Join(", ", parameters.Select(_ =>
					{
						if (!_.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
						{
							namespaces.Add($"using {_.Type.ContainingNamespace!.ToDisplayString()};");
						}

						return $"{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {_.Name}";
					})));
			var rockInstanceParameters = string.Join(", ", $"self", string.Join(", ", parameters.Select(_ => $"{_.Name}")));

			writer.WriteLine($"internal static {typeToMock.Name} Instance({instanceParameters})");
			writer.WriteLine($"var mock = new Rock{typeToMock.Name}({rockInstanceParameters});");
			writer.WriteLine("self.Mocks.Add(mock);");
			writer.WriteLine("return mock;");
		}
	}
}