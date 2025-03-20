using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Models;

namespace Rocks.Tests.Models;

public static class ParameterModelTests
{
	[Test]
	public static void Create()
	{
		var code =
			"""
			public class Target
			{
				public void Go(string value) { }
			}
			""";

		(var parameter, var compilation) = ParameterModelTests.GetSymbolsCompilation(code);
		var model = new ParameterModel(parameter, compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.ExplicitDefaultValue, Is.Null);
			Assert.That(model.HasExplicitDefaultValue, Is.False);
			Assert.That(model.IsParams, Is.False);
			Assert.That(model.Name, Is.EqualTo("value"));
			Assert.That(model.RefKind, Is.EqualTo(RefKind.None));
			Assert.That(model.RequiresNullableAnnotation, Is.False);
			Assert.That(model.Type.FullyQualifiedName, Is.EqualTo("string"));
		});
	}

	[Test]
	public static void CreateWithAttributes()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go([In] string value) { }
			}
			""";

		(var parameter, var compilation) = ParameterModelTests.GetSymbolsCompilation(code);
		var model = new ParameterModel(parameter, compilation);

		Assert.That(model.AttributesDescription, Is.EqualTo("[global::System.Runtime.InteropServices.InAttribute]"));
	}

	[Test]
	public static void CreateWithDefaultValues()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go(string value = "data") { }
			}
			""";

		(var parameter, var compilation) = ParameterModelTests.GetSymbolsCompilation(code);
		var model = new ParameterModel(parameter, compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.ExplicitDefaultValue, Is.EqualTo("\"data\""));
			Assert.That(model.HasExplicitDefaultValue, Is.True);
		});
	}

	[Test]
	public static void CreateWithParams()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go(params string[] values) { }
			}
			""";

		(var parameter, var compilation) = ParameterModelTests.GetSymbolsCompilation(code);
		var model = new ParameterModel(parameter, compilation);

		Assert.That(model.IsParams, Is.True);
	}

	[Test]
	public static void CreateWithRequiresNullableAnnotation()
	{
		var code =
			"""
			using System.Runtime.InteropServices;

			public class Target
			{
				public void Go(string values = null) { }
			}
			""";

		(var parameter, var compilation) = ParameterModelTests.GetSymbolsCompilation(code);
		var model = new ParameterModel(parameter, compilation);

		Assert.That(model.RequiresNullableAnnotation, Is.True);
	}

	private static (IParameterSymbol, Compilation) GetSymbolsCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0], compilation);
	}
}