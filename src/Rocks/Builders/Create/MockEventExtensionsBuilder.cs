using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockEventExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, 
		ImmutableHashSet<(string adornmentFQN, string typeArguments, string constraints)> adornmentsFQNs)
	{
		writer.WriteLines(
			$$"""
			
			internal static class {{mockType.Type.FlattenedName}}AdornmentsEventExtensions
			{
			""");
		writer.Indent++;

		foreach (var (name, argsType) in mockType.Events.Select(_ => (_.Name, _.ArgsType)).Distinct())
		{
			foreach ((var adornmentFQN, var typeArguments, var constraints) in adornmentsFQNs.OrderBy(_ => _))
			{
				writer.WriteLines(
					$$"""
					internal static {{adornmentFQN}} Raise{{name}}{{typeArguments}}(this {{adornmentFQN}} self, {{argsType}} args){{constraints}} => 
						self.AddRaiseEvent(new("{{name}}", args));
					""");
			}
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}