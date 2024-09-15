using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ProjectionBuilder
{
	internal static void Build(IndentedTextWriter writer, ProjectedModelInformation projectedModel)
	{
		if (projectedModel.PointerCount > 0)
		{
			ProjectionBuilder.BuildPointerArgument(writer, projectedModel);
		}
		else
		{
			ProjectionBuilder.BuildSpecialArgument(writer, projectedModel);
		}
	}

	private static void BuildSpecialArgument(IndentedTextWriter writer, ProjectedModelInformation projectedModel)
	{
		var referenceType = projectedModel.Type!;

		var needsUnsafe = referenceType.TypeKind == TypeKind.FunctionPointer ?
			$"unsafe " : string.Empty;
		var typeName = referenceType.FlattenedName;
		var fullyQualifiedName = referenceType.FullyQualifiedName;

		writer.WriteLines(
			$$"""
			internal {{needsUnsafe}}delegate bool {{typeName}}ArgumentEvaluation({{fullyQualifiedName}} @value);

			internal sealed {{needsUnsafe}}class {{typeName}}Argument
				: Argument
			{
				private readonly {{typeName}}ArgumentEvaluation? evaluation;
				private readonly {{fullyQualifiedName}} value;
				private readonly ValidationState validation;

				internal {{typeName}}Argument() => this.validation = ValidationState.None;

				internal {{typeName}}Argument({{fullyQualifiedName}} @value)
				{
					this.value = @value;
					this.validation = ValidationState.Value;
				}

				internal {{typeName}}Argument({{typeName}}ArgumentEvaluation @evaluation)
				{
					this.evaluation = @evaluation;
					this.validation = ValidationState.Evaluation;
				}

				public static implicit operator {{typeName}}Argument({{fullyQualifiedName}} @value) => new(@value);

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
			""");
	}

	private static void BuildPointerArgument(IndentedTextWriter writer, ProjectedModelInformation projectedModel)
	{
		var pointerNames = projectedModel.PointerNames!;
		var pointerSplats = new string('*', (int)projectedModel.PointerCount);

		writer.WriteLines(
			$$"""
			internal unsafe delegate bool {{pointerNames}}ArgumentEvaluation<T>(T{{pointerSplats}} @value) where T : unmanaged;

			internal sealed unsafe class {{pointerNames}}Argument<T>
				: Argument
				where T : unmanaged
			{
				private readonly {{pointerNames}}ArgumentEvaluation<T>? evaluation;
				private readonly T{{pointerSplats}} value;
				private readonly ValidationState validation;

				internal {{pointerNames}}Argument() => this.validation = ValidationState.None;

				internal {{pointerNames}}Argument(T{{pointerSplats}} @value)
				{
					this.value = @value;
					this.validation = ValidationState.Value;
				}

				internal {{pointerNames}}Argument({{pointerNames}}ArgumentEvaluation<T> @evaluation)
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
}