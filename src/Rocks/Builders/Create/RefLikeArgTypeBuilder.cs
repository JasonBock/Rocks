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

	internal static string GetProjectedFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ? 
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = RefLikeArgTypeBuilder.GetProjectedName(type);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedEvaluationDelegateName(ITypeSymbol type) =>
		$"ArgEvaluationFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";

	internal static string GetProjectedEvaluationDelegateFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ? 
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var validationDelegateName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateName(type);
		var validationDelegateFullyQualifiedName = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateFullyQualifiedName(type, typeToMock);
		var argName = RefLikeArgTypeBuilder.GetProjectedName(type);
		var argConstructorName = RefLikeArgTypeBuilder.GetProjectedConstructorName(type);
		var typeName = type.GetReferenceableName();

		writer.WriteLine($"public delegate bool {validationDelegateName}({typeName} value);");
		writer.WriteLine();
		writer.WriteLine($"public sealed class {argName}");
		writer.Indent++;
		writer.WriteLine(": global::Rocks.Argument");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"private readonly {validationDelegateFullyQualifiedName}? evaluation;");
		writer.WriteLine("private readonly global::Rocks.ValidationState validation;");
		writer.WriteLine();
		writer.WriteLine($"internal {argConstructorName}() => this.validation = global::Rocks.ValidationState.None;");
		writer.WriteLine();
		writer.WriteLine($"internal {argConstructorName}({validationDelegateFullyQualifiedName} evaluation)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("this.evaluation = evaluation;");
		writer.WriteLine($"this.validation = global::Rocks.ValidationState.Evaluation;");
		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();

		writer.WriteLine($"public bool IsValid({typeName} value) =>");
		writer.Indent++;
		writer.WriteLine("this.validation switch");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("global::Rocks.ValidationState.None => true,");
		writer.WriteLine("global::Rocks.ValidationState.Evaluation => this.evaluation!(value),");
		writer.WriteLine("_ => throw new global::System.NotSupportedException(\"Invalid validation state.\"),");
		writer.Indent--;
		writer.WriteLine("};");
		writer.Indent--;

		writer.Indent--;
		writer.WriteLine("}");
	}
}