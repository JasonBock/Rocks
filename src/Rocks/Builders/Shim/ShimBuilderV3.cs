using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimBuilderV3
{
   internal static string GetShimName(TypeReferenceModel shimType) => 
		$"Shim{shimType.FlattenedName}{shimType.FlattenedName.GetHash()}";

   internal static void Build(IndentedTextWriter writer, TypeMockModel shimType, string mockTypeName)
	{
		var shimName = ShimBuilderV3.GetShimName(shimType.Type);

		writer.WriteLine($"private sealed class {shimName}");
		writer.Indent++;
		writer.WriteLine($": {shimType.Type.FullyQualifiedName}");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"private readonly {mockTypeName} mock;");
		writer.WriteLine();
		writer.WriteLine($"public {shimName}({mockTypeName} @mock) =>");
		writer.Indent++;
		writer.WriteLine($"this.mock = @mock;");
		writer.Indent--;

		ShimMethodBuilderV3.Build(writer, shimType);
		ShimPropertyBuilderV3.Build(writer, shimType);
		ShimIndexerBuilderV3.Build(writer, shimType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}