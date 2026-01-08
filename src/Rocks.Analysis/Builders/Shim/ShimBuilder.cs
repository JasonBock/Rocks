using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Shim;

internal static class ShimBuilder
{
	internal static string GetShimName(ITypeReferenceModel shimType)
	{
		var name = $"Shim{shimType.FlattenedName}";

		if(shimType.TypeParameters.Length > 0)
		{
			name = $"{name}{string.Join("", shimType.TypeParameters.Select(_ => _.Name))}";
		}

		return name;
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel shimType)
	{
		var shimName = GetShimName(shimType.Type);

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