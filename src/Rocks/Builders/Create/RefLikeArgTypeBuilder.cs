using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class RefLikeArgTypeBuilder
{
	private static string GetProjectedConstructorName(ITypeSymbol type) =>
		$"ArgFor{(type.IsOpenGeneric() ? type.GetName(TypeNameOption.NoGenerics) : type.GetName(TypeNameOption.Flatten))}";

	internal static string GetProjectedName(ITypeSymbol type) =>
		$"ArgFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";

	private static string GetProjectedEvaluationDelegateName(ITypeSymbol type) =>
		$"ArgEvaluationFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";

	internal static void Build(IndentedTextWriter writer, ITypeSymbol type)
	{
		var validationDelegateName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		var argName = RefLikeArgTypeBuilder.GetProjectedName(type);
		var argConstructorName = RefLikeArgTypeBuilder.GetProjectedConstructorName(type);
		var typeName = type.GetName();

		writer.WriteLine($"public delegate bool {validationDelegateName}({typeName} value);");
		writer.WriteLine();
		writer.WriteLine($"public sealed class {argName}");
		writer.Indent++;
		writer.WriteLine($": {nameof(Argument)}");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"private readonly {validationDelegateName}? evaluation;");
		writer.WriteLine($"private readonly {nameof(ValidationState)} validation;");
		writer.WriteLine();
		writer.WriteLine($"internal {argConstructorName}() => this.validation = {nameof(ValidationState)}.{nameof(ValidationState.None)};");
		writer.WriteLine();
		writer.WriteLine($"internal {argConstructorName}({validationDelegateName} evaluation)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("this.evaluation = evaluation;");
		writer.WriteLine($"this.validation = {nameof(ValidationState)}.{nameof(ValidationState.Evaluation)};");
		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();

		writer.WriteLine($"public bool IsValid({typeName} value) =>");
		writer.Indent++;
		writer.WriteLine("this.validation switch");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.None)} => true,");
		writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.Evaluation)} => this.evaluation!(value),");
		writer.WriteLine($"_ => throw new {nameof(NotSupportedException)}(\"Invalid validation state.\"),");
		writer.Indent--;
		writer.WriteLine("};");
		writer.Indent--;

		writer.Indent--;
		writer.WriteLine("}");
	}
}