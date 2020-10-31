using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.Builders
{
	/*
	
	Example: If we are given a method like: Foo(int a, string b)

	internal static MethodAdornments Foo(this MethodExpectations<IMockable> self, Arg<int> a, Arg<string> b) =>
		new MethodAdornments(self.Add(0, new Dictionary<string, Arg>
		{
			{ "a", a },
			{ "b", b },
		}));
	*/

	internal static class MethodExpectationsExtensionsMethodBuilder
	{
		internal static void Build(IndentedTextWriter writer, MethodMockableResult result, SortedSet<string> namespaces, ref uint memberIdentifier)
		{
			var method = result.Value;
			var parameters = string.Join(", ", new[]
			{
				$"this {nameof(MethodExpectations<string>)}<{method.ContainingType.Name}> self",
				string.Join(", ", method.Parameters.Select(_ =>
				{
					if(!_.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
					{
						namespaces.Add($"using {_.Type.ContainingNamespace!.ToDisplayString()};");
					}

					return $"Arg<{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> {_.Name}";
				}))
			});

			var (returnValue, newAdornments) = method.ReturnsVoid ? 
				(nameof(MethodAdornments), $"new {nameof(MethodAdornments)}") : 
				($"{nameof(MethodAdornments)}<{method.ReturnType.Name}>", $"new {nameof(MethodAdornments)}<{method.ReturnType.Name}>");

			writer.WriteLine($"internal static {returnValue} {method.Name}({parameters}) =>");
			writer.Indent++;

			if(method.Parameters.Length == 0)
			{
				writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new Dictionary<string, Arg>()));");
			}
			else
			{
				writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new Dictionary<string, Arg>");
				writer.WriteLine("{");
				writer.Indent++;

				foreach(var parameter in method.Parameters)
				{
					writer.WriteLine($"{{ \"{parameter.Name}\", {parameter.Name} }},");
				}

				writer.Indent--;
				writer.WriteLine("}));");
			}

			writer.Indent--;
		}
	}
}