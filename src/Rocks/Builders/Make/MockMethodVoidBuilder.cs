using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockMethodVoidBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodMockableResult result, Compilation compilation)
	{
		var method = result.Value;

		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);
		var explicitTypeNameDescription = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.GetName(TypeNameOption.IncludeGenerics)}." : string.Empty;

		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var requiresNullable = _.RequiresForcedNullableAnnotation() ? "?" : string.Empty;
			var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()}{requiresNullable} @{_.Name}{defaultValue}";
			return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
		}));
		var methodSignature =
			$"void {explicitTypeNameDescription}{method.GetName()}({methodParameters})";

		var attributes = method.GetAttributes();

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
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

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine("throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}