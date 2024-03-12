using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class RefLikeArgTypeBuilder
{
	internal static string GetProjectedFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var argForType = type.RefLikeArgProjectedName;
		var typeArguments = typeToMock.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var argForType = type.RefLikeArgProjectedEvaluationDelegateName;
		var typeArguments = typeToMock.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var validationDelegateName = type.RefLikeArgProjectedEvaluationDelegateName;
		var validationDelegateFullyQualifiedName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel.Type);
		var argName = type.RefLikeArgProjectedName;
		var argConstructorName = type.RefLikeArgConstructorProjectedName;
		var typeName = type.FullyQualifiedName;

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