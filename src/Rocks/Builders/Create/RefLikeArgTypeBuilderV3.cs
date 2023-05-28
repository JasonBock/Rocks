using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class RefLikeArgTypeBuilderV3
{
	// TODO: Not sure this method is needed anymore...
	internal static string GetProjectedFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var containingNamespace = typeToMock.Namespace;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = type.RefLikeArgProjectedName;
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeMockModel typeModel)
	{
		var containingNamespace = typeModel.MockType.Namespace;
		var projectionsForNamespace = $"ProjectionsFor{typeModel.MockType.FlattenedName}";
		var argForType = type.RefLikeArgProjectedName;
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var validationDelegateName = type.RefLikeArgProjectedEvaluationDelegateName;
		var validationDelegateFullyQualifiedName = RefLikeArgTypeBuilderV3.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel);
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