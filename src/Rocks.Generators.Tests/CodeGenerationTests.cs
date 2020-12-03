using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	public static class CodeGenerationTests
	{
		[Test]
		public static void GenerateForBaseClassLibrary()
		{
			var types = new ConcurrentBag<Type>();
			Parallel.ForEach(typeof(object).Assembly.GetTypes()
				.Where(_ => _.IsPublic && !_.IsSealed && !_.IsGenericTypeDefinition &&
					!_.IsGenericType), _ =>
					{
						if (_.IsValidTarget())
						{
							types.Add(_);
						}
					});

			var discoveredTypes = types.ToArray();

			var code = CodeGenerationTests.GetCode(types, discoveredTypes);
			var syntaxTree = CSharpSyntaxTree.ParseText(code);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[]
				{
					MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
					MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location),
				});
			var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
				references, new(OutputKind.DynamicallyLinkedLibrary));

			var generator = new RockCreateGenerator();
			var driver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(generator));
			driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Any(
					_ => _.Severity == DiagnosticSeverity.Error || _.Severity == DiagnosticSeverity.Warning), Is.False);
			});
		}

		private static string GetCode(ConcurrentBag<Type> types, Type[] discoveredTypes)
		{
			using var writer = new StringWriter();
			using var indentWriter = new IndentedTextWriter(writer, "\t");
			indentWriter.WriteLine("using Rocks;");
			indentWriter.WriteLine();
			indentWriter.WriteLine("public static class GenerateCode");
			indentWriter.WriteLine("{");
			indentWriter.Indent++;

			indentWriter.WriteLine("public static void Go()");
			indentWriter.WriteLine("{");
			indentWriter.Indent++;

			for (var i = 0; i < types.Count; i++)
			{
				indentWriter.WriteLine($"var r{i} = Rock.Create<{discoveredTypes[i].FullName}>();");
			}

			indentWriter.Indent--;
			indentWriter.WriteLine("}");

			indentWriter.Indent--;
			indentWriter.WriteLine("}");

			return writer.ToString();
		}

		private static bool IsValidTarget(this Type self)
		{
			// TODO: What about generic parameters? Oh god, structs, classes, etc. how do I generate those?
			var code = $"using {self.Namespace}; public class Foo {{ public {self.Name} Data {{ get; }} }}";
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
			var information = new MockInformation(symbol!, compilation.Assembly, model);

			return !information.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error);
		}
	}
}