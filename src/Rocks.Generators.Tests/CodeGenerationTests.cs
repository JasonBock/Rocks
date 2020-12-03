using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System;
using System.Linq;

namespace Rocks.Tests
{
	public interface IShouldBeMockable
	{
		void Foo();
	}

#pragma warning disable CA1040 // Avoid empty interfaces
	public interface IShouldNotBeMockable { }
#pragma warning restore CA1040 // Avoid empty interfaces

	public static class CodeGenerationTests
	{
		[Test]
		[Ignore("Work in progress...")]
		public static void GenerateForBaseClassLibrary()
		{
			// Here's the intent. I want find all the types that are "contained" with the same assembly
			// that System.Object exists in. For those that CAN be mocked, generate code like this:
			// 
			// using TargetObject.Namespace;
			// 
			// public static class GenerateCode
			// {
			//   public static void Go()
			//   {
			//     var r0 = Rock.Create<TargetObject>();
			//   }
			// }
			//
			// That's it. Just a bunch of "Rock.Create<>" calls, and then throw the generator at it.
			// It shouldn't fail.
			//
			// Now, there's a hiccup here. MockInformation works with a ITypeSymbol, not a Type.
			// So, one thought (though this would be sloooooow), is to have a method that takes a Type,
			// throws it into code (somehow), like:
			//
			// TargetObject o = default;
			//
			// Get the ITypeSymbol for that, and use MockInformation to see if it's worthy. If it is,
			// it becomes a candidate for the GenerateCode.Go() method.

			var types = typeof(object).Assembly.GetTypes()
				.Where(_ => _.IsPublic && !_.IsSealed && !_.IsGenericTypeDefinition && !_.IsGenericType);
			var typeCount = types.ToArray().Length;
			var validCount = 0;

			foreach(var type in types)
			{
				if(type.IsValidTarget())
				{
					validCount++;
				}
			}

			var shouldBeMockable = typeof(IShouldBeMockable);
			var shouldNotBeMockable = typeof(IShouldNotBeMockable);

			Assert.Multiple(() =>
			{
				Assert.That(shouldBeMockable.IsValidTarget(), Is.True);
				Assert.That(shouldNotBeMockable.IsValidTarget(), Is.False);
			});
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