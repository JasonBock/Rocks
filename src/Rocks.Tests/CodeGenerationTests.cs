using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Builders;
using Rocks.Configuration;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Versioning;

namespace Rocks.Tests;

public static class CodeGenerationTests
{
	private static readonly Type[] targetTypes = new Type[] { typeof(object), typeof(Dictionary<,>), typeof(ImmutableArray), typeof(HttpMessageHandler) };

	[Test]
	public static void GenerateCreatesForBaseClassLibrary() =>
		CodeGenerationTests.GenerateForBaseClassLibrary(new RockCreateGenerator());

	[Test]
	public static void GenerateMakesForBaseClassLibrary() =>
		CodeGenerationTests.GenerateForBaseClassLibrary(new RockMakeGenerator());

	private static void GenerateForBaseClassLibrary(IIncrementalGenerator generator)
	{
		var isCreate = generator is RockCreateGenerator;
		var assemblies = CodeGenerationTests.targetTypes.Select(_ => _.Assembly).ToHashSet();

		//var discoveredTypes = new ConcurrentDictionary<Type, byte>();

		//foreach (var assembly in assemblies)
		//{
		//	Parallel.ForEach(assembly.GetTypes()
		//		.Where(_ => _.IsPublic && !_.IsSealed), _ =>
		//		{
		//			if (_.IsValidTarget())
		//			{
		//				discoveredTypes.AddOrUpdate(_, 0, (_, _) => 0);
		//			}
		//		});
		//}

		//var types = discoveredTypes.Keys.ToArray();
		var types = new Type[] { typeof(TextReader) };

		var code = CodeGenerationTests.GetCode(types, isCreate);
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location)))
			.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location),
			});
		var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
			references, new(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

		var driver = CSharpGeneratorDriver.Create(generator);
		driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

		Assert.Multiple(() =>
		{
			Assert.That(types.Length, Is.GreaterThan(0));
			Assert.That(diagnostics.Any(
				_ => _.Severity == DiagnosticSeverity.Error || _.Severity == DiagnosticSeverity.Warning), Is.False);

			using var outputStream = new MemoryStream();
			var result = outputCompilation.Emit(outputStream);

			Assert.That(result.Success, Is.True);
			// TODO: Remember to include warnings as well before #167 is merged.
			var errorDiagnostics = result.Diagnostics.Where(_ => _.Severity == DiagnosticSeverity.Error).ToArray();
			Assert.That(errorDiagnostics.Length, Is.EqualTo(0));
		});
	}

	private static string GetCode(Type[] types, bool isCreate)
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

		for (var i = 0; i < types.Length; i++)
		{
			indentWriter.WriteLine($"var r{i} = Rock.{(isCreate ? "Create" : "Make")}<{types[i].GetTypeDefinition()}>();");
		}

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		return writer.ToString();
	}

	private static bool IsValidTarget(this Type self)
	{
		if(self.GetCustomAttribute<RequiresPreviewFeaturesAttribute>() is null)
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

		return false;
	}

	/// <summary>
	/// This isn't complete. There may be constraints that won't be satisfied with this.
	/// I'll tackle them as they come.
	/// </summary>
	private static string GetTypeDefinition(this Type self)
	{
		if (self.IsGenericTypeDefinition)
		{
			var selfGenericArguments = self.GetGenericArguments();
			var genericArguments = new string[selfGenericArguments.Length];

			for (var i = 0; i < selfGenericArguments.Length; i++)
			{
				var argument = selfGenericArguments[0];
				var argumentAttributes = argument.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;

				if (argumentAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
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