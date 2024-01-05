using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PointerArgTypeBuilderV4
{
	// TODO: Not sure this method is needed anymore...
	internal static string GetProjectedFullyQualifiedName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = type.PointerArgProjectedName;
		var parameterType = type.PointerArgParameterType is not null ? $"<{type.PointerArgParameterType}>" : null;
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{projectionsForNamespace}.{argForType}{parameterType}";
	}

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(TypeReferenceModel type, TypeMockModel typeModel)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeModel.Type.FlattenedName}";
		var argForType = type.PointerArgProjectedEvaluationDelegateName;
		var parameterType = type.PointerArgParameterType is not null ? $"<{type.PointerArgParameterType}>" : null;
		return $"global::{(typeModel.Type.Namespace.Length == 0 ? "" : $"{typeModel.Type.Namespace}.")}{projectionsForNamespace}.{argForType}{parameterType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeReferenceModel type, TypeMockModel typeModel)
	{
		var validationDelegateName = type.PointerArgProjectedEvaluationDelegateName;
		var validationDelegateFullyQualifiedName = PointerArgTypeBuilderV4.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeModel);
		var argName = type.PointerArgProjectedName;
		var typeName = type.FullyQualifiedName;
		var parameterType = type.PointerArgParameterType is not null ? 
			$"<{type.PointerArgParameterType}>" : null;
		var unmanagedConstraint = type.PointerArgParameterType is not null ?
			$" where {type.PointerArgParameterType} : unmanaged" : null;

		writer.WriteLines(
			$$"""
			internal unsafe delegate bool {{validationDelegateName}}{{parameterType}}({{typeName}} @value){{unmanagedConstraint}};
			
			internal unsafe sealed class {{argName}}{{parameterType}}
				: global::Rocks.Argument{{unmanagedConstraint}}
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
					public static implicit operator {{argName}}{{parameterType}}({{typeName}} @value) => new(@value);
					
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