using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class RefLikeArgTypeBuilder
{
	private static string GetProjectedConstructorName(ITypeSymbol type) =>
		$"ArgFor{(type.IsOpenGeneric() ? type.GetName(TypeNameOption.NoGenerics) : type.GetName(TypeNameOption.Flatten))}";

	internal static string GetProjectedName(ITypeSymbol type) =>
		$"ArgFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";

	internal static string GetProjectedFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ? 
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = RefLikeArgTypeBuilder.GetProjectedName(type);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateName(ITypeSymbol type) =>
		$"ArgEvaluationFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ? 
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var validationDelegateName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		var validationDelegateFullyQualifiedName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeToMock);
		var argName = RefLikeArgTypeBuilder.GetProjectedName(type);
		var argConstructorName = RefLikeArgTypeBuilder.GetProjectedConstructorName(type);
		var typeName = type.GetReferenceableName();

		writer.WriteLines(
			$$"""
			internal delegate bool {{validationDelegateName}}({{typeName}} @value);
			
			internal sealed class {{argName}}
				: global::Rocks.Argument
			{
				private readonly {{validationDelegateFullyQualifiedName}}? evaluation;
				private readonly global::Rocks.ValidationState validation;
				
				internal {{argConstructorName}}() => this.validation = global::Rocks.ValidationState.None;
				
				internal {{argConstructorName}}({{validationDelegateFullyQualifiedName}} @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = global::Rocks.ValidationState.Evaluation;
				}
				
				public bool IsValid({{typeName}} @value) =>
					this.validation switch
					{
						global::Rocks.ValidationState.None => true,
						global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
						_ => throw new global::System.NotSupportedException("Invalid validation state."),
					};
			}
			""");
	}
}