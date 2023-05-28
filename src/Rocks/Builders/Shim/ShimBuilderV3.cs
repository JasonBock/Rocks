using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimBuilderV3
{
   internal static string GetShimName(TypeReferenceModel shimType) => 
		$"Shim{shimType.FlattenedName}{shimType.FlattenedName.GetHash()}";

   internal static void Build(IndentedTextWriter writer, TypeReferenceModel shimType, string mockTypeName,
		Compilation compilation, TypeMockModel mockType)
	{
		var shimName = ShimBuilderV3.GetShimName(shimType);

		writer.WriteLine($"private sealed class {shimName}");
		writer.Indent++;
		writer.WriteLine($": {shimType.FullyQualifiedName}");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"private readonly {mockTypeName} mock;");
		writer.WriteLine();
		writer.WriteLine($"public {shimName}({mockTypeName} @mock) =>");
		writer.Indent++;
		writer.WriteLine($"this.mock = @mock;");
		writer.Indent--;

		// TODO: Well....crud. I was reusing the fact that I could create a MockInformation
		// on the fly because I still had all the symbols at this point. Now, I don't :(.
		// I'll have to go back when I transform the shims when I do "new TypeModel"
		// and create all of them at that time.
		// Interesting side note: I don't look at the diagnostics from a shim mock...should I?
		var shimInformation = new MockInformation(shimType, compilation.Assembly, mockType.Model,
			mockType.ConfigurationValues, BuildType.Create);

		ShimMethodBuilderV3.Build(writer, compilation, shimInformation);
		ShimPropertyBuilderV3.Build(writer, compilation, shimInformation);
		ShimIndexerBuilderV3.Build(writer, compilation, shimInformation);

		writer.Indent--;
		writer.WriteLine("}");
	}
}