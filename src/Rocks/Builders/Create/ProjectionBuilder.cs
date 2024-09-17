using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ProjectionBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeReferenceModel projectedModel)
	{
		if (projectedModel.PointedAtCount > 0)
		{
			if (projectedModel.PointedAt!.SpecialType == SpecialType.System_Void)
			{
				ProjectionBuilder.BuildVoidPointerArgument(writer, projectedModel);
			}
			else
			{
				ProjectionBuilder.BuildPointerArgument(writer, projectedModel);
			}
		}
		else
		{
			ProjectionBuilder.BuildSpecialArgument(writer, projectedModel);
		}
	}

	private static void BuildSpecialArgument(IndentedTextWriter writer, TypeReferenceModel projectedModel)
	{
		var needsUnsafe = projectedModel.TypeKind == TypeKind.FunctionPointer ?
			$"unsafe " : string.Empty;
		var typeName = projectedModel.FlattenedName;
		var fullyQualifiedName = projectedModel.FullyQualifiedName;

		writer.WriteLines(
			$$"""
			internal {{needsUnsafe}}delegate bool {{typeName}}ArgumentEvaluation({{fullyQualifiedName}} @value);

			internal sealed {{needsUnsafe}}class {{typeName}}Argument
				: Argument
			{
				private readonly {{typeName}}ArgumentEvaluation? evaluation;
				private readonly ValidationState validation;

				internal {{typeName}}Argument() => this.validation = ValidationState.None;

				internal {{typeName}}Argument({{typeName}}ArgumentEvaluation @evaluation)
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

	private static void BuildPointerArgument(IndentedTextWriter writer, TypeReferenceModel projectedModel)
	{
		var pointerNames = projectedModel.PointerNames!;
		var pointerSplats = new string('*', (int)projectedModel.PointedAtCount);

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

	private static void BuildVoidPointerArgument(IndentedTextWriter writer, TypeReferenceModel projectedModel)
	{
		var pointerNames = projectedModel.PointerNames!;
		var pointerSplats = new string('*', (int)projectedModel.PointedAtCount);

		writer.WriteLines(
			$$"""
			internal unsafe delegate bool {{pointerNames}}VoidArgumentEvaluation(void{{pointerSplats}} @value);

			internal sealed unsafe class {{pointerNames}}VoidArgument
				: Argument
			{
				private readonly {{pointerNames}}VoidArgumentEvaluation? evaluation;
				private readonly void{{pointerSplats}} value;
				private readonly ValidationState validation;

				internal {{pointerNames}}VoidArgument() => this.validation = ValidationState.None;

				internal {{pointerNames}}VoidArgument(void{{pointerSplats}} @value)
				{
					this.value = @value;
					this.validation = ValidationState.Value;
				}

				internal {{pointerNames}}VoidArgument({{pointerNames}}VoidArgumentEvaluation @evaluation)
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