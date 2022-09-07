using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Text;

namespace Rocks.Builders.Create;

internal static class MockConstructorExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, Compilation compilation)
	{
		var requiredInitProperties = information.TypeToMock!.Type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
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
				writer.WriteLine(
					$"public {(requiredInitProperty.IsRequired ? "required " : string.Empty)}{requiredInitProperty.Type.GetReferenceableName()} {requiredInitProperty.Name} {{ get; init; }}");
				requiredInitObjectInitializationSyntaxBuilder.AppendLine(
					$"\t{requiredInitProperty.Name} = constructorProperties.{requiredInitProperty.Name},");
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
			$"this {WellKnownNames.Expectations}<{typeToMock.GenericName}> self{constructorPropertiesParameter}";
		var instanceParameters = parameters.Length == 0 ? selfParameter :
			string.Join(", ", selfParameter,
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

		writer.WriteLine($"internal {(isUnsafe ? "unsafe " : string.Empty)}static {typeToMock.GenericName} {WellKnownNames.Instance}({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("if (!self.WasInstanceInvoked)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("self.WasInstanceInvoked = true;");

		var newMock = $"new {nameof(Rock)}{typeToMock.FlattenedName}({rockInstanceParameters})";
		if(requiredInitObjectInitializationSyntax.Length == 0)
		{
			writer.WriteLine($"return {newMock};");
		}
		else
		{
			writer.WriteLine("return constructorProperties is null ?");
			writer.Indent++;

			if(hasRequiredProperties)
			{
				writer.WriteLine("throw new ArgumentNullException(nameof(constructorProperties)) :");
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
		writer.WriteLine("}");
		writer.WriteLine("else");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("throw new NewMockInstanceException(\"Can only create a new mock once.\");");
		writer.Indent--;
		writer.WriteLine("}");
		writer.Indent--;
		writer.WriteLine("}");
	}
}