using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, ITypeSymbol shimType, string mockTypeName,
		Compilation compilation)
	{
		foreach (var method in shimType.GetMembers().OfType<IMethodSymbol>()
			.Where(_ => _.MethodKind == MethodKind.Ordinary && !_.IsVirtual))
		{
			writer.WriteLine();

			var returnType = string.Empty;

			if (method.ReturnsVoid)
			{
				returnType = "void ";
			}
			else
			{
				var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
				returnType = $"{returnByRef}{method.ReturnType.GetName()}";
			}

			var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
			{
				var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)}" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}{defaultValue}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
			}));

			var attributes = method.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription(compilation));
			}

			var isUnsafe = method.IsUnsafe() ? "unsafe " : string.Empty;

			var constraints = method.GetConstraints();

			if (constraints.Length == 0)
			{
				writer.WriteLine($"public {isUnsafe}{returnType} {method.GetName()}({methodParameters}) =>");
				writer.Indent++;
			}
			else
			{
				writer.WriteLine($"public {isUnsafe}{returnType} {method.GetName()}({methodParameters})");
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
				return $"{direction}{_.Name}";
			}));
			writer.WriteLine($"this.mock.{method.GetName()}({passedParameters});");
		}
	}
}