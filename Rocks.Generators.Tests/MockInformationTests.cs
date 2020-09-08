using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Descriptors;
using System;
using System.Linq;

namespace Rocks.Tests
{
	public static class MockInformationTests
	{
		[Test]
		public static void CreateWhenTypeIsSealed()
		{
			var code = "public sealed class SealedType { }";
			var information = MockInformationTests.GetInformation(code);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSealedTypeDescriptor.Id), Is.True);
				Assert.That(information.Constructors.Length, Is.EqualTo(0));
				Assert.That(information.Methods.Length, Is.EqualTo(0));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenTypeIsObsoleteAndErrorIsTrue()
		{
			var code =
@"using System;

[Obsolete(""a"", true)]
public class ObsoleteType { }";

			var information = MockInformationTests.GetInformation(code);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockObsoleteTypeDescriptor.Id), Is.True);
				Assert.That(information.Constructors.Length, Is.EqualTo(0));
				Assert.That(information.Methods.Length, Is.EqualTo(0));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenTypeHasNoMockableMembers()
		{
			var code =
@"public class NoMockables
{
	public override sealed bool Equals(object? obj) => base.Equals(obj);
	public override sealed int GetHashCode() => base.GetHashCode();
	public override sealed string? ToString() => base.ToString();
}";

			var information = MockInformationTests.GetInformation(code);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Any(_ => _.Id == TypeHasNoMockableMembersDescriptor.Id), Is.True);
				Assert.That(information.Constructors.Length, Is.EqualTo(0));
				Assert.That(information.Methods.Length, Is.EqualTo(0));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWithInterfaceMethods()
		{
			var code =
@"public interface ITest
{
	void Foo();
}";

			var information = MockInformationTests.GetInformation(code);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(0));
				Assert.That(information.Methods.Length, Is.EqualTo(4));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		private static MockInformation GetInformation(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location) });
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true).OfType<TypeDeclarationSyntax>().Single();
			var typeSymbol = model.GetDeclaredSymbol(typeSyntax)!;
			return new MockInformation(typeSymbol, model, compilation);
		}
	}
}