using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockConstructorBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, 
		ImmutableArray<ParameterModel> parameters)
	{
		var namingContext = new VariablesNamingContext(parameters);
		var requiredInitPropertiesAndIndexers = mockType.ConstructorProperties;
		var hasRequiredProperties = requiredInitPropertiesAndIndexers.Any(_ => _.IsRequired);

		var contextParameters = requiredInitPropertiesAndIndexers.Length == 0 ?
			Array.Empty<string>() :
			[$"ConstructorProperties{(!hasRequiredProperties ? "?" : string.Empty)} @{namingContext["constructorProperties"]}"];
		var instanceParameters = 
			string.Join(", ", contextParameters.Concat(parameters.Select(_ =>
				{
					var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
					var direction = _.RefKind switch
					{
						RefKind.Ref => "ref ",
						RefKind.Out => "out ",
						RefKind.In => "in ",
						_ => string.Empty
					};
					return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name}";
				})));

		if (hasRequiredProperties)
		{
			writer.WriteLine("[global::System.Diagnostics.CodeAnalysis.SetsRequiredMembers]");
		}

		if (parameters.Length > 0)
		{
			var isUnsafe = parameters.Any(_ => _.Type.IsPointer) ? "unsafe " : string.Empty;

			writer.WriteLine($"public {isUnsafe}Mock({instanceParameters})");
			writer.Indent++;
			writer.WriteLine(@$": base({string.Join(", ", parameters.Select(_ =>
			{
				var requiresNullable = _.RequiresNullableAnnotation ? "!" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}{requiresNullable}";
			}))})");
			writer.Indent--;
			writer.WriteLine("{");
			writer.Indent++;
			MockConstructorBuilder.BuildFieldSetters(writer, namingContext, requiredInitPropertiesAndIndexers, hasRequiredProperties);
			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine($"public Mock({instanceParameters})");
			writer.WriteLine("{");
			writer.Indent++;
			MockConstructorBuilder.BuildFieldSetters(writer, namingContext, requiredInitPropertiesAndIndexers, hasRequiredProperties);
			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildFieldSetters(IndentedTextWriter writer, VariablesNamingContext namingContext,
		EquatableArray<ConstructorPropertyModel> requiredInitPropertiesAndIndexers, bool hasRequiredProperties)
	{
		if (requiredInitPropertiesAndIndexers.Length > 0)
		{
			var initIndexers = requiredInitPropertiesAndIndexers.Where(
				_ => _.IsIndexer && (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)).ToImmutableArray();
			var enumerableTypes = initIndexers.Select(_ =>
				_.Parameters.Length == 1 ?
					_.Parameters[0].Type.FullyQualifiedName :
					$"({string.Join(", ", _.Parameters.Select(p => p.Type.FullyQualifiedName))})").ToImmutableArray();
			var forEachTypes = initIndexers.Select(_ =>
				_.Parameters.Length == 1 ?
					$"var {_.Parameters[0].Name}" :
					$"({string.Join(", ", _.Parameters.Select(p => $"var {p.Name}"))})").ToImmutableArray();
			var indexerNames = initIndexers.Select(_ => string.Join(", ", _.Parameters.Select(p => p.Name))).ToImmutableArray();

			if (!hasRequiredProperties)
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
					foreach ({{forEachType}} in (global::System.Collections.Generic.IEnumerable<{{enumerableType}}>)@{{namingContext["constructorProperties"]}})
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