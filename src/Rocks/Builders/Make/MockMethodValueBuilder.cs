using Microsoft.CodeAnalysis;
using Rocks.Exceptions;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockMethodValueBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodMockableResult result, SemanticModel model,
		NamespaceGatherer namespaces, Compilation compilation)
	{
		var method = result.Value;

		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);
		var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		var returnType = $"{returnByRef}{method.ReturnType.GetReferenceableName()}";
		var parametersDescription = string.Join(", ", method.Parameters.Select(_ =>
		{
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}";
		}));
		var explicitTypeNameDescription = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.GetName(TypeNameOption.IncludeGenerics)}." : string.Empty;

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
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}{defaultValue}";
			return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
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

		var constraints = method.GetConstraints();

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
			writer.WriteLine($"{outParameter.Name} = default!;");
		}

		var taskType = model.Compilation.GetTypeByMetadataName(typeof(Task).FullName);
		var taskOfTType = model.Compilation.GetTypeByMetadataName(typeof(Task<>).FullName);
		var valueTaskType = model.Compilation.GetTypeByMetadataName(typeof(ValueTask).FullName);
		var valueTaskOfTType = model.Compilation.GetTypeByMetadataName(typeof(ValueTask<>).FullName);

		if(shouldThrowDoesNotReturnException)
		{
			namespaces.Add(typeof(DoesNotReturnException));
			writer.WriteLine($"throw new {nameof(DoesNotReturnException)}();");
		}
		else
		{
			if (method.ReturnType.Equals(taskType))
			{
				namespaces.Add(taskType.ContainingNamespace);
				writer.WriteLine($"return {nameof(Task)}.{nameof(Task.CompletedTask)};");
			}
			else if (method.ReturnType.Equals(valueTaskType))
			{
				namespaces.Add(valueTaskType.ContainingNamespace);
				writer.WriteLine($"return new {nameof(ValueTask)}();");
			}
			else if (method.ReturnType.OriginalDefinition.Equals(taskOfTType))
			{
				namespaces.Add(taskOfTType.ContainingNamespace);
				writer.WriteLine($"return {nameof(Task)}.{nameof(Task.FromResult)}(default({(method.ReturnType as INamedTypeSymbol)!.TypeArguments[0].GetName()}));");
			}
			else if (method.ReturnType.OriginalDefinition.Equals(valueTaskOfTType))
			{
				namespaces.Add(valueTaskOfTType.ContainingNamespace);
				var typeArgument = (method.ReturnType as INamedTypeSymbol)!.TypeArguments[0];
				writer.WriteLine($"return new {nameof(ValueTask)}<{typeArgument.GetName()}>(default({typeArgument.GetName()})!);");
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