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
			});
		}

		[Test]
		public static void CreateWhenInterfaceHasMethods()
		{
			const string targetTypeName = "InterfaceWithMembers";
			var code =
$@"using System;

public interface {targetTypeName} 
{{
	void Foo();
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(0));
				Assert.That(information.Methods.Length, Is.EqualTo(1));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenInterfaceHasProperties()
		{
			const string targetTypeName = "InterfaceWithMembers";
			var code =
$@"using System;

public interface {targetTypeName} 
{{
	string Data {{ get; set; }}
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(0));
				Assert.That(information.Methods.Length, Is.EqualTo(0));
				Assert.That(information.Properties.Length, Is.EqualTo(1));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenInterfaceHasEvents()
		{
			const string targetTypeName = "InterfaceWithMembers";
			var code =
$@"using System;

public interface {targetTypeName} 
{{
	void Foo();
	event EventHandler TargetEvent;
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(0));
				Assert.That(information.Methods.Length, Is.EqualTo(1));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(1));
			});
		}

		[Test]
		public static void CreateWhenClassHasMethods()
		{
			const string targetTypeName = "ClassWithMembers";
			const string fooMethodName = "Foo";

			var code =
$@"using System;

public class {targetTypeName} 
{{
	public virtual void {fooMethodName}() {{ }}
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(1));
				Assert.That(information.Methods.Length, Is.EqualTo(4));
				var fooMethod = information.Methods.Single(_ => _.Value.Name == fooMethodName);
				Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var getHashCodeMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
				Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var equalsMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.Equals));
				Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var toStringMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.ToString));
				Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenClassHasProperties()
		{
			const string targetTypeName = "ClassWithMembers";
			var code =
$@"using System;

public class {targetTypeName} 
{{
	public virtual string Data {{ get; set; }}
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(1));
				Assert.That(information.Methods.Length, Is.EqualTo(3));
				var getHashCodeMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
				Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var equalsMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.Equals));
				Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var toStringMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.ToString));
				Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				Assert.That(information.Properties.Length, Is.EqualTo(1));
				Assert.That(information.Events.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void CreateWhenClassHasEvents()
		{
			const string targetTypeName = "ClassWithMembers";

			var code =
$@"using System;

public class {targetTypeName} 
{{
	public virtual event EventHandler TargetEvent;
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(1));
				Assert.That(information.Methods.Length, Is.EqualTo(3));
				var getHashCodeMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
				Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var equalsMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.Equals));
				Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var toStringMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.ToString));
				Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(1));
			});
		}

		[Test]
		public static void CreateWhenClassHasConstructors()
		{
			const string targetTypeName = "ClassWithMembers";
			var code =
$@"using System;

public class {targetTypeName} 
{{
	public {targetTypeName}() {{ }}

	public {targetTypeName}(string a) {{ }}

	public virtual event EventHandler TargetEvent;
}}";
			var information = MockInformationTests.GetInformation(code, targetTypeName);

			Assert.Multiple(() =>
			{
				Assert.That(information.Diagnostics.Length, Is.EqualTo(0));
				Assert.That(information.Constructors.Length, Is.EqualTo(2));
				Assert.That(information.Methods.Length, Is.EqualTo(3));
				var getHashCodeMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
				Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var equalsMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.Equals));
				Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				var toStringMethod = information.Methods.Single(_ => _.Value.Name == nameof(object.ToString));
				Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
				Assert.That(information.Properties.Length, Is.EqualTo(0));
				Assert.That(information.Events.Length, Is.EqualTo(1));
			});
		}
		private static MockInformation GetInformation(string source, string targetTypeName)
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
				.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == targetTypeName);
			var typeSymbol = model.GetDeclaredSymbol(typeSyntax)!;
			return new MockInformation(typeSymbol, typeSymbol.ContainingAssembly, model);
		}
	}
}