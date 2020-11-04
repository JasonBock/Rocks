using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rocks.Tests.Builders
{
	public static class RockCreateBuilderTests
	{
		[Test]
		public static void DoHappyPathForValue()
		{
			var code =
@"using System.IO;

namespace FooStuff
{
	public interface IFoo
	{
		string Bar(int a, string b);
		void Foo(int a, string b);
		void Baz();
	}
}";
			var information = RockCreateBuilderTests.GetInformation(code);

			using var writer = new StringWriter();
			using var indentWriter = new IndentedTextWriter(writer, "	");
			var namespaces = new SortedSet<string>();

			var builder = new RockCreateBuilder(information);
			var result = builder.Text.ToString();

			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Empty);
			});
		}

		[Test]
		public static void DoHappyPathForVoid()
		{
			var code =
@"using System.IO;

namespace FooStuff
{
	public interface IFoo
	{
		void Foo(int a, Guid b, StringWriter c);
		void Bar();
	}
}";
			var information = RockCreateBuilderTests.GetInformation(code);

			using var writer = new StringWriter();
			using var indentWriter = new IndentedTextWriter(writer, "	");
			var namespaces = new SortedSet<string>();

			var builder = new RockCreateBuilder(information);
			var result = builder.Text.ToString();

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