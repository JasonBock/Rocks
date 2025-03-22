using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Models;
using System.Reflection;
using System.Runtime.Versioning;

namespace Rocks.CodeGenerationTest.Extensions;

internal static class TypeExtensions
{
	internal static string GetTypeDefinition(this Type self, string[] aliases, bool includeGenericParameterTypeNames)
	{
		if (self.IsGenericTypeDefinition)
		{
			var selfGenericArguments = self.GetGenericArguments();

			var genericContent = includeGenericParameterTypeNames ?
				self.GetTypeDefinitionParameters() :
				$"<{new string(',', selfGenericArguments.Length - 1)}>";

			return $"{(aliases.Length > 0 ? $"{aliases[0]}::" : string.Empty)}{self.FullName!.Split("`")[0]}{genericContent}";
		}
		else
		{
			return self.FullName!;
		}
	}

	internal static string GetTypeDefinitionParameters(this Type self)
	{
		if (self.IsGenericTypeDefinition)
		{
			var selfGenericArguments = self.GetGenericArguments();
			return $"<{string.Join(", ", selfGenericArguments.Select(_ => _.Name))}>";
		}
		else
		{
			return string.Empty;
		}
	}

	internal static bool IsValidTarget(this Type self, string[] aliases)
	{
		if (self.GetCustomAttribute<RequiresPreviewFeaturesAttribute>() is null)
		{
			var code =
				$$"""
				{{(aliases.Length > 0 ? $"extern alias {aliases[0]}" : string.Empty)}}
				public class Foo 
				{ 
					public {{self.GetTypeDefinition(aliases, false)}} Data { get; } 
				}
				""";
			var syntaxTree = CSharpSyntaxTree.ParseText(code);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat([MetadataReference.CreateFromFile(self.Assembly.Location).WithAliases(aliases)]);
			var compilation = CSharpCompilation.Create("generator", [syntaxTree],
				references, new(OutputKind.DynamicallyLinkedLibrary,
					allowUnsafe: true,
					generalDiagnosticOption: ReportDiagnostic.Error,
					specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<PropertyDeclarationSyntax>().Single();
			var symbol = model.GetDeclaredSymbol(propertySyntax)!.Type;
			var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression("public static void Foo() { }"));
			var mockModel = MockModel.Create(invocation, symbol!, null, new(model), BuildType.Create, true);
			return mockModel.Information is not null;
		}

		return false;
	}
}