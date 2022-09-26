using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Text;

namespace Rocks.Builders.Make;

internal static class MockConstructorExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, Compilation compilation)
	{
		var requiredInitProperties = information.TypeToMock!.Type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => !_.IsIndexer && (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly)).ToArray();

		var requiredInitObjectInitializationSyntax = string.Empty;
		var hasRequiredProperties = false;

		if (requiredInitProperties.Length > 0)
		{
			// Generate the ConstructorProperties class
			// and generate the object initialization syntax that will be used
			// in the Instance() methods.
			writer.WriteLine("public sealed class ConstructorProperties");
			writer.WriteLine("{");
			writer.Indent++;

			var requiredInitObjectInitializationSyntaxBuilder = new StringBuilder();
			requiredInitObjectInitializationSyntaxBuilder.AppendLine("{");

			foreach (var requiredInitProperty in requiredInitProperties)
			{
				hasRequiredProperties |= requiredInitProperty.IsRequired;
				var propertyNullability = (requiredInitProperty.NullableAnnotation == NullableAnnotation.None ||
					requiredInitProperty.NullableAnnotation == NullableAnnotation.NotAnnotated) && requiredInitProperty.Type.IsReferenceType ?
						"?" : string.Empty;
				var propertyAssignment = (requiredInitProperty.NullableAnnotation == NullableAnnotation.None ||
					requiredInitProperty.NullableAnnotation == NullableAnnotation.NotAnnotated) && requiredInitProperty.Type.IsReferenceType ?
						"!" : string.Empty;
				var isRequired = requiredInitProperty.IsRequired ? "required " : string.Empty;
				var propertyTypeName = requiredInitProperty.Type.GetReferenceableName();
				writer.WriteLine(
					$"public {isRequired}{propertyTypeName}{propertyNullability} {requiredInitProperty.Name} {{ get; init; }}");
				requiredInitObjectInitializationSyntaxBuilder.AppendLine(
					$"\t{requiredInitProperty.Name} = constructorProperties.{requiredInitProperty.Name}{propertyAssignment},");
			}

			requiredInitObjectInitializationSyntaxBuilder.Append("};");
			requiredInitObjectInitializationSyntax = requiredInitObjectInitializationSyntaxBuilder.ToString();

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();
		}

		if (information.Constructors.Length > 0)
		{
			foreach (var constructor in information.Constructors)
			{
				MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
					constructor.Parameters, requiredInitObjectInitializationSyntax, hasRequiredProperties);
			}
		}
		else
		{
			MockConstructorExtensionsBuilder.Build(writer, information.TypeToMock!,
				ImmutableArray<IParameterSymbol>.Empty, requiredInitObjectInitializationSyntax, hasRequiredProperties);
		}
	}

	private static void Build(IndentedTextWriter writer, MockedType typeToMock, ImmutableArray<IParameterSymbol> parameters,
		string requiredInitObjectInitializationSyntax, bool hasRequiredProperties)
	{
		var constructorPropertiesParameter =
			requiredInitObjectInitializationSyntax.Length > 0 ?
				$", ConstructorProperties{(!hasRequiredProperties ? "?" : string.Empty)} constructorProperties" :
				string.Empty;
		var selfParameter =
			$"this global::Rocks.MakeGeneration<{typeToMock.ReferenceableName}> self{constructorPropertiesParameter}";

		var instanceParameters = parameters.Length == 0 ? selfParameter :
			string.Join(", ", selfParameter,
				string.Join(", ", string.Join(", ", parameters.Select(_ =>
				{
					var direction = _.RefKind switch
					{
						RefKind.Ref => "ref ",
						RefKind.Out => "out ",
						RefKind.In => "in ",
						_ => string.Empty
					};
					return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}";
				}))));
		var isUnsafe = false;
		var rockInstanceParameters = string.Join(", ", string.Join(", ", parameters.Select(_ =>
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

		writer.WriteLine($"internal {(isUnsafe ? "unsafe " : string.Empty)}static {typeToMock.ReferenceableName} Instance({instanceParameters}) =>");
		writer.Indent++;

		var newMock = $"new Rock{typeToMock.FlattenedName}({rockInstanceParameters})";
		if (requiredInitObjectInitializationSyntax.Length == 0)
		{
			writer.WriteLine($"{newMock};");
		}
		else
		{
			writer.WriteLine("constructorProperties is null ?");
			writer.Indent++;

			if (hasRequiredProperties)
			{
				writer.WriteLine("throw new global::System.ArgumentNullException(nameof(constructorProperties)) :");
			}
			else
			{
				writer.WriteLine($"{newMock} :");
			}

			writer.WriteLine($"{newMock}");
			writer.WriteLines(requiredInitObjectInitializationSyntax);
			writer.Indent--;
		}

		writer.Indent--;
	}
}