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
			var typeArgumentsNamingContext = method.IsGenericMethod ?
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
				var returnTypeValue = method.IsGenericMethod && method.TypeArguments.Contains(method.ReturnType.FullyQualifiedName) ?
					typeArgumentsNamingContext[method.ReturnType.FullyQualifiedName] : method.ReturnType.FullyQualifiedName;
				returnType = $"{returnByRef}{returnTypeValue}";
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
				var typeName = method.IsGenericMethod && method.TypeArguments.Contains(_.Type.FullyQualifiedName) ?
					typeArgumentsNamingContext[_.Type.FullyQualifiedName] : _.Type.FullyQualifiedName;
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{typeName} @{_.Name}{defaultValue}";
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
			var typeArguments = method.IsGenericMethod ?
				$"<{string.Join(", ", method.TypeArguments.Select(_ => !method.MockType.TypeArguments.Contains(_) ? _ : typeArgumentsNamingContext[_]))}>" : string.Empty;

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
						writer.WriteLine($"{constraint.ToString(typeArgumentsNamingContext, method)} =>");
					}
					else
					{
						writer.WriteLine(constraint.ToString(typeArgumentsNamingContext, method));
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