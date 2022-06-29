using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.Tests.Extensions;

public static class IPropertySymbolExtensionsGetAllAttributesTests
{
	[Test]
	public static void GetAllAttributesOnAbstractProperty()
	{
		var code =
@"using System;
using System.Diagnostics.CodeAnalysis;

namespace MockTests
{
	public interface Allow
	{
		 [AllowNull]
		 string NewLine { get; set; }
	}
}";

		var propertySymbol = IPropertySymbolExtensionsGetAllAttributesTests.GetPropertySymbol(code);
		var attributes = propertySymbol.GetAllAttributes();

		Assert.Multiple(() =>
		{
			Assert.That(attributes.Length, Is.EqualTo(1));
			Assert.That(attributes[0].AttributeClass!.Name, Is.EqualTo(nameof(AllowNullAttribute)));
		});
	}

	[Test]
	public static void GetAllAttributesOnNonAbstractProperty()
	{
		var code =
@"using System;
using System.Diagnostics.CodeAnalysis;

namespace MockTests
{
	public class Allow
	{
		 [AllowNull]
		 public virtual string NewLine { get; set; }
	}
}";

		var propertySymbol = IPropertySymbolExtensionsGetAllAttributesTests.GetPropertySymbol(code);
		var attributes = propertySymbol.GetAllAttributes();

		Assert.Multiple(() =>
		{
			Assert.That(attributes.Length, Is.EqualTo(1));
			Assert.That(attributes[0].AttributeClass!.Name, Is.EqualTo(nameof(AllowNullAttribute)));
		});
	}

	private static IPropertySymbol GetPropertySymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.Where(_ => _.IsKind(SyntaxKind.IndexerDeclaration) || _.IsKind(SyntaxKind.PropertyDeclaration)).Single();
		return (model.GetDeclaredSymbol(propertySyntax) as IPropertySymbol)!;
	}
}