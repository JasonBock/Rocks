using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;

namespace Rocks.Builders.Create
{
	internal static class PointerArgTypeBuilder
	{
		internal static string GetProjectedName(ITypeSymbol type) =>
			$"{nameof(Argument)}For{type.GetName(TypeNameOption.Flatten)}";

		internal static string GetProjectedEvaluationDelegateName(ITypeSymbol type) =>
			$"{nameof(Argument)}EvaluationFor{type.GetName(TypeNameOption.Flatten)}";

		internal static void Build(IndentedTextWriter writer, ITypeSymbol type)
		{
			var validationDelegateName = PointerArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
			var argName = PointerArgTypeBuilder.GetProjectedName(type);
			var typeName = type.GetName();

			writer.WriteLine($"public unsafe delegate bool {validationDelegateName}({typeName} value);");
			writer.WriteLine();
			writer.WriteLine("[Serializable]");
			writer.WriteLine($"public unsafe sealed class {argName}");
			writer.Indent++;
			writer.WriteLine($": {nameof(Argument)}");
			writer.Indent--;
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"private readonly {validationDelegateName}? evaluation;");
			writer.WriteLine($"private readonly {typeName} value;");
			writer.WriteLine($"private readonly {nameof(ValidationState)} validation;");
			writer.WriteLine();
			writer.WriteLine($"private {argName}() => this.validation = {nameof(ValidationState)}.{nameof(ValidationState.None)};");
			writer.WriteLine();
			writer.WriteLine($"private {argName}({typeName} value)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine("this.value = value;");
			writer.WriteLine($"this.validation = {nameof(ValidationState)}.{nameof(ValidationState.Value)};");
			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();
			writer.WriteLine($"private {argName}({validationDelegateName} evaluation)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine("this.evaluation = evaluation;");
			writer.WriteLine($"this.validation = {nameof(ValidationState)}.{nameof(ValidationState.Evaluation)};");
			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();

			writer.WriteLine($"public static {argName} Any() => new();");
			writer.WriteLine();
			writer.WriteLine($"public static {argName} Is({typeName} value) => new(value);");
			writer.WriteLine();
			writer.WriteLine($"public static {argName} Validate({validationDelegateName} evaluation) => new(evaluation);");

			writer.WriteLine();
			writer.WriteLine($"public static implicit operator {argName}({typeName} value) => new(value);");
			writer.WriteLine();

			writer.WriteLine($"public bool IsValid({typeName} value) =>");
			writer.Indent++;
			writer.WriteLine("this.validation switch");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.None)} => true,");
			writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.Value)} => value == this.value,");
			writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.Evaluation)} => this.evaluation!(value),");
			writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.DefaultValue)} => throw new {nameof(NotSupportedException)}(\"Cannot validate an argument value in the {nameof(ValidationState.DefaultValue)} state.\"),");
			writer.WriteLine($"_ => throw new {nameof(InvalidEnumArgumentException)}($\"Invalid value for validation: {{this.validation}}\")");
			writer.Indent--;
			writer.WriteLine("};");
			writer.Indent--;

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}