using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockConstructorExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, Compilation compilation)
	{
		var requiredInitProperties = information.TypeToMock!.Type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => !_.IsIndexer && (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly)).ToArray();
		var requiredInitIndexers = information.TypeToMock!.Type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => _.IsIndexer && (_.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly)).ToArray();
		var enumerableTypes = requiredInitIndexers.Select(_ =>
			_.Parameters.Length == 1 ?
				_.Parameters[0].Type.GetFullyQualifiedName() :
				$"({string.Join(", ", _.Parameters.Select(p => p.Type.GetFullyQualifiedName()))})").ToArray();

		var hasRequiredProperties = false;

		if (requiredInitProperties.Length > 0 || requiredInitIndexers.Length > 0)
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
					writer.WriteLine($"private readonly global::System.Collections.Generic.Dictionary<{indexerKey}, {indexer.Type.GetFullyQualifiedName()}> i{i} = new();");
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
						internal {{indexer.Type.GetFullyQualifiedName()}} this[{{string.Join(", ", indexer.Parameters.Select(p => $"{p.Type.GetFullyQualifiedName()} {p.Name}"))}}]
						{
						    get => this.i{{i}}[{{indexerKey}}];
						    init => this.i{{i}}[{{indexerKey}}] = value;
						}
						""");
				}
			}

			if (requiredInitIndexers.Length > 0 && requiredInitProperties.Length > 0)
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
				var propertyTypeName = requiredInitProperty.Type.GetFullyQualifiedName();
				writer.WriteLine(
					$"internal {isRequired}{propertyTypeName}{propertyNullability} {requiredInitProperty.Name} {{ get; init; }}");
			}

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();
		}

		if (information.Constructors.Length > 0)
		{
			foreach (var constructor in information.Constructors)
			{
				MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
					constructor.Parameters, requiredInitProperties.Length > 0 || requiredInitIndexers.Length > 0, hasRequiredProperties);
			}
		}
		else
		{
			MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
				ImmutableArray<IParameterSymbol>.Empty, requiredInitProperties.Length > 0 || requiredInitIndexers.Length > 0, hasRequiredProperties);
		}
	}

	private static void Build(IndentedTextWriter writer, MockedType typeToMock, ImmutableArray<IParameterSymbol> parameters, 
		bool requiredInitObjectInitialization, bool hasRequiredProperties)
	{
		var namingContext = new VariableNamingContext(parameters);
		var constructorPropertiesParameter =
			requiredInitObjectInitialization ?
				$", ConstructorProperties{(!hasRequiredProperties ? "?" : string.Empty)} @{namingContext["constructorProperties"]}" :
				string.Empty;
		var selfParameter =
			$"this global::Rocks.Expectations.Expectations<{typeToMock.ReferenceableName}> @{namingContext["self"]}{constructorPropertiesParameter}";
		var instanceParameters = parameters.Length == 0 ? selfParameter :
			string.Join(", ", selfParameter,
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
		var isUnsafe = false;
		var contextParameters = requiredInitObjectInitialization ?
			$"@{namingContext["self"]}, @{namingContext["constructorProperties"]}" :
			$"@{namingContext["self"]}";
		var rockInstanceParameters = parameters.Length == 0 ? contextParameters :
			string.Join(", ", contextParameters, string.Join(", ", parameters.Select(_ =>
			{
				isUnsafe |= _.Type.IsPointer();
				var requiresNullable = _.RequiresForcedNullableAnnotation() ? "!" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}{requiresNullable}";
			})));

		writer.WriteLine($"internal {(isUnsafe ? "unsafe " : string.Empty)}static {typeToMock.ReferenceableName} Instance({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		if (hasRequiredProperties)
		{
			writer.WriteLine($"if (@{namingContext["constructorProperties"]} is null)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($"throw new global::System.ArgumentNullException(nameof(@{namingContext["constructorProperties"]})) :");
			writer.Indent--;
			writer.WriteLine("}");
		}

		writer.WriteLine($"if (!@{namingContext["self"]}.WasInstanceInvoked)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"@{namingContext["self"]}.WasInstanceInvoked = true;");
		writer.WriteLine($"return new Rock{typeToMock.FlattenedName}({rockInstanceParameters});");
		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine("else");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("throw new global::Rocks.Exceptions.NewMockInstanceException(\"Can only create a new mock once.\");");
		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
	}
}