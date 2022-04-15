using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimBuilder
{
	internal static void Build(IndentedTextWriter writer, ITypeSymbol shimType, string mockTypeName, 
		Compilation compilation)
	{
		var shimName = $"Shim{mockTypeName}";

		// TODO: If type is generic...what happens?
		writer.WriteLine($"private sealed class {shimName}");
		writer.Indent++;
		writer.WriteLine($": {shimType.GetName()}");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"private readonly {mockTypeName} mock;");
		writer.WriteLine();
		writer.WriteLine($"public {shimName}({mockTypeName} mock) =>");
		writer.Indent++;
		writer.WriteLine($"this.mock = mock;");
		writer.Indent--;

		ShimMethodBuilder.Build(writer, shimType, mockTypeName, compilation);
		ShimPropertyBuilder.Build(writer, shimType, compilation);
		ShimIndexerBuilder.Build(writer, shimType, compilation);

		writer.Indent--;
		writer.WriteLine("}");
	}
}