using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockEventExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, ImmutableHashSet<string> adornmentsFQNs)
	{
		writer.WriteLines(
			$$"""
			
			internal static class {{mockType.Type.FlattenedName}}AdornmentsEventExtensions
			{
			""");
		writer.Indent++;

		foreach (var (name, argsType) in mockType.Events.Select(_ => (_.Name, _.ArgsType)).Distinct())
		{
			foreach (var adornment in adornmentsFQNs.OrderBy(_ => _))
			{
				writer.WriteLines(
					$$"""
					internal static {{adornment}} Raise{{name}}(this {{adornment}} self, {{argsType}} args) => 
						self.AddRaiseEvent(new("{{name}}", args));
					""");
			}
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}