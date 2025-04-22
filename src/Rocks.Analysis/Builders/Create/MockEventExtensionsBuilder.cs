using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockEventExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFQN)
	{
		// We can't generate this class if the mock type
		// is an open generic. A closed generic is fine.
		if (mockType.Events.Length > 0 && !mockType.Type.IsOpenGeneric)
		{
			var adornmentsPrefixName = mockType.AdornmentsFlattenedName;

			writer.WriteLines(
				$$"""
				
				internal static class {{adornmentsPrefixName}}AdornmentsEventExtensions
				{
				""");
			writer.Indent++;

			var adornmentsIntermediateInterface = $"{expectationsFQN}.Adornments.IAdornmentsFor{adornmentsPrefixName}<TAdornments>";

			foreach (var (name, argsType) in mockType.Events.Select(_ => (_.Name, _.ArgsType)).Distinct())
			{
				writer.WriteLines(
					$$"""
					internal static TAdornments Raise{{name}}<TAdornments>(this TAdornments self, {{argsType}} args) where TAdornments : {{adornmentsIntermediateInterface}} => 
						self.AddRaiseEvent(new("{{name}}", args));
					""");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}