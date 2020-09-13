using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Descriptors;
using Rocks.Extensions;
using System;
using System.Linq;

namespace Rocks.Tests
{
	public static class MockInformationTests
	{
		[Test]
		public static void CreateWhenClassIsSealed()
		{
			const string targetTypeName = "SealedType";
			var code = $"public sealed class {targetTypeName} {{ }}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSealedTypeDescriptor.Id), Is.True);

				Assert.That(information.Constructors.Length, Is.EqualTo(1));
				var constructorMethod = information.Constructors[0];
				Assert.That(constructorMethod.Parameters.Length, Is.EqualTo(0));

				Assert.That(information.Methods.Length, Is.EqualTo(3));
				var getHashCodeMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
				Assert.That(getHashCodeMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var equalsMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.Equals));
				Assert.That(equalsMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var toStringMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.ToString));
				Assert.That(toStringMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenTypeIsObsoleteAndErrorIsTrue()
		{
			const string targetTypeName = "ObsoleteType";
			var code =
$@"using System;

[Obsolete(""a"", true)]
public class {targetTypeName} {{ }}";

			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockObsoleteTypeDescriptor.Id), Is.True);

				Assert.That(information.Constructors.Length, Is.EqualTo(1));
				var constructorMethod = information.Constructors[0];
				Assert.That(constructorMethod.Parameters.Length, Is.EqualTo(0));

				Assert.That(information.Methods.Length, Is.EqualTo(3));
				var getHashCodeMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
				Assert.That(getHashCodeMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var equalsMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.Equals));
				Assert.That(equalsMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var toStringMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.ToString));
				Assert.That(toStringMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenClassHasNoMockableMembers()
		{
			const string targetTypeName = "NoMockables";
			var code =
$@"public class {targetTypeName}
{{
	public override sealed bool Equals(object? obj) => base.Equals(obj);
	public override sealed int GetHashCode() => base.GetHashCode();
	public override sealed string? ToString() => base.ToString();
}}";

			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Any(_ => _.Id == TypeHasNoMockableMembersDescriptor.Id), Is.True);

				Assert.That(information.Constructors.Length, Is.EqualTo(1));
				var constructorMethod = information.Constructors[0];
				Assert.That(constructorMethod.Parameters.Length, Is.EqualTo(0));

				Assert.That(information.Methods.Length, Is.EqualTo(0));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenInterfaceHasNoMockableMembers()
		{
			const string targetTypeName = "NoMockables";
			var code = $"public interface {targetTypeName} {{ }}";

			var information = MockInformationTests.GetInformation(code, targetTypeName);

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
		public static void CreateWhenClassHasNoAccessibleConstructors()
		{
			const string targetTypeName = "SealedType";
			var code = 
$@"public class {targetTypeName} 
{{
	private {targetTypeName}() {{ }}
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Any(_ => _.Id == TypeHasNoAccessibleConstructorsDescriptor.Id), Is.True);
				Assert.That(information.Constructors.Length, Is.EqualTo(0));

				Assert.That(information.Methods.Length, Is.EqualTo(3));
				var getHashCodeMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
				Assert.That(getHashCodeMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var equalsMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.Equals));
				Assert.That(equalsMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var toStringMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.ToString));
				Assert.That(toStringMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
				Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		private static MockInformation GetInformation(string source, string targetTypeName)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location) });
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == targetTypeName);
			var typeSymbol = model.GetDeclaredSymbol(typeSyntax)!;
			return new MockInformation(typeSymbol, typeSymbol.ContainingAssembly, model, compilation);
		}
	}
}