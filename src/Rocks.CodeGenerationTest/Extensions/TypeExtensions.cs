using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Models;
using System.Reflection;
using System.Runtime.Versioning;

namespace Rocks.CodeGenerationTest.Extensions
{
   internal static class TypeExtensions
	{
		internal static string GetTypeDefinition(this Type self,
			Dictionary<Type, Dictionary<string, string>>? genericTypeMappings, string[] aliases)
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

					return $"{(aliases.Length > 0 ? $"{aliases[0]}::" : string.Empty)}{self.FullName!.Split("`")[0]}<{string.Join(", ", genericArguments)}>";
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

					return $"{(aliases.Length > 0 ? $"{aliases[0]}::" : string.Empty)}{self.FullName!.Split("`")[0]}<{string.Join(", ", genericArguments)}>";
				}
			}
			else
			{
				return self.FullName!;
			}
		}

		internal static bool IsValidTarget(this Type self,
			string[] aliases,
			Dictionary<Type, Dictionary<string, string>>? genericTypeMappings = null)
		{
			if (self.GetCustomAttribute<RequiresPreviewFeaturesAttribute>() is null)
			{
				var code = 
					$$"""
					{{(aliases.Length > 0 ? $"extern alias {aliases[0]}" : string.Empty)}}
					public class Foo 
					{ 
						public {{self.GetTypeDefinition(genericTypeMappings, aliases)}} Data { get; } 
					}
					""";
				var syntaxTree = CSharpSyntaxTree.ParseText(code);
				var references = AppDomain.CurrentDomain.GetAssemblies()
					.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
					.Select(_ => MetadataReference.CreateFromFile(_.Location))
					.Concat(new[] { MetadataReference.CreateFromFile(self.Assembly.Location).WithAliases(aliases) });
				var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
					references, new(OutputKind.DynamicallyLinkedLibrary,
						allowUnsafe: true,
						generalDiagnosticOption: ReportDiagnostic.Error,
						specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));
				var model = compilation.GetSemanticModel(syntaxTree, true);

				var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
					.OfType<PropertyDeclarationSyntax>().Single();
				var symbol = model.GetDeclaredSymbol(propertySyntax)!.Type;
				var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression("public static void Foo() { }")); 
				var mockModel = MockModel.Create(invocation, symbol!, model, BuildType.Create, true);
				return mockModel.Type is not null;
			}

			return false;
		}
	}
}