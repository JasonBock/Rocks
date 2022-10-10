using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimBuilder
{
	internal static string GetShimName(ITypeSymbol shimType)
	{
		var shimName = shimType.GetName(TypeNameOption.Flatten);
		return $"Shim{shimName}{shimName.GetHash()}";
	}

	internal static void Build(IndentedTextWriter writer, ITypeSymbol shimType, string mockTypeName,
		Compilation compilation, MockInformation typeToMockInformation)
	{
		var shimName = ShimBuilder.GetShimName(shimType);

		writer.WriteLine($"private sealed class {shimName}");
		writer.Indent++;
		writer.WriteLine($": {shimType.GetReferenceableName()}");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"private readonly {mockTypeName} mock;");
		writer.WriteLine();
		writer.WriteLine($"public {shimName}({mockTypeName} @mock) =>");
		writer.Indent++;
		writer.WriteLine($"this.mock = @mock;");
		writer.Indent--;

		var shimInformation = new MockInformation(shimType, compilation.Assembly, typeToMockInformation.Model,
			typeToMockInformation.ConfigurationValues, BuildType.Create);

		ShimMethodBuilder.Build(writer, compilation, shimInformation);
		ShimPropertyBuilder.Build(writer, compilation, shimInformation);
		ShimIndexerBuilder.Build(writer, compilation, shimInformation);

		writer.Indent--;
		writer.WriteLine("}");
	}
}