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
		var argForType = type.RefLikeArgProjectedName;
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeMockModel typeModel)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeModel.Type.FlattenedName}";
		var argForType = type.RefLikeArgProjectedEvaluationDelegateName;
		return $"global::{(typeModel.Type.Namespace.Length == 0 ? "" : $"{typeModel.Type.Namespace}.")}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var validationDelegateName = type.RefLikeArgProjectedEvaluationDelegateName;
		var validationDelegateFullyQualifiedName = ProjectedArgTypeBuilderV4.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel);
		var argName = type.RefLikeArgProjectedName;
		var argConstructorName = type.RefLikeArgConstructorProjectedName;
		var typeName = type.FullyQualifiedName;
		var isUnsafe = type.IsPointer ? " unsafe" : string.Empty;

		writer.WriteLines(
			$$"""
			internal{{isUnsafe}} delegate bool {{validationDelegateName}}({{typeName}} @value);
			
			internal{{isUnsafe}} sealed class {{argName}}
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