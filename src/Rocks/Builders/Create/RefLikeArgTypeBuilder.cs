using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;

namespace Rocks.Builders.Create
{
	internal static class RefLikeArgTypeBuilder
	{
		internal static string GetProjectedName(ITypeSymbol type) =>
			$"ArgFor{type.GetName(TypeNameOption.Flatten)}";

		internal static string GetProjectedEvaluationDelegateName(ITypeSymbol type) =>
			$"ArgFor{type.GetName(TypeNameOption.Flatten)}Evaluation";

		internal static void Build(IndentedTextWriter writer, ITypeSymbol type)
		{
			var validationDelegateName = RefLikeArgTypeBuilder.GetProjectedName(type);
			var argName = RefLikeArgTypeBuilder.GetProjectedName(type);
			var typeName = type.GetName();

			writer.WriteLine($"public delegate {typeName} {validationDelegateName}({typeName} value);");
			writer.WriteLine();
			writer.WriteLine("[Serializable]");
			writer.WriteLine($"public sealed class {argName}");
			writer.Indent++;
			writer.WriteLine(": Arg");
			writer.Indent--;
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"private readonly {validationDelegateName}? evaluation;");
			writer.WriteLine($"private readonly {typeName} value;");
			writer.WriteLine("private readonly ValidationState validation;");
			writer.WriteLine();
			writer.WriteLine($"internal {argName}() => this.validation = {nameof(ValidationState)}.{nameof(ValidationState.None)};");
			writer.WriteLine();
			writer.WriteLine($"internal {argName}({nameof(ValidationState)} state) => this.validation = state;");
			writer.WriteLine();
			writer.WriteLine($"internal {argName}({validationDelegateName} evaluation)");
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