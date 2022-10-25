using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockMethodValueBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodMockableResult result, SemanticModel model, Compilation compilation)
	{
		var method = result.Value;

		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);
		var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		var returnType = $"{returnByRef}{method.ReturnType.GetFullyQualifiedName()}";
		var explicitTypeNameDescription = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.GetFullyQualifiedName()}." : string.Empty;

		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var requiresNullable = _.RequiresForcedNullableAnnotation() ? "?" : string.Empty;
			var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue.GetDefaultValue(_.Type)}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetFullyQualifiedName()}{requiresNullable} @{_.Name}{defaultValue}";
			var attributes = _.GetAttributes().GetDescription(compilation);
			return $"{(attributes.Length > 0 ? $"{attributes} " : string.Empty)}{parameter}";
		}));
		var methodSignature =
			$"{returnType} {explicitTypeNameDescription}{method.GetName()}({methodParameters})";

		var attributes = method.GetAttributes();

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var returnAttributes = method.GetReturnTypeAttributes();

		if (returnAttributes.Length > 0)
		{
			writer.WriteLine(returnAttributes.GetDescription(compilation, AttributeTargets.ReturnValue));
		}

		var isUnsafe = method.IsUnsafe() ? "unsafe " : string.Empty;
		var isPublic = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
		writer.WriteLine($"{isPublic}{isUnsafe}{(result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

		var constraints = ImmutableArray<string>.Empty;

		if (result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			method.ContainingType.TypeKind == TypeKind.Interface)
		{
			constraints = method.GetConstraints();
		}

		if (result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ||
			result.RequiresOverride == RequiresOverride.Yes)
		{
			constraints = constraints.AddRange(method.GetDefaultConstraints());
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

		var taskType = model.Compilation.GetTypeByMetadataName(typeof(Task).FullName);
		var taskOfTType = model.Compilation.GetTypeByMetadataName(typeof(Task<>).FullName);
		var valueTaskType = model.Compilation.GetTypeByMetadataName(typeof(ValueTask).FullName);
		var valueTaskOfTType = model.Compilation.GetTypeByMetadataName(typeof(ValueTask<>).FullName);

		if(shouldThrowDoesNotReturnException)
		{
			writer.WriteLine("throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
		else
		{
			if (method.ReturnType.Equals(taskType))
			{
				writer.WriteLine("return global::System.Threading.Tasks.Task.CompletedTask;");
			}
			else if (method.ReturnType.Equals(valueTaskType))
			{
				writer.WriteLine("return new global::System.Threading.Tasks.ValueTask();");
			}
			else if (method.ReturnType.OriginalDefinition.Equals(taskOfTType))
			{
				var taskReturnType = (method.ReturnType as INamedTypeSymbol)!;
				var isNullForgiving = taskReturnType.TypeArgumentNullableAnnotations[0] == NullableAnnotation.Annotated ? string.Empty : "!";
				writer.WriteLine($"return global::System.Threading.Tasks.Task.FromResult(default({taskReturnType.TypeArguments[0].GetFullyQualifiedName()}){isNullForgiving});");
			}
			else if (method.ReturnType.OriginalDefinition.Equals(valueTaskOfTType))
			{
				var taskReturnType = (method.ReturnType as INamedTypeSymbol)!;
				var isNullForgiving = taskReturnType.TypeArgumentNullableAnnotations[0] == NullableAnnotation.Annotated ? string.Empty : "!";
				writer.WriteLine($"return new global::System.Threading.Tasks.ValueTask<{taskReturnType.TypeArguments[0].GetFullyQualifiedName()}>(default({taskReturnType.TypeArguments[0].GetName()}){isNullForgiving});");
			}
			else
			{
				if (method.ReturnsByRef || method.ReturnsByRefReadonly)
				{
					writer.WriteLine($"return ref this.rr{result.MemberIdentifier};");
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