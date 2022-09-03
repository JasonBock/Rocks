using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockConstructorExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Constructors.Length > 0)
		{
			foreach (var constructor in information.Constructors)
			{
				MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
					constructor.Parameters);
			}
		}
		else
		{
			MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
				ImmutableArray<IParameterSymbol>.Empty);
		}
	}

	private static void Build(IndentedTextWriter writer, MockedType typeToMock, ImmutableArray<IParameterSymbol> parameters)
	{
		var instanceParameters = parameters.Length == 0 ?
			$"this {WellKnownNames.Expectations}<{typeToMock.GenericName}> self" :
			string.Join(", ", $"this {WellKnownNames.Expectations}<{typeToMock.GenericName}> self",
				string.Join(", ", parameters.Select(_ =>
				{
					var direction = _.RefKind switch
					{
						RefKind.Ref => "ref ",
						RefKind.Out => "out ",
						RefKind.In => "in ",
						_ => string.Empty
					};
					return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}";
				})));
		var isUnsafe = false;
		var rockInstanceParameters = parameters.Length == 0 ? "self" :
			string.Join(", ", "self", string.Join(", ", parameters.Select(_ =>
			{
				isUnsafe |= _.Type.IsPointer();
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}{_.Name}";
			})));

		writer.WriteLines(
			$$"""
			internal {{(isUnsafe ? "unsafe " : string.Empty)}}static {{typeToMock.GenericName}} {{WellKnownNames.Instance}}({{instanceParameters}})
			{
				if (!self.WasInstanceInvoked)
				{
					self.WasInstanceInvoked = true;
					return new {{nameof(Rock)}}{{typeToMock.FlattenedName}}({{rockInstanceParameters}});
				}
				else
				{
					throw new NewMockInstanceException("Can only create a new mock once.");
				}
			}
			""");
	}
}