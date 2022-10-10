using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Shim;

internal static class ShimMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, Compilation compilation, MockInformation shimInformation)
	{
		foreach (var shimMethod in shimInformation.Methods.Results
			.Where(_ => _.Value.MethodKind == MethodKind.Ordinary && !_.Value.IsVirtual)
			.Select(_ => _.Value))
		{
			writer.WriteLine();

			var returnType = string.Empty;

			if (shimMethod.ReturnsVoid)
			{
				returnType = "void ";
			}
			else
			{
				var returnByRef = shimMethod.ReturnsByRef ? "ref " : shimMethod.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
				returnType = $"{returnByRef}{shimMethod.ReturnType.GetReferenceableName()}";
			}

			var methodParameters = string.Join(", ", shimMethod.Parameters.Select(_ =>
			{
				var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)}" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} @{_.Name}{defaultValue}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
			}));

			var attributes = shimMethod.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription(compilation));
			}

			var isUnsafe = shimMethod.IsUnsafe() ? "unsafe " : string.Empty;

			var constraints = shimMethod.GetConstraints();

			if (constraints.Length == 0)
			{
				writer.WriteLine($"public {isUnsafe}{returnType} {shimMethod.GetName()}({methodParameters}) =>");
				writer.Indent++;
			}
			else
			{
				writer.WriteLine($"public {isUnsafe}{returnType} {shimMethod.GetName()}({methodParameters})");
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
			writer.WriteLine($"(({shimInformation.TypeToMock!.Type.GetReferenceableName()})this.mock).{shimMethod.GetName()}({passedParameters});");
			writer.Indent--;
		}
	}
}