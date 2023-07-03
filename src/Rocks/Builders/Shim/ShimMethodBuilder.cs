using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel shimType)
	{
		foreach (var shimMethod in shimType.Methods
			.Where(_ => _.MethodKind == MethodKind.Ordinary && !_.IsVirtual)
			.Select(_ => _))
		{
			writer.WriteLine();

			var returnType = string.Empty;

			if (shimMethod.ReturnsVoid)
			{
				returnType = "void ";
			}
			else
			{
				var returnByRef = shimMethod.ReturnsByRef ? "ref " : shimMethod.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
				returnType = $"{returnByRef}{shimMethod.ReturnType.FullyQualifiedName}";
			}

			var methodParameters = string.Join(", ", shimMethod.Parameters.Select(_ =>
			{
				var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue}" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName} @{_.Name}{defaultValue}";
				return $"{(_.AttributesDescription.Length > 0 ? $"{_.AttributesDescription} " : string.Empty)}{parameter}";
			}));

			if (shimMethod.AttributesDescription.Length > 0)
			{
				writer.WriteLine(shimMethod.AttributesDescription);
			}

			var isUnsafe = shimMethod.IsUnsafe ? "unsafe " : string.Empty;

			var constraints = shimMethod.Constraints;

			if (constraints.Length == 0)
			{
				writer.WriteLine($"public {isUnsafe}{returnType} {shimMethod.Name}({methodParameters}) =>");
				writer.Indent++;
			}
			else
			{
				writer.WriteLine($"public {isUnsafe}{returnType} {shimMethod.Name}({methodParameters})");
				writer.Indent++;

				for (var i = 0; i < constraints.Length; i++)
				{
					var constraint = constraints[i];

					if (i == constraints.Length - 1)
					{
						writer.WriteLine($"{constraint} =>");
					}
					else
					{
						writer.WriteLine(constraint);
					}
				}
			}

			var passedParameters = string.Join(", ", shimMethod.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}";
			}));

			writer.WriteLine($"(({shimType.Type.FullyQualifiedName})this.mock).{shimMethod.Name}({passedParameters});");
			writer.Indent--;
		}
	}
}