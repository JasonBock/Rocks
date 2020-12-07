using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders.Make
{
	internal static class MockMethodValueBuilder
	{
		internal static void Build(IndentedTextWriter writer, MethodMockableResult result)
		{
			var method = result.Value;
			var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
			var returnType = $"{returnByRef}{method.ReturnType.GetName()}";
			var parametersDescription = string.Join(", ", method.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}";
			}));
			var explicitTypeNameDescription = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
				$"{method.ContainingType.GetName(TypeNameOption.NoGenerics)}." : string.Empty;

			var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
			{
				var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue.GetDefaultValue()}" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}{defaultValue}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
			}));
			var methodSignature =
				$"{returnType} {explicitTypeNameDescription}{method.GetName()}({methodParameters})";

			var attributes = method.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			var returnAttributes = method.GetReturnTypeAttributes();

			if (returnAttributes.Length > 0)
			{
				writer.WriteLine(returnAttributes.GetDescription(AttributeTargets.ReturnValue));
			}

			var isPublic = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				"public " : string.Empty;
			writer.WriteLine($"{isPublic}{(result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

			var constraints = method.GetConstraints();

			if(result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ||
				result.RequiresOverride == RequiresOverride.Yes)
			{
				constraints = constraints.AddRange(method.GetDefaultConstraints());
			}

			if(constraints.Length > 0)
			{
				writer.Indent++;

				foreach(var constraint in constraints)
				{
					writer.WriteLine(constraint);
				}

				writer.Indent--;
			}

			writer.WriteLine("{");
			writer.Indent++;

			foreach(var outParameter in method.Parameters.Where(_ => _.RefKind == RefKind.Out))
			{
				writer.WriteLine($"{outParameter.Name} = default!;");
			}

			if (method.ReturnsByRef || method.ReturnsByRefReadonly)
			{
				writer.WriteLine($"return ref this.rr{result.MemberIdentifier};");
			}
			else
			{
				writer.WriteLine("return default!;");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}