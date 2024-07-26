﻿using Microsoft.CodeAnalysis;
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
			"""
			using System;
			using System.Diagnostics.CodeAnalysis;
			
			namespace MockTests
			{
				public interface Allow
				{
					[AllowNull]
					string NewLine { get; set; }
				}
			}
			""";

		var propertySymbol = IPropertySymbolExtensionsGetAllAttributesTests.GetPropertySymbol(code);
		var attributes = propertySymbol.GetAllAttributes();

		Assert.Multiple(() =>
		{
			Assert.That(attributes, Has.Length.EqualTo(1));
			Assert.That(attributes[0].AttributeClass!.Name, Is.EqualTo(nameof(AllowNullAttribute)));
		});
	}

	[Test]
	public static void GetAllAttributesOnNonAbstractProperty()
	{
		var code =
			"""
			using System;
			using System.Diagnostics.CodeAnalysis;
			
			namespace MockTests
			{
				public class Allow
				{
					[AllowNull]
					public virtual string NewLine { get; set; }
				}
			}
			""";

		var propertySymbol = IPropertySymbolExtensionsGetAllAttributesTests.GetPropertySymbol(code);
		var attributes = propertySymbol.GetAllAttributes();

		Assert.Multiple(() =>
		{
			Assert.That(attributes, Has.Length.EqualTo(1));
			Assert.That(attributes[0].AttributeClass!.Name, Is.EqualTo(nameof(AllowNullAttribute)));
		});
	}

	private static IPropertySymbol GetPropertySymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.Single(_ => _.IsKind(SyntaxKind.IndexerDeclaration) || _.IsKind(SyntaxKind.PropertyDeclaration));
		return (model.GetDeclaredSymbol(propertySyntax) as IPropertySymbol)!;
	}
}