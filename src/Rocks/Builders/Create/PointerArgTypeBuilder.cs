using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PointerArgTypeBuilder
{
	internal static string GetProjectedName(ITypeSymbol type) =>
		$"ArgumentFor{type.GetName(TypeNameOption.Flatten)}";

	internal static string GetProjectedFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			typeToMock.ContainingNamespace!.ToDisplayString() : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = PointerArgTypeBuilder.GetProjectedName(type);
		return $"global::{containingNamespace}.{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateName(ITypeSymbol type) =>
		$"ArgumentEvaluationFor{type.GetName(TypeNameOption.Flatten)}";

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			typeToMock.ContainingNamespace!.ToDisplayString() : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = PointerArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		return $"global::{containingNamespace}.{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, ITypeSymbol type)
	{
		var validationDelegateName = PointerArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		var argName = PointerArgTypeBuilder.GetProjectedName(type);
		var typeName = type.GetName();

		writer.WriteLine($"public unsafe delegate bool {validationDelegateName}({typeName} value);");
		writer.WriteLine();
		writer.WriteLine($"public unsafe sealed class {argName}");
		writer.Indent++;
		writer.WriteLine($": global::Rocks.Argument");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"private readonly {validationDelegateName}? evaluation;");
		writer.WriteLine($"private readonly {typeName} value;");
		writer.WriteLine($"private readonly global::Rocks.ValidationState validation;");
		writer.WriteLine();
		writer.WriteLine($"internal {argName}() => this.validation = global::Rocks.ValidationState.None;");
		writer.WriteLine();
		writer.WriteLine($"internal {argName}({typeName} value)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("this.value = value;");
		writer.WriteLine("this.validation = global::Rocks.ValidationState.Value;");
		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();
		writer.WriteLine($"internal {argName}({validationDelegateName} evaluation)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("this.evaluation = evaluation;");
		writer.WriteLine("this.validation = global::Rocks.ValidationState.Evaluation;");
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
		writer.WriteLine("global::Rocks.ValidationState.None => true,");

		if (type.Kind == SymbolKind.FunctionPointerType)
		{
			writer.WriteLine("#pragma warning disable CS8909");
		}

		writer.WriteLine("global::Rocks.ValidationState.Value => value == this.value,");

		if (type.Kind == SymbolKind.FunctionPointerType)
		{
			writer.WriteLine("#pragma warning restore CS8909");
		}

		writer.WriteLine("global::Rocks.ValidationState.Evaluation => this.evaluation!(value),");
		writer.WriteLine("global::Rocks.ValidationState.DefaultValue => throw new global::System.NotSupportedException(\"Cannot validate an argument value in the ValidationState.DefaultValue state.\"),");
		writer.WriteLine("_ => throw new global::System.ComponentModel.InvalidEnumArgumentException($\"Invalid value for validation: {{this.validation}}\")");
		writer.Indent--;
		writer.WriteLine("};");
		writer.Indent--;

		writer.Indent--;
		writer.WriteLine("}");
	}
}