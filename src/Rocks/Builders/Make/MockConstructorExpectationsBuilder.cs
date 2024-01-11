using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockConstructorExpectationsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType,
		string expectationsFullyQualifiedName)
	{
		var constructorProperties = mockType.ConstructorProperties;
		var enumerableTypes = constructorProperties.Select(_ =>
			_.Parameters.Length == 1 ?
				_.Parameters[0].Type.FullyQualifiedName :
				$"({string.Join(", ", _.Parameters.Select(p => p.Type.FullyQualifiedName))})").ToImmutableArray();
		var requiredInitProperties = constructorProperties.Where(_ => !_.IsIndexer).ToImmutableArray();
		var requiredInitIndexers = constructorProperties.Where(_ => _.IsIndexer).ToImmutableArray();
		var hasRequiredProperties = requiredInitProperties.Any(_ => _.IsRequired);

		if (constructorProperties.Length > 0)
		{
			// Generate the ConstructorProperties class
			// and generate the object initialization syntax that will be used
			// in the Instance() methods.
			writer.WriteLine("internal sealed class ConstructorProperties");

			if (requiredInitIndexers.Length > 0)
			{
				writer.Indent++;
				writer.WriteLine($": {string.Join(", ", enumerableTypes.Select(_ => $"global::System.Collections.Generic.IEnumerable<{_}>"))}");
				writer.Indent--;
			}

			writer.WriteLine("{");
			writer.Indent++;

			if (requiredInitIndexers.Length > 0)
			{
				for (var i = 0; i < requiredInitIndexers.Length; i++)
				{
					var indexer = requiredInitIndexers[i];
					var indexerKey = enumerableTypes[i];
					writer.WriteLine($"private readonly global::System.Collections.Generic.Dictionary<{indexerKey}, {indexer.Type.FullyQualifiedName}> i{i} = new();");
				}

				writer.WriteLine();

				for (var i = 0; i < requiredInitIndexers.Length; i++)
				{
					var indexer = requiredInitIndexers[i];
					var indexerKey = enumerableTypes[i];
					writer.WriteLines(
						$$"""
						global::System.Collections.Generic.IEnumerator<{{indexerKey}}> global::System.Collections.Generic.IEnumerable<{{indexerKey}}>.GetEnumerator()
						{
							foreach(var key in this.i{{i}}.Keys)
							{
								yield return key;
							}
						}
						""");
				}

				writer.WriteLine();
				writer.WriteLine($"global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() => throw new global::System.NotImplementedException();");
				writer.WriteLine();

				for (var i = 0; i < requiredInitIndexers.Length; i++)
				{
					var indexer = requiredInitIndexers[i];
					var indexerKey = indexer.Parameters.Length == 1 ?
						indexer.Parameters[0].Name :
						$"({string.Join(", ", indexer.Parameters.Select(p => p.Name))})";
					writer.WriteLines(
						$$"""
						internal {{indexer.Type.FullyQualifiedName}} this[{{string.Join(", ", indexer.Parameters.Select(p => $"{p.Type.FullyQualifiedName} {p.Name}"))}}]
						{
							get => this.i{{i}}[{{indexerKey}}];
							init => this.i{{i}}[{{indexerKey}}] = value;
						}
						""");
				}
			}

			if (requiredInitIndexers.Length > 0 && requiredInitIndexers.Length > 0)
			{
				writer.WriteLine();
			}

			foreach (var requiredInitProperty in requiredInitProperties)
			{
				hasRequiredProperties |= requiredInitProperty.IsRequired;
				var propertyNullability = (requiredInitProperty.NullableAnnotation == NullableAnnotation.None ||
					requiredInitProperty.NullableAnnotation == NullableAnnotation.NotAnnotated) && requiredInitProperty.Type.IsReferenceType ?
						"?" : string.Empty;
				var isRequired = requiredInitProperty.IsRequired ? "required " : string.Empty;
				var propertyTypeName = requiredInitProperty.Type.FullyQualifiedName;
				writer.WriteLine(
					$"internal {isRequired}{propertyTypeName}{propertyNullability} {requiredInitProperty.Name} {{ get; init; }}");
			}

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();
		}

		if (mockType.Constructors.Length > 0)
		{
			foreach (var constructor in mockType.Constructors)
			{
				MockConstructorExpectationsBuilder.Build(writer, mockType.Type,
					constructor.Parameters, constructorProperties.Length > 0, 
					hasRequiredProperties, expectationsFullyQualifiedName);
			}
		}
		else
		{
			MockConstructorExpectationsBuilder.Build(writer, mockType.Type,
				ImmutableArray<ParameterModel>.Empty, requiredInitProperties.Length > 0 || requiredInitIndexers.Length > 0, 
				hasRequiredProperties, expectationsFullyQualifiedName);
		}
	}

	private static void Build(IndentedTextWriter writer, TypeReferenceModel type, ImmutableArray<ParameterModel> parameters,
		bool requiredInitObjectInitialization, bool hasRequiredProperties, string expectationsFullyQualifiedName)
	{
		var namingContext = new VariableNamingContext(parameters);
		var constructorPropertiesParameter =
			requiredInitObjectInitialization ?
				[$"{expectationsFullyQualifiedName}.ConstructorProperties{(!hasRequiredProperties ? "?" : string.Empty)} @{namingContext["constructorProperties"]}"] :
				Array.Empty<string>();
		var instanceParameters = 
			$"{string.Join(", ", constructorPropertiesParameter.Concat(parameters.Select(_ =>
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
			})))}";
		var isUnsafe = false;
		var contextParameters = requiredInitObjectInitialization ?
			[$"@{namingContext["constructorProperties"]}"] :
			Array.Empty<string>();
		var rockInstanceParameters = string.Join(", ",
			contextParameters.Concat(parameters.Select(_ =>
			{
				isUnsafe |= _.Type.IsPointer;
				var requiresNullable = _.RequiresNullableAnnotation ? "!" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}{requiresNullable}";
			})));

		writer.WriteLine($"internal {(isUnsafe ? "unsafe " : string.Empty)}{type.FullyQualifiedName} Instance({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		if (hasRequiredProperties)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull({namingContext["constructorProperties"]});");
		}

		writer.WriteLine($"return new Mock({rockInstanceParameters});");
		writer.Indent--;
		writer.WriteLine("}");
	}
}