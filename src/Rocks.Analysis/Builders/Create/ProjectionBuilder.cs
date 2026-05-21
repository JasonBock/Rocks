using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class ProjectionBuilder
{
	internal static string BuildArgument(
		ITypeReferenceModel type, TypeArgumentsNamingContext typeArgumentsNamingContext, bool requiresNullableAnnotation)
	{
		string argumentTypeName;

		if (type.IsPointer)
		{
			if (type.TypeKind == TypeKind.FunctionPointer)
			{
				argumentTypeName = $"global::Rocks.Projections.ArgumentFor{type.FlattenedName}";
			}
			else if (type.PointedAt!.SpecialType == SpecialType.System_Void)
			{
				argumentTypeName = $"global::Rocks.Projections.{type.PointerNames!}VoidArgument";
			}
			else
			{
				argumentTypeName = $"global::Rocks.Projections.{type.PointerNames!}Argument<{type.PointedAt!.BuildName(typeArgumentsNamingContext)}>";
			}
		}
		else
		{
			var nullableAnnotation = requiresNullableAnnotation ? "?" : string.Empty;

			argumentTypeName = type.RequiresProjectedArgument ?
				$"global::Rocks.Projections.{type.Name}Argument" :
				type.IsRefLikeType || type.AllowsRefLikeType ?
					$"global::Rocks.RefStructArgument<{type.BuildName(typeArgumentsNamingContext)}{nullableAnnotation}>" :
					$"global::Rocks.Argument<{type.BuildName(typeArgumentsNamingContext)}{nullableAnnotation}>";
		}

		return argumentTypeName;
	}

	internal static void Build(IndentedTextWriter writer, ITypeReferenceModel projectedModel, string accessibility)
	{
		if (projectedModel.TypeKind == TypeKind.FunctionPointer)
		{
			BuildFunctionPointerArgument(writer, projectedModel, accessibility);
		}
		else if (projectedModel.PointedAtCount > 0)
		{
			if (projectedModel.PointedAt!.SpecialType == SpecialType.System_Void)
			{
				BuildVoidPointerArgument(writer, projectedModel, accessibility);
			}
			else
			{
				BuildPointerArgument(writer, projectedModel, accessibility);
			}
		}
		else
		{
			BuildSpecialArgument(writer, projectedModel, accessibility);
		}
	}

	private static void BuildFunctionPointerArgument(IndentedTextWriter writer, ITypeReferenceModel projectedModel, string accessibility)
	{
		var flattenedName = projectedModel.FlattenedName;
		var fullyQualifiedName = projectedModel.FullyQualifiedName;

		writer.WriteLines(
			$$"""
			#pragma warning disable CS8909

			{{accessibility}} unsafe delegate bool ArgumentEvaluationFor{{flattenedName}}({{fullyQualifiedName}} @value);

			{{accessibility}} sealed unsafe class ArgumentFor{{flattenedName}}
				: Argument
			{
				private readonly ArgumentEvaluationFor{{flattenedName}}? evaluation;
				private readonly {{fullyQualifiedName}} value;
				private readonly ValidationState validation;

				{{accessibility}} ArgumentFor{{flattenedName}}() => this.validation = ValidationState.None;

				{{accessibility}} ArgumentFor{{flattenedName}}({{fullyQualifiedName}} @value)
				{
					this.value = @value;
					this.validation = ValidationState.Value;
				}

				{{accessibility}} ArgumentFor{{flattenedName}}(ArgumentEvaluationFor{{flattenedName}} @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = ValidationState.Evaluation;
				}

				public static implicit operator ArgumentFor{{flattenedName}}({{fullyQualifiedName}} @value) => new(@value);

				public bool IsValid({{fullyQualifiedName}} @value) =>
					this.validation switch
					{
						ValidationState.None => true,
						ValidationState.Value => @value == this.value,
						ValidationState.Evaluation => this.evaluation!(@value),
						ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
						_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
					};
			}

			#pragma warning restore CS8909
			""");
	}

	private static void BuildSpecialArgument(IndentedTextWriter writer, ITypeReferenceModel projectedModel, string accessibility)
	{
		var needsUnsafe = projectedModel.TypeKind == TypeKind.FunctionPointer ?
			$"unsafe " : string.Empty;
		var typeName = projectedModel.FlattenedName;
		var fullyQualifiedName = projectedModel.FullyQualifiedName;

		writer.WriteLines(
			$$"""
			{{accessibility}} {{needsUnsafe}}delegate bool {{typeName}}ArgumentEvaluation({{fullyQualifiedName}} @value);

			{{accessibility}} sealed {{needsUnsafe}}class {{typeName}}Argument
				: Argument
			{
				private readonly {{typeName}}ArgumentEvaluation? evaluation;
				private readonly ValidationState validation;

				{{accessibility}} {{typeName}}Argument() => this.validation = ValidationState.None;

				{{accessibility}} {{typeName}}Argument({{typeName}}ArgumentEvaluation @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = ValidationState.Evaluation;
				}

				public bool IsValid({{fullyQualifiedName}} @value) =>
					this.validation switch
					{
						ValidationState.None => true,
						ValidationState.Evaluation => this.evaluation!(@value),
						ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
						_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
					};
			}
			""");
	}

	private static void BuildPointerArgument(IndentedTextWriter writer, ITypeReferenceModel projectedModel, string accessibility)
	{
		var pointerNames = projectedModel.PointerNames!;
		var pointerSplats = new string('*', (int)projectedModel.PointedAtCount);

		writer.WriteLines(
			$$"""
			{{accessibility}} unsafe delegate bool {{pointerNames}}ArgumentEvaluation<T>(T{{pointerSplats}} @value) where T : unmanaged;

			{{accessibility}} sealed unsafe class {{pointerNames}}Argument<T>
				: Argument
				where T : unmanaged
			{
				private readonly {{pointerNames}}ArgumentEvaluation<T>? evaluation;
				private readonly T{{pointerSplats}} value;
				private readonly ValidationState validation;

				{{accessibility}} {{pointerNames}}Argument() => this.validation = ValidationState.None;

				{{accessibility}} {{pointerNames}}Argument(T{{pointerSplats}} @value)
				{
					this.value = @value;
					this.validation = ValidationState.Value;
				}

				{{accessibility}} {{pointerNames}}Argument({{pointerNames}}ArgumentEvaluation<T> @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = ValidationState.Evaluation;
				}

				public static implicit operator {{pointerNames}}Argument<T>(T{{pointerSplats}} @value) => new(@value);

				public bool IsValid(T{{pointerSplats}} @value) =>
					this.validation switch
					{
						ValidationState.None => true,
						ValidationState.Value => @value == this.value,
						ValidationState.Evaluation => this.evaluation!(@value),
						ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
						_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
					};
			}
			""");
	}

	private static void BuildVoidPointerArgument(IndentedTextWriter writer, ITypeReferenceModel projectedModel, string accessibility)
	{
		var pointerNames = projectedModel.PointerNames!;
		var pointerSplats = new string('*', (int)projectedModel.PointedAtCount);

		writer.WriteLines(
			$$"""
			{{accessibility}} unsafe delegate bool {{pointerNames}}VoidArgumentEvaluation(void{{pointerSplats}} @value);

			{{accessibility}} sealed unsafe class {{pointerNames}}VoidArgument
				: Argument
			{
				private readonly {{pointerNames}}VoidArgumentEvaluation? evaluation;
				private readonly void{{pointerSplats}} value;
				private readonly ValidationState validation;

				{{accessibility}} {{pointerNames}}VoidArgument() => this.validation = ValidationState.None;

				{{accessibility}} {{pointerNames}}VoidArgument(void{{pointerSplats}} @value)
				{
					this.value = @value;
					this.validation = ValidationState.Value;
				}

				{{accessibility}} {{pointerNames}}VoidArgument({{pointerNames}}VoidArgumentEvaluation @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = ValidationState.Evaluation;
				}

				public static implicit operator {{pointerNames}}VoidArgument(void{{pointerSplats}} @value) => new(@value);

				public bool IsValid(void{{pointerSplats}} @value) =>
					this.validation switch
					{
						ValidationState.None => true,
						ValidationState.Value => @value == this.value,
						ValidationState.Evaluation => this.evaluation!(@value),
						ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
						_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
					};
			}
			""");
	}
}