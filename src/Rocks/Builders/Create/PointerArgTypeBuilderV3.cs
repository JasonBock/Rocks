using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PointerArgTypeBuilderV3
{
	// TODO: Not sure this method is needed anymore...
	internal static string GetProjectedFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var containingNamespace = typeToMock.Namespace;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = type.PointerArgProjectedName;
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeMockModel typeModel)
	{
		var containingNamespace = typeModel.MockType.Namespace;
		var projectionsForNamespace = $"ProjectionsFor{typeModel.MockType.FlattenedName}";
		var argForType = type.PointerArgProjectedEvaluationDelegateName;
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var validationDelegateName = type.PointerArgProjectedEvaluationDelegateName;
		var validationDelegateFullyQualifiedName = PointerArgTypeBuilderV3.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel);
		var argName = type.PointerArgProjectedName;
		var typeName = type.FullyQualifiedName;

		writer.WriteLines(
			$$"""
			internal unsafe delegate bool {{validationDelegateName}}({{typeName}} @value);

			internal unsafe sealed class {{argName}}
				: global::Rocks.Argument
			{
				private readonly {{validationDelegateFullyQualifiedName}}? evaluation;
				private readonly {{typeName}} value;
				private readonly global::Rocks.ValidationState validation;

				internal {{argName}}() => this.validation = global::Rocks.ValidationState.None;

				internal {{argName}}({{typeName}} @value)
				{
					this.value = @value;
					this.validation = global::Rocks.ValidationState.Value;
				}

				internal {{argName}}({{validationDelegateFullyQualifiedName}} @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = global::Rocks.ValidationState.Evaluation;
				}
			""");

		if (type.Kind != SymbolKind.FunctionPointerType)
		{
			writer.WriteLines(
				$$"""

					public static implicit operator {{argName}}({{typeName}} @value) => new(@value);

				""");
		}

		writer.WriteLines(
			$$"""
				public bool IsValid({{typeName}} @value) =>
					this.validation switch
					{
						global::Rocks.ValidationState.None => true,
			""");

		if (type.Kind == SymbolKind.FunctionPointerType)
		{
			writer.WriteLine("			#pragma warning disable CS8909");
		}

		writer.WriteLine("			global::Rocks.ValidationState.Value => @value == this.value,");

		if (type.Kind == SymbolKind.FunctionPointerType)
		{
			writer.WriteLine("			#pragma warning restore CS8909");
		}

		writer.WriteLines(
			$$$"""
						global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
						global::Rocks.ValidationState.DefaultValue => throw new global::System.NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
						_ => throw new global::System.ComponentModel.InvalidEnumArgumentException($"Invalid value for validation: {{this.validation}}")
					};
			}
			""");
	}
}