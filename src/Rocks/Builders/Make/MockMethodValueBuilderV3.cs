using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockMethodValueBuilderV3
{
	internal static void Build(IndentedTextWriter writer, MethodModel method)
	{
		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn;
		var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
		var returnType = $"{returnByRef}{method.ReturnType.FullyQualifiedName}";
		var explicitTypeNameDescription = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.FullyQualifiedName}." : string.Empty;

		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
			var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name}{defaultValue}";
			var attributes = _.AttributesDescription;
			return $"{(attributes.Length > 0 ? $"{attributes} " : string.Empty)}{parameter}";
		}));
		var methodSignature = $"{returnType} {explicitTypeNameDescription}{method.Name}({methodParameters})";

		if (method.AttributesDescription.Length > 0)
		{
			writer.WriteLine(method.AttributesDescription);
		}

		if (method.ReturnTypeAttributesDescription.Length > 0)
		{
			writer.WriteLine(method.ReturnTypeAttributesDescription);
		}

		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;
		var isPublic = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{method.OverridingCodeValue} " : string.Empty;
		writer.WriteLine($"{isPublic}{isUnsafe}{(method.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

		var constraints = ImmutableArray<string>.Empty;

		if (method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			method.ContainingType.TypeKind == TypeKind.Interface)
		{
			constraints = method.Constraints;
		}

		if (method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ||
			method.RequiresOverride == RequiresOverride.Yes)
		{
			constraints = constraints.AddRange(method.DefaultConstraints);
		}

		if (constraints.Length > 0)
		{
			writer.Indent++;

			foreach (var constraint in constraints)
			{
				writer.WriteLine(constraint);
			}

			writer.Indent--;
		}

		writer.WriteLine("{");
		writer.Indent++;

		foreach (var outParameter in method.Parameters.Where(_ => _.RefKind == RefKind.Out))
		{
			writer.WriteLine($"@{outParameter.Name} = default!;");
		}

		if(shouldThrowDoesNotReturnException)
		{
			writer.WriteLine("throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
		else
		{
			if (method.ReturnTypeIsTaskType)
			{
				writer.WriteLine("return global::System.Threading.Tasks.Task.CompletedTask;");
			}
			else if (method.ReturnTypeIsValueTaskType)
			{
				writer.WriteLine("return new global::System.Threading.Tasks.ValueTask();");
			}
			else if (method.ReturnTypeIsTaskOfTType)
			{
				var isNullForgiving = method.ReturnTypeIsTaskOfTTypeAndIsNullForgiving ? string.Empty : "!";
				writer.WriteLine($"return global::System.Threading.Tasks.Task.FromResult(default({method.ReturnTypeTypeArguments[0].FullyQualifiedName}){isNullForgiving});");
			}
			else if (method.ReturnTypeIsValueTaskOfTType)
			{
				var isNullForgiving = method.ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving ? string.Empty : "!";
				writer.WriteLine($"return new global::System.Threading.Tasks.ValueTask<{method.ReturnTypeTypeArguments[0].FullyQualifiedName}>(default({method.ReturnTypeTypeArguments[0].IncludeGenericsName}){isNullForgiving});");
			}
			else
			{
				if (method.ReturnsByRef || method.ReturnsByRefReadOnly)
				{
					writer.WriteLine($"return ref this.rr{method.MemberIdentifier};");
				}
				else
				{
					writer.WriteLine("return default!;");
				}
			}
		}
		writer.Indent--;
		writer.WriteLine("}");
	}
}