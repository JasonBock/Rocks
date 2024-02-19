using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockEventExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFQN)
	{
		writer.WriteLines(
			$$"""
			internal static class {{mockType.Type.FlattenedName}}AdornmentsEventExtensions
			{
			""");
		writer.Indent++;

		var adornmentsIntermediateInterface = $"{expectationsFQN}.Adornments.IAdornmentsFor{mockType.Type.FlattenedName}<TAdornments>";

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