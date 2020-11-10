using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Builders;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rocks.Tests.Builders
{
	public static class MethodExpectationsExtensionsBuilderTests
	{
		[Test]
		public static void DoHappyPath()
		{
			var code =
@"using System.IO;

public interface IFoo
{
	void Foo(int a, Guid b, StringWriter c);
}";
			var information = MethodExpectationsExtensionsBuilderTests.GetInformation(code);

			using var writer = new StringWriter();
			using var indentWriter = new IndentedTextWriter(writer, "	");
			var namespaces = new SortedSet<string>();
			MethodExpectationsExtensionsBuilder.Build(indentWriter, information, namespaces);
			var result = writer.ToString();

			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Empty);
			});
		}

		private static MockInformation GetInformation(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(RockCreateGenerator).Assembly.Location) });
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<TypeDeclarationSyntax>().Single();
			var typeSymbol = model.GetDeclaredSymbol(typeSyntax)!;
			return new MockInformation(typeSymbol, typeSymbol.ContainingAssembly, model, compilation);
		}
	}
}