using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;

namespace Rocks.Builders.Create
{
	internal static class PointerArgTypeBuilder
	{
		internal static void Build(IndentedTextWriter writer, ITypeSymbol type)
		{
			var validationDelegateName = $"{type.GetName(TypeNameOption.Flatten)}Evaluation";
			var argName = $"ArgOf{type.GetName(TypeNameOption.Flatten)}";
			var typeName = type.GetName();

			writer.WriteLine($"public static unsafe delegate {validationDelegateName}({typeName} value);");
			writer.WriteLine();
			writer.WriteLine("[Serializable]");
			writer.WriteLine($"public sealed class {argName}");
			writer.Indent++;
			writer.WriteLine(": Arg");
			writer.Indent--;
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"private readonly {validationDelegateName}? evaluation;");
			writer.WriteLine($"private unsafe readonly {typeName} value;");
			writer.WriteLine("private readonly ValidationState validation;");
			writer.WriteLine();
			writer.WriteLine($"internal {argName}() => this.validation = ValidationState.None;");
			writer.WriteLine();
			writer.WriteLine($"internal {argName}(ValidationState state) => this.validation = state;");
			writer.WriteLine();
			writer.WriteLine($"internal unsafe {argName}({typeName} value)");
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

			writer.WriteLine($"public unsafe bool IsValid({typeName} value) =>");
			writer.Indent++;
			writer.WriteLine("this.validation switch");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.None)} => true,");
			writer.WriteLine($"{nameof(ValidationState)}.{nameof(ValidationState.Value)} => a == this.value,");
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