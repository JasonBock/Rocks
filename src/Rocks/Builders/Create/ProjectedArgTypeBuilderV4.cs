using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ProjectedArgTypeBuilderV4
{
	// TODO: Not sure this method is needed anymore...
	internal static string GetProjectedFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = type.EsotericArgumentProjectedName;
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeMockModel typeModel)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeModel.Type.FlattenedName}";
		var argForType = type.EsotericArgumentProjectedEvaluationDelegateName;
		return $"global::{(typeModel.Type.Namespace.Length == 0 ? "" : $"{typeModel.Type.Namespace}.")}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var validationDelegateName = type.EsotericArgumentProjectedEvaluationDelegateName;
		var validationDelegateFullyQualifiedName = ProjectedArgTypeBuilderV4.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel);
		var argName = type.EsotericArgumentProjectedName;
		var argConstructorName = type.EsotericArgumentConstructorProjectedName;
		var typeName = type.PointerArgParameterType;
		var isUnsafe = type.IsPointer ? " unsafe" : string.Empty;
		var typeArgument = type.IsPointer && type.IsBasedOnTypeParameter ? $"<{typeName}>" : string.Empty;
		var constraint = type.IsPointer && type.IsBasedOnTypeParameter ? $" where {typeName} : unmanaged" : string.Empty;

		writer.WriteLines(
			$$"""
			internal{{isUnsafe}} delegate bool {{validationDelegateName}}{{typeArgument}}({{type.FullyQualifiedName}} @value){{constraint}};
			
			internal{{isUnsafe}} sealed class {{argName}}{{typeArgument}}
				: global::Rocks.Argument{{constraint}}
			{
				private readonly {{validationDelegateFullyQualifiedName}}{{typeArgument}}? evaluation;
				private readonly global::Rocks.ValidationState validation;
				
				internal {{argConstructorName}}() => this.validation = global::Rocks.ValidationState.None;
				
				internal {{argConstructorName}}({{validationDelegateFullyQualifiedName}}{{typeArgument}} @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = global::Rocks.ValidationState.Evaluation;
				}
				
				public bool IsValid({{type.FullyQualifiedName}} @value) =>
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