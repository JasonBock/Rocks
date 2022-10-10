using Microsoft.CodeAnalysis;
using Rocks.Builders.Shim;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockConstructorBuilder
{
	internal static void Build(IndentedTextWriter writer, MockedType typeToMock,
		ImmutableArray<IParameterSymbol> parameters, ImmutableArray<ITypeSymbol> shims)
	{
		var typeToMockName = typeToMock.ReferenceableName;
		var namingContext = new VariableNamingContext(parameters);
		var instanceParameters = parameters.Length == 0 ?
			$"global::Rocks.Expectations.Expectations<{typeToMockName}> @{namingContext["expectations"]}" :
			string.Join(", ", $"global::Rocks.Expectations.Expectations<{typeToMockName}> @{namingContext["expectations"]}",
				string.Join(", ", parameters.Select(_ =>
				{
					var requiresNullable = _.RequiresForcedNullableAnnotation() ? "?" : string.Empty;
					var direction = _.RefKind switch
					{
						RefKind.Ref => "ref ",
						RefKind.Out => "out ",
						RefKind.In => "in ",
						_ => string.Empty
					};
					return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()}{requiresNullable} @{_.Name}";
				})));

		var mockTypeName = $"Rock{typeToMock.FlattenedName}";

		if (parameters.Length > 0)
		{
			var passedParameter = string.Join(", ", parameters.Select(_ =>
			{
				var requiresNullable = _.RequiresForcedNullableAnnotation() ? "!" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}{requiresNullable}";
			}));
			var isUnsafe = parameters.Any(_ => _.Type.IsPointer()) ? "unsafe " : string.Empty;
			writer.WriteLine($"public {isUnsafe}{mockTypeName}({instanceParameters})");
			writer.Indent++;
			writer.WriteLine($": base({passedParameter}) =>");
			writer.Indent++;
			MockConstructorBuilder.BuildFieldSetters(writer, mockTypeName, namingContext, shims);
			writer.Indent--;
			writer.Indent--;
		}
		else
		{
			writer.WriteLine($"public {mockTypeName}({instanceParameters}) =>");
			writer.Indent++;
			MockConstructorBuilder.BuildFieldSetters(writer, mockTypeName, namingContext, shims);
			writer.Indent--;
		}
	}

	private static void BuildFieldSetters(IndentedTextWriter writer, string mockTypeName, 
		VariableNamingContext namingContext, ImmutableArray<ITypeSymbol> shims)
	{
		if (shims.Length == 0)
		{
			writer.WriteLine($"this.handlers = @{namingContext["expectations"]}.Handlers;");
		}
		else
		{
			var shimFields = string.Join(", ", shims.Select(_ => $"this.shimFor{_.GetName(TypeNameOption.Flatten)}"));
			var shimConstructors = string.Join(", ", shims.Select(_ => $"new {ShimBuilder.GetShimName(_)}(this)"));
			writer.WriteLine($"(this.handlers, {shimFields}) = (@{namingContext["expectations"]}.Handlers, {shimConstructors});");
		}
	}
}