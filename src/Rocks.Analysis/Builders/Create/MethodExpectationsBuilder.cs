using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MethodExpectationsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type,
		MethodModel[] methods,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		for (var i = 0; i < methods.Length; i++)
		{
			var method = methods[i];
			MethodExpectationsMethodBuilder.Build(writer, type, method, expectationsFullyQualifiedName, adornmentsFQNsPipeline);

			if (i != methods.Length - 1)
			{
				writer.WriteLine();
			}
		}
	}
}