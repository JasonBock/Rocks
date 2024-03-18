using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class RefLikeArgTypeBuilder
{
	internal static string GetProjectedFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var typeToMockArguments = typeToMock.IsOpenGeneric ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		var typeArguments = type.IsOpenGeneric ?
			$"<{string.Join(", ", type.TypeArguments)}>" : string.Empty;
		var argForType = type.RefLikeArgProjectedName;
		var @namespace = typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.";
		return $"global::{@namespace}{typeToMock.FlattenedName}CreateExpectations{typeToMockArguments}.Projections.{argForType}{typeArguments}";
	}

	private static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var typeArguments = typeToMock.IsOpenGeneric ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		var argForType = type.RefLikeArgProjectedEvaluationDelegateName;
		var typeArgumentsNamingContext = new TypeArgumentsNamingContext(typeToMock);
		var argTypeArguments = type.IsOpenGeneric ?
			$"<{string.Join(", ", type.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{argForType}{argTypeArguments}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var typeArgumentsNamingContext = new TypeArgumentsNamingContext(typeModel.Type);
		var typeArguments = type.IsGenericType ?
			$"<{string.Join(", ", type.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
		var typeParameters = !type.IsGenericType ? string.Empty : typeArguments;
		var validationDelegateName = $"{type.RefLikeArgProjectedEvaluationDelegateName}{(type.IsOpenGeneric ? typeArguments : string.Empty)}";
		var validationDelegateFullyQualifiedName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel.Type);
		var argName = $"{type.RefLikeArgProjectedName}{(type.IsOpenGeneric ? typeArguments : string.Empty)}";
		var argConstructorName = type.RefLikeArgConstructorProjectedName;
		var typeName = $"{type.FullyQualifiedNameNoGenerics}{typeParameters}{(type.NullableAnnotation == NullableAnnotation.Annotated ? "?" : string.Empty)}";

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