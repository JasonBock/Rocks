using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.ComponentModel;

namespace Rocks.Builders.Create;

internal static class PointerArgTypeBuilder
{
	internal static string GetProjectedName(ITypeSymbol type) =>
		$"{nameof(Argument)}For{type.GetName(TypeNameOption.Flatten)}";

	private static string GetProjectedEvaluationDelegateName(ITypeSymbol type) =>
		$"{nameof(Argument)}EvaluationFor{type.GetName(TypeNameOption.Flatten)}";

	internal static void Build(IndentedTextWriter writer, ITypeSymbol type)
	{
		var validationDelegateName = PointerArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		var argName = PointerArgTypeBuilder.GetProjectedName(type);
		var typeName = type.GetName();

		writer.WriteLine($"public unsafe delegate bool {validationDelegateName}({typeName} value);");
		writer.WriteLine();
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
		writer.WriteLine($"internal {argName}() => this.validation = {nameof(ValidationState)}.{nameof(ValidationState.None)};");
		writer.WriteLine();
		writer.WriteLine($"internal {argName}({typeName} value)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("this.value = value;");
		writer.WriteLine($"this.validation = {nameof(ValidationState)}.{nameof(ValidationState.Value)};");
		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();
		writer.WriteLine($"internal {argName}({validationDelegateName} evaluation)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("this.evaluation = evaluation;");
		writer.WriteLine($"this.validation = {nameof(ValidationState)}.{nameof(ValidationState.Evaluation)};");
		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();

		if (type.Kind != SymbolKind.FunctionPointerType)
		{
			writer.WriteLine();
			writer.WriteLine($"public static implicit operator {argName}({typeName} value) => new(value);");
			writer.WriteLine();
		}

		writer.WriteLine($"public bool IsValid({typeName} value) =>");
		writer.Indent++;
		writer.WriteLine("this.validation switch");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.None)} => true,");

		if (type.Kind == SymbolKind.FunctionPointerType)
		{
			writer.WriteLine("#pragma warning disable CS8909");
		}

		writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.Value)} => value == this.value,");

		if (type.Kind == SymbolKind.FunctionPointerType)
		{
			writer.WriteLine("#pragma warning restore CS8909");
		}

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