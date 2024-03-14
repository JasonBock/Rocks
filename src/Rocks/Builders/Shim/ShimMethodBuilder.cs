using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Shim;

internal static class ShimMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel shimType)
	{
		foreach (var method in shimType.Methods
			.Where(_ => _.MethodKind == MethodKind.Ordinary && !_.IsVirtual)
			.Select(_ => _))
		{
			var typeArgumentNamingContext = method.IsGenericMethod ?
				new VariableNamingContext(shimType.Type.TypeArguments.ToImmutableHashSet()) :
				new VariableNamingContext();

			writer.WriteLine();

			var returnType = string.Empty;
			var useNullForgiving = string.Empty;

			if (method.ReturnsVoid)
			{
				returnType = "void ";
			}
			else
			{
				var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
				returnType = $"{returnByRef}{typeArgumentNamingContext[method.ReturnType.FullyQualifiedName]}";
				useNullForgiving = "!";
			}

			var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
			{
				var defaultValue = _.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
					$" = {_.ExplicitDefaultValue}" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{typeArgumentNamingContext[_.Type.FullyQualifiedName]} @{_.Name}{defaultValue}";
				return $"{(_.AttributesDescription.Length > 0 ? $"{_.AttributesDescription} " : string.Empty)}{parameter}";
			}));

			if (method.AttributesDescription.Length > 0)
			{
				writer.WriteLine(method.AttributesDescription);
			}

			var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;
			var constraints = method.Constraints;
			var (accessibility, explicitName) = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				("public ", string.Empty) : (string.Empty, $"{method.ContainingType.FullyQualifiedName}.");
			var typeArguments = method.TypeArguments.Length > 0 ?
				$"<{string.Join(", ", method.TypeArguments.Select(_ => typeArgumentNamingContext[_]))}>" : string.Empty;

			if (constraints.Length == 0)
			{
				writer.WriteLine($"{accessibility}{isUnsafe}{returnType} {explicitName}{method.Name}{typeArguments}({methodParameters}) =>");
				writer.Indent++;
			}
			else
			{
				writer.WriteLine($"{accessibility}{isUnsafe}{returnType} {explicitName}{method.Name}{typeArguments}({methodParameters})");
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

			var passedParameters = string.Join(", ", method.Parameters.Select(_ =>
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

			writer.WriteLine($"(({shimType.Type.FullyQualifiedName})this.mock).{method.Name}{typeArguments}({passedParameters}){useNullForgiving};");
			writer.Indent--;
		}
	}
}