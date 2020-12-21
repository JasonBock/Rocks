using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Builders;
using Rocks.Configuration;
using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	public static class CodeGenerationTests
	{
		[TestCase(typeof(object))]
		//[TestCase(typeof(Dictionary<,>))]
		//[TestCase(typeof(ImmutableArray))]
		public static void GenerateCreatesForBaseClassLibrary(Type targetType) => 
			CodeGenerationTests.GenerateForBaseClassLibrary(targetType, new RockCreateGenerator());

		[TestCase(typeof(object))]
		//[TestCase(typeof(Dictionary<,>))]
		//[TestCase(typeof(ImmutableArray))]
		public static void GenerateMakesForBaseClassLibrary(Type targetType) =>
			CodeGenerationTests.GenerateForBaseClassLibrary(targetType, new RockMakeGenerator());

		private static void GenerateForBaseClassLibrary(Type targetType, ISourceGenerator generator)
		{
			if (targetType is null)
			{
				throw new ArgumentNullException(nameof(targetType));
			}

			var types = new ConcurrentBag<Type>();
			Parallel.ForEach(targetType.Assembly.GetTypes()
				.Where(_ => _.IsPublic && !_.IsSealed), _ =>
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
					MetadataReference.CreateFromFile(targetType.Assembly.Location),
					MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location),
				});
			var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
				references, new(OutputKind.DynamicallyLinkedLibrary));

			var driver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(generator));
			driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

			Assert.Multiple(() =>
			{
				Assert.That(discoveredTypes.Length, Is.GreaterThan(0));
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
				indentWriter.WriteLine($"var r{i} = Rock.Create<{discoveredTypes[i].GetTypeDefinition()}>();");
			}

			indentWriter.Indent--;
			indentWriter.WriteLine("}");

			indentWriter.Indent--;
			indentWriter.WriteLine("}");

			return writer.ToString();
		}

		private static bool IsValidTarget(this Type self)
		{
			var code = $"public class Foo {{ public {self.GetTypeDefinition()} Data {{ get; }} }}";
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
			var information = new MockInformation(symbol!, compilation.Assembly, model, 
				new ConfigurationValues(IndentStyle.Tab, 3, true), BuildType.Create);

			return !information.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error);
		}

		/// <summary>
		/// This isn't complete. There may be constraints that won't be satisfied with this.
		/// I'll tackle them as they come.
		/// </summary>
		private static string GetTypeDefinition(this Type self)
		{
			if(self.IsGenericTypeDefinition)
			{
				var selfGenericArguments = self.GetGenericArguments();
				var genericArguments = new string[selfGenericArguments.Length];

				for(var i = 0; i < selfGenericArguments.Length; i++)
				{
					var argument = selfGenericArguments[0];
					var argumentAttributes = argument.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;

					if(argumentAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
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

				return $"{self.FullName!.Split("`")[0]}<{string.Join(", ", genericArguments)}>";
			}
			else
			{
				return self.FullName!;
			}
		}
	}
}