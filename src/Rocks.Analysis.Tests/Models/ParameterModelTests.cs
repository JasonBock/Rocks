using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Models;
using System.Threading.Tasks;

namespace Rocks.Analysis.Tests.Models;

public static class ParameterModelTests
{
	[Test]
	public static async Task CreateAsync()
	{
		var code =
			"""
			public class Target
			{
				public void Go(string value) { }
			}
			""";

		(var parameter, var modelContext) = await ParameterModelTests.GetSymbolsCompilationAsync(code);
		var model = new ParameterModel(parameter, modelContext);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.ExplicitDefaultValue, Is.Null);
			Assert.That(model.HasExplicitDefaultValue, Is.False);
			Assert.That(model.IsParams, Is.False);
			Assert.That(model.Name, Is.EqualTo("value"));
			Assert.That(model.RefKind, Is.EqualTo(RefKind.None));
			Assert.That(model.RequiresNullableAnnotation, Is.False);
			Assert.That(model.Type.FullyQualifiedName, Is.EqualTo("string"));
		}
	}

	[Test]
	public static async Task CreateWithAttributesAsync()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go([In] string value) { }
			}
			""";

		(var parameter, var modelContext) = await ParameterModelTests.GetSymbolsCompilationAsync(code);
		var model = new ParameterModel(parameter, modelContext);

		Assert.That(model.AttributesDescription, Is.EqualTo("[global::System.Runtime.InteropServices.InAttribute]"));
	}

	[Test]
	public static async Task CreateWithDefaultValuesAsync()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go(string value = "data") { }
			}
			""";

		(var parameter, var modelContext) = await ParameterModelTests.GetSymbolsCompilationAsync(code);
		var model = new ParameterModel(parameter, modelContext);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.ExplicitDefaultValue, Is.EqualTo("\"data\""));
			Assert.That(model.HasExplicitDefaultValue, Is.True);
		}
	}

	[Test]
	public static async Task CreateWithParamsAsync()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go(params string[] values) { }
			}
			""";

		(var parameter, var modelContext) = await ParameterModelTests.GetSymbolsCompilationAsync(code);
		var model = new ParameterModel(parameter, modelContext);

		Assert.That(model.IsParams, Is.True);
	}

	[Test]
	public static async Task CreateWithRequiresNullableAnnotationAsync()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go(string values = null) { }
			}
			""";

		(var parameter, var modelContext) = await ParameterModelTests.GetSymbolsCompilationAsync(code);
		var model = new ParameterModel(parameter, modelContext);

		Assert.That(model.RequiresNullableAnnotation, Is.True);
	}

	private static async Task<(IParameterSymbol, ModelContext)> GetSymbolsCompilationAsync(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);
		var root = await syntaxTree.GetRootAsync();
		var typeSyntax = root.DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var methodSyntax = root.DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0], new(model));
	}
}