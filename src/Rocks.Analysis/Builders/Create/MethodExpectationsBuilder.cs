using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MethodExpectationsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type,
		List<MethodModel> methods,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		for (var i = 0; i < methods.Count; i++)
		{
			var method = methods[i];
			MethodExpectationsMethodBuilder.Build(writer, type, method, expectationsFullyQualifiedName, adornmentsFQNsPipeline);

			if (i != methods.Count - 1)
			{
				writer.WriteLine();
			}
		}
	}
}