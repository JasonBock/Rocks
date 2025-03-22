using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimBuilder
{
   internal static string GetShimName(ITypeReferenceModel shimType) => 
		$"Shim{shimType.FlattenedName}";

   internal static void Build(IndentedTextWriter writer, TypeMockModel shimType)
	{
		var shimName = ShimBuilder.GetShimName(shimType.Type);

		writer.WriteLines(
			$$"""
			private sealed class {{shimName}}
				: {{shimType.Type.FullyQualifiedName}}
			{
				private readonly Mock mock;
				
				public {{shimName}}(Mock @mock) =>
					this.mock = @mock;
			""");

		writer.Indent++;

		ShimMethodBuilder.Build(writer, shimType);
		ShimPropertyBuilder.Build(writer, shimType);
		ShimIndexerBuilder.Build(writer, shimType);
		ShimEventBuilder.Build(writer, shimType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}