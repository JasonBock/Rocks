﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Descriptors;
using System;
using System.Globalization;
using System.Linq;

namespace Rocks.Tests.Descriptors
{
	public static class CannotSpecifyTypeWithOpenGenericParametersDescriptorTests
	{
		[Test]
		public static void Create()
		{
			var syntaxTree = CSharpSyntaxTree.ParseText("public class X { }");
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location));
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<TypeDeclarationSyntax>().Single();

			var descriptor = CannotSpecifyTypeWithOpenGenericParametersDescriptor.Create(model.GetDeclaredSymbol(typeSyntax)!);

			Assert.Multiple(() =>
			{
				Assert.That(descriptor.GetMessage(), Is.EqualTo("The type X has an open generic parameter and cannot be mocked"));
				Assert.That(descriptor.Descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(CannotSpecifyTypeWithOpenGenericParametersDescriptor.Title));
				Assert.That(descriptor.Id, Is.EqualTo(CannotSpecifyTypeWithOpenGenericParametersDescriptor.Id));
				Assert.That(descriptor.Severity, Is.EqualTo(DiagnosticSeverity.Error));
			});
		}
	}
}