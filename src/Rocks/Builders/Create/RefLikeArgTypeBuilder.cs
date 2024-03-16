using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class RefLikeArgTypeBuilder
{
	internal static string GetProjectedFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var typeArguments = typeToMock.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		var argForType = type.RefLikeArgProjectedName;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{argForType}{typeArguments}";
	}

	private static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var typeArguments = typeToMock.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		var argForType = type.RefLikeArgProjectedEvaluationDelegateName;
		var typeArgumentsNamingContext = new VariableNamingContext(typeToMock.TypeArguments.ToImmutableHashSet());
		var argTypeArguments = type.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", type.TypeArguments.Select(_ => typeArgumentsNamingContext[_]))}>" : "";
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{argForType}{argTypeArguments}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var typeArgumentsNamingContext = new VariableNamingContext(typeModel.Type.TypeArguments.ToImmutableHashSet());
		var typeArguments = type.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", type.TypeArguments.Select(_ => typeArgumentsNamingContext[_]))}>" : "";

		var validationDelegateName = $"{type.RefLikeArgProjectedEvaluationDelegateName}{typeArguments}";
		var validationDelegateFullyQualifiedName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel.Type);
		var argName = $"{type.RefLikeArgProjectedName}{typeArguments}";
		var argConstructorName = type.RefLikeArgConstructorProjectedName;
		var typeName = $"{type.FullyQualifiedNameNoGenerics}{typeArguments}";

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