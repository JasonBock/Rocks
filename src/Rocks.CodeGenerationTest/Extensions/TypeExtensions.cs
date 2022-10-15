using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Rocks.Builders;
using Rocks.Configuration;
using System.Reflection;
using System.Runtime.Versioning;

namespace Rocks.CodeGenerationTest.Extensions
{
	internal static class TypeExtensions
	{
		internal static string GetTypeDefinition(this Type self,
			Dictionary<Type, Dictionary<string, string>>? genericTypeMappings)
		{
			if (self.IsGenericTypeDefinition)
			{
				if (genericTypeMappings?.ContainsKey(self) ?? false)
				{
					var selfGenericArguments = self.GetGenericArguments();
					var genericArguments = new string[selfGenericArguments.Length];

					for (var i = 0; i < selfGenericArguments.Length; i++)
					{
						var argument = selfGenericArguments[i];
						genericArguments[i] = genericTypeMappings[self][argument.Name];
					}

					return $"{self.FullName!.Split("`")[0]}<{string.Join(", ", genericArguments)}>";
				}
				else
				{
					var selfGenericArguments = self.GetGenericArguments();
					var genericArguments = new string[selfGenericArguments.Length];

					for (var i = 0; i < selfGenericArguments.Length; i++)
					{
						var argument = selfGenericArguments[i];
						var argumentAttributes = argument.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;

						if (argumentAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
						{
							genericArguments[i] = "int";
						}
						else if (argumentAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
						{
							genericArguments[i] = "object";
						}
						else
						{
							genericArguments[i] = "object";
						}
					}

					return $"{self.FullName!.Split("`")[0]}<{string.Join(", ", genericArguments)}>";
				}
			}
			else
			{
				return self.FullName!;
			}
		}

		internal static bool IsValidTarget(this Type self,
			Dictionary<Type, Dictionary<string, string>>? genericTypeMappings = null)
		{
			if (self.GetCustomAttribute<RequiresPreviewFeaturesAttribute>() is null)
			{
				var code = $"public class Foo {{ public {self.GetTypeDefinition(genericTypeMappings)} Data {{ get; }} }}";
				var syntaxTree = CSharpSyntaxTree.ParseText(code);
				var references = AppDomain.CurrentDomain.GetAssemblies()
					.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
					.Select(_ => MetadataReference.CreateFromFile(_.Location))
					.Concat(new[] { MetadataReference.CreateFromFile(self.Assembly.Location) });
				var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
					references, new(OutputKind.DynamicallyLinkedLibrary));
				var model = compilation.GetSemanticModel(syntaxTree, true);

				var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
					.OfType<PropertyDeclarationSyntax>().Single();
				var symbol = model.GetDeclaredSymbol(propertySyntax)!.Type;
				var information = new MockInformation(symbol!, compilation.Assembly, model,
					new ConfigurationValues(IndentStyle.Tab, 3, true), BuildType.Create);

				return information.TypeToMock is not null;
			}

			return false;
		}

	}
}