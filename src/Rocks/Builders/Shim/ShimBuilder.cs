using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimBuilder
{
   internal static string GetShimName(TypeReferenceModel shimType) => 
		$"Shim{shimType.FlattenedName}{shimType.FlattenedName.GetHash()}";

   internal static void Build(IndentedTextWriter writer, TypeMockModel shimType, string mockTypeName)
	{
		var shimName = ShimBuilder.GetShimName(shimType.Type);

		writer.WriteLines(
			$$"""
			private sealed class {{shimName}}
				: {{shimType.Type.FullyQualifiedName}}
			{
				private readonly {{mockTypeName}} mock;
				
				public {{shimName}}({{mockTypeName}} @mock) =>
					this.mock = @mock;
			""");

		writer.Indent++;

		ShimMethodBuilder.Build(writer, shimType);
		ShimPropertyBuilder.Build(writer, shimType);
		ShimIndexerBuilder.Build(writer, shimType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}