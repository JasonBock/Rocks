using Microsoft.CodeAnalysis;
using Rocks.Builders.Shim;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockConstructorBuilder
{
	internal static void Build(IndentedTextWriter writer, MockedType typeToMock, Compilation compilation,
		ImmutableArray<IParameterSymbol> parameters, ImmutableArray<ITypeSymbol> shims)
	{
		var typeToMockName = typeToMock.ReferenceableName;
		var namingContext = new VariableNamingContext(parameters);
		var requiredInitPropertiesAndIndexers = typeToMock.Type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly)).ToArray();
		var hasRequiredProperties = requiredInitPropertiesAndIndexers.Any(_ => _.IsRequired);

		var contextParameters = requiredInitPropertiesAndIndexers.Length == 0 ?
			$"global::Rocks.Expectations.Expectations<{typeToMockName}> @{namingContext["expectations"]}" :
			$"global::Rocks.Expectations.Expectations<{typeToMockName}> @{namingContext["expectations"]}, ConstructorProperties{(!hasRequiredProperties ? "?" : string.Empty)} @{namingContext["constructorProperties"]}";
		var instanceParameters = parameters.Length == 0 ?
			contextParameters :
			string.Join(", ", contextParameters,
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
					return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetFullyQualifiedName()}{requiresNullable} @{_.Name}";
				})));

		var mockTypeName = $"Rock{typeToMock.FlattenedName}";

		if (hasRequiredProperties)
		{
			writer.WriteLine("[global::System.Diagnostics.CodeAnalysis.SetsRequiredMembers]");
		}

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
			writer.WriteLine($": base({passedParameter})");
			writer.Indent--;
			writer.WriteLine("{");
			writer.Indent++;
			MockConstructorBuilder.BuildFieldSetters(writer, compilation, namingContext, shims, requiredInitPropertiesAndIndexers, hasRequiredProperties);
			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine($"public {mockTypeName}({instanceParameters})");
			writer.WriteLine("{");
			writer.Indent++;
			MockConstructorBuilder.BuildFieldSetters(writer, compilation, namingContext, shims, requiredInitPropertiesAndIndexers, hasRequiredProperties);
			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildFieldSetters(IndentedTextWriter writer, Compilation compilation,
		VariableNamingContext namingContext, ImmutableArray<ITypeSymbol> shims,
		IPropertySymbol[] requiredInitPropertiesAndIndexers, bool hasRequiredProperties)
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

		if(requiredInitPropertiesAndIndexers.Length > 0)
		{
			var initIndexers = requiredInitPropertiesAndIndexers.Where(
				_ => _.IsIndexer && (_.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
					_.CanBeSeenByContainingAssembly(compilation.Assembly)).ToArray();
			var enumerableTypes = initIndexers.Select(_ =>
				_.Parameters.Length == 1 ?
					_.Parameters[0].Type.GetFullyQualifiedName() :
					$"({string.Join(", ", _.Parameters.Select(p => p.Type.GetFullyQualifiedName()))})").ToArray();
			var forEachTypes = initIndexers.Select(_ =>
				_.Parameters.Length == 1 ?
					$"var {_.Parameters[0].Name}" :
					$"({string.Join(", ", _.Parameters.Select(p => $"var {p.Name}"))})").ToArray();
			var indexerNames = initIndexers.Select(_ => string.Join(", ", _.Parameters.Select(p => p.Name))).ToArray();

			if(!hasRequiredProperties)
			{
				writer.WriteLine($"if (@{namingContext["constructorProperties"]} is not null)");
				writer.WriteLine("{");
				writer.Indent++;
			}

			foreach (var requiredInitProperty in requiredInitPropertiesAndIndexers.Where(_ => !_.IsIndexer))
			{
				var propertyAssignment = (requiredInitProperty.NullableAnnotation == NullableAnnotation.None ||
					requiredInitProperty.NullableAnnotation == NullableAnnotation.NotAnnotated) && requiredInitProperty.Type.IsReferenceType ?
						"!" : string.Empty;
				writer.WriteLine($"this.{requiredInitProperty.Name} = @{namingContext["constructorProperties"]}.{requiredInitProperty.Name}{propertyAssignment};");
			}

			for (var i = 0; i < initIndexers.Length; i++)
			{
				var initIndexer = initIndexers[i];
				var enumerableType = enumerableTypes[i];
				var forEachType = forEachTypes[i];
				var indexerName = indexerNames[i];

				writer.WriteLines(
					$$"""
					foreach ({{forEachType}} in ((global::System.Collections.Generic.IEnumerable<{{enumerableType}}>)@{{namingContext["constructorProperties"]}}))
					{
						this[{{indexerName}}] = constructorProperties[{{indexerName}}];
					}
					""");
			}

			if (!hasRequiredProperties)
			{
				writer.Indent--;
				writer.WriteLine("}");
			}
		}
	}
}