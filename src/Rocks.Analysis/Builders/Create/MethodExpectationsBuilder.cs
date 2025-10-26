using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MethodExpectationsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, 
		List<MethodModel> methods,
		string expectationsFullyQualifiedName, string expectationsSource, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		foreach (var method in methods)
		{
			MethodExpectationsMethodBuilder.Build(writer, type, method, expectationsFullyQualifiedName, expectationsSource, adornmentsFQNsPipeline);
			writer.WriteLine();
		}
	}
}