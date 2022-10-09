using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Builders;
using Rocks.Configuration;
using Rocks.Diagnostics;
using Rocks.Extensions;

namespace Rocks.Tests;

public static class MockInformationTests
{
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public UsesObsolete(DoNotUse use) { } public virtual void Foo() { } }", ".ctor")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual void ObsoleteMethod(DoNotUse use) { } }", "ObsoleteMethod")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual DoNotUse ObsoleteMethod() => default!; }", "ObsoleteMethod")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual DoNotUse ObsoleteProperty { get; } }", "ObsoleteProperty")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual DoNotUse ObsoleteProperty { get; } }", "ObsoleteProperty")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual int this[DoNotUse value] { get; } }", "this")]
	public static void CreateWhenMemberUsesObsoleteType(string code, string memberName)
	{
		var information = MockInformationTests.GetInformation(code, "UsesObsolete", BuildType.Create);

		Assert.Multiple(() =>
		{
			var diagnostic = information.Diagnostics.First(_ => _.Id == MemberUsesObsoleteTypeDiagnostic.Id);
			Assert.That(diagnostic.GetMessage(), Does.Contain(memberName));
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal abstract void Work(); }", (int)BuildType.Create, true, true)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work(); }", (int)BuildType.Create, true, true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal abstract string Work { get; } }", (int)BuildType.Create, true, true)]
	[TestCase("public interface InternalTargets { string VisibleWork { get; } internal string Work { get; } }", (int)BuildType.Create, true, true)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal abstract event EventHandler Work; }", (int)BuildType.Create, true, true)]
	[TestCase("using System; public interface InternalTargets { event EventHandler VisibleWork; internal event EventHandler Work; }", (int)BuildType.Create, true, true)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal abstract void Work(); }", (int)BuildType.Make, true, true)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work(); }", (int)BuildType.Make, true, true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal abstract string Work { get; } }", (int)BuildType.Make, true, true)]
	[TestCase("public interface InternalTargets { string VisibleWork { get; } internal string Work { get; } }", (int)BuildType.Make, true, true)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal abstract event EventHandler Work; }", (int)BuildType.Make, true, true)]
	[TestCase("using System; public interface InternalTargets { event EventHandler VisibleWork; internal event EventHandler Work; }", (int)BuildType.Make, true, true)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal virtual void Work() { } }", (int)BuildType.Create, false, false)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work() { } }", (int)BuildType.Create, false, false)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal virtual string Work { get; } }", (int)BuildType.Create, false, false)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal virtual event EventHandler Work; }", (int)BuildType.Create, false, false)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal virtual void Work() { } }", (int)BuildType.Make, false, false)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work() { } }", (int)BuildType.Make, false, false)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal virtual string Work { get; } }", (int)BuildType.Make, false, false)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal virtual event EventHandler Work; }", (int)BuildType.Make, false, false)]
	public static void CreateWhenTargetHasInternalAbstractMembers(string code, int buildType, bool hasDiagnostic, bool isMockNull)
	{
		var (referencingSymbol, _) = MockInformationTests.GetType("public class Target { }", "Target");

		const string targetTypeName = "InternalTargets";
		var information = MockInformationTests.GetInformation(code, targetTypeName, (BuildType)buildType,
			containingAssembly: referencingSymbol.ContainingAssembly);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == TypeHasInaccessibleAbstractMembersDiagnostic.Id), Is.EqualTo(hasDiagnostic));
			Assert.That(information.TypeToMock is null, Is.EqualTo(isMockNull));
		});
	}

	[Test]
	public static void CreateWhenInterfaceHasStaticAbstractMethod()
	{
		const string targetTypeName = "IHaveStaticAbstractMethod";
		var code =
			$$"""
			public interface {{targetTypeName}} 
			{ 
				static abstract void Foo();
			}
			""";
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == InterfaceHasStaticAbstractMembersDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassDerivesFromEnum()
	{
		const string targetTypeName = "EnumType";
		var code = $"public enum {targetTypeName} {{ }}";
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSealedTypeDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassIsEnum()
	{
		const string targetTypeName = "EnumType";
		var code = $"public enum {targetTypeName} {{ }}";
		var (type, model) = MockInformationTests.GetType(code, targetTypeName);
		var information = new MockInformation(type.BaseType!, type.ContainingAssembly, model,
			new ConfigurationValues(IndentStyle.Tab, 3, false), BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSpecialTypesDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassIsValueType()
	{
		const string targetTypeName = "ValueTypeType";
		var code = $"public struct {targetTypeName} {{ }}";
		var (type, model) = MockInformationTests.GetType(code, targetTypeName);
		var information = new MockInformation(type.BaseType!, type.ContainingAssembly, model,
			new ConfigurationValues(IndentStyle.Tab, 3, false), BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSpecialTypesDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassDerivesFromValueType()
	{
		const string targetTypeName = "StructType";
		var code = $"public struct {targetTypeName} {{ }}";
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSealedTypeDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassIsSealed()
	{
		const string targetTypeName = "SealedType";
		var code = $"public sealed class {targetTypeName} {{ }}";
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSealedTypeDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
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

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockObsoleteTypeDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenTypeIsObsoleteAndErrorIsSetToFalseAndTreatWarningsAsErrorsAsTrue()
	{
		const string targetTypeName = "ObsoleteType";
		var code =
$@"using System;

[Obsolete(""a"", false)]
public class {targetTypeName} {{ }}";

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create, true);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockObsoleteTypeDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenTypeIsObsoleteAndErrorIsSetToFalseAndTreatWarningsAsErrorsAsFalse()
	{
		const string targetTypeName = "ObsoleteType";
		var code =
$@"using System;

[Obsolete(""a"", false)]
public class {targetTypeName} 
{{ 
	public virtual void Foo() {{ }}
}}";

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create, false);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockObsoleteTypeDiagnostic.Id), Is.False);
			Assert.That(information.TypeToMock, Is.Not.Null);
		});
	}

	[Test]
	public static void CreateWhenTypeIsOpenGeneric()
	{
		const string targetTypeName = "IGeneric";
		var code =
$@"public interface IBase<T1, T2> 
{{
	void Foo(T1 a, T2 b);
}}

public interface IGeneric<T1> : IBase<T1, string> {{ }}";

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassDerivesFromDelegate()
	{
		const string targetTypeName = "MySpecialMethod";
		var code = $"public delegate void {targetTypeName}();";
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSealedTypeDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassIsMulticastDelegate()
	{
		const string targetTypeName = "MySpecialMethod";
		var code = $"public delegate void {targetTypeName}();";
		var (type, model) = MockInformationTests.GetType(code, targetTypeName);

		while (type is not null && type.SpecialType != SpecialType.System_MulticastDelegate)
		{
			type = type.BaseType;
		}

		var information = new MockInformation(type!, type!.ContainingAssembly, model,
			new ConfigurationValues(IndentStyle.Tab, 3, false), BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSpecialTypesDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassIsDelegate()
	{
		const string targetTypeName = "MySpecialMethod";
		var code = $"public delegate void {targetTypeName}();";
		var (type, model) = MockInformationTests.GetType(code, targetTypeName);

		while (type is not null && type.SpecialType != SpecialType.System_Delegate)
		{
			type = type.BaseType;
		}

		var information = new MockInformation(type!, type!.ContainingAssembly, model,
			new ConfigurationValues(IndentStyle.Tab, 3, false), BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == CannotMockSpecialTypesDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
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

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == TypeHasNoMockableMembersDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenClassHasNoMockableMembersAndBuildTypeIsMake()
	{
		const string targetTypeName = "NoMockables";
		var code =
$@"public class {targetTypeName}
{{
	public override sealed bool Equals(object? obj) => base.Equals(obj);
	public override sealed int GetHashCode() => base.GetHashCode();
	public override sealed string? ToString() => base.ToString();
}}";

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Make);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Is.Empty);
			Assert.That(information.TypeToMock, Is.Not.Null);
		});
	}

	[Test]
	public static void CreateWhenInterfaceHasNoMockableMembers()
	{
		const string targetTypeName = "NoMockables";
		var code = $"public interface {targetTypeName} {{ }}";

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == TypeHasNoMockableMembersDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
		});
	}

	[Test]
	public static void CreateWhenInterfaceHasNoMockableMembersAndBuildTypeIsMake()
	{
		const string targetTypeName = "NoMockables";
		var code = $"public interface {targetTypeName} {{ }}";

		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Make);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Is.Empty);
			Assert.That(information.TypeToMock, Is.Not.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics.Any(_ => _.Id == TypeHasNoAccessibleConstructorsDiagnostic.Id), Is.True);
			Assert.That(information.TypeToMock, Is.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Has.Length.EqualTo(0));
			Assert.That(information.Constructors, Has.Length.EqualTo(0));
			Assert.That(information.Methods.Results, Has.Length.EqualTo(1));
			Assert.That(information.Properties.Results, Has.Length.EqualTo(0));
			Assert.That(information.Events.Results, Has.Length.EqualTo(0));
			Assert.That(information.TypeToMock, Is.Not.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Is.Empty);
			Assert.That(information.Constructors, Has.Length.EqualTo(0));
			Assert.That(information.Methods.Results, Has.Length.EqualTo(0));
			Assert.That(information.Properties.Results, Has.Length.EqualTo(1));
			Assert.That(information.Events.Results, Has.Length.EqualTo(0));
			Assert.That(information.TypeToMock, Is.Not.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Has.Length.EqualTo(0));
			Assert.That(information.Constructors, Has.Length.EqualTo(0));
			Assert.That(information.Methods.Results, Has.Length.EqualTo(1));
			Assert.That(information.Properties.Results, Has.Length.EqualTo(0));
			Assert.That(information.Events.Results, Has.Length.EqualTo(1));
			Assert.That(information.TypeToMock, Is.Not.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Has.Length.EqualTo(0));
			Assert.That(information.Constructors, Has.Length.EqualTo(1));
			Assert.That(information.Methods.Results, Has.Length.EqualTo(4));
			var fooMethod = information.Methods.Results.Single(_ => _.Value.Name == fooMethodName);
			Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var getHashCodeMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(information.Properties.Results, Has.Length.EqualTo(0));
			Assert.That(information.Events.Results, Has.Length.EqualTo(0));
			Assert.That(information.TypeToMock, Is.Not.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Has.Length.EqualTo(0));
			Assert.That(information.Constructors, Has.Length.EqualTo(1));
			Assert.That(information.Methods.Results, Has.Length.EqualTo(3));
			var getHashCodeMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(information.Properties.Results, Has.Length.EqualTo(1));
			Assert.That(information.Events.Results, Has.Length.EqualTo(0));
			Assert.That(information.TypeToMock, Is.Not.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Is.Empty);
			Assert.That(information.Constructors, Has.Length.EqualTo(1));
			Assert.That(information.Methods.Results, Has.Length.EqualTo(3));
			var getHashCodeMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(information.Properties.Results, Is.Empty);
			Assert.That(information.Events.Results, Has.Length.EqualTo(1));
			Assert.That(information.TypeToMock, Is.Not.Null);
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
		var information = MockInformationTests.GetInformation(code, targetTypeName, BuildType.Create);

		Assert.Multiple(() =>
		{
			Assert.That(information.Diagnostics, Is.Empty);
			Assert.That(information.Constructors, Has.Length.EqualTo(2));
			Assert.That(information.Methods.Results, Has.Length.EqualTo(3));
			var getHashCodeMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = information.Methods.Results.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(information.Properties.Results, Is.Empty);
			Assert.That(information.Events.Results, Has.Length.EqualTo(1));
			Assert.That(information.TypeToMock, Is.Not.Null);
		});
	}

	private static (ITypeSymbol, SemanticModel) GetType(string source, string targetTypeName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(new[] { MetadataReference.CreateFromFile(typeof(RockCreateGenerator).Assembly.Location) });
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var descendantNodes = syntaxTree.GetRoot().DescendantNodes(_ => true);

		if (descendantNodes.OfType<BaseTypeDeclarationSyntax>()
			.SingleOrDefault(_ => _.Identifier.Text == targetTypeName) is not MemberDeclarationSyntax typeSyntax)
		{
			typeSyntax = descendantNodes.OfType<DelegateDeclarationSyntax>()
				.Single(_ => _.Identifier.Text == targetTypeName);
		}

		return ((model.GetDeclaredSymbol(typeSyntax)! as ITypeSymbol)!, model);
	}

	private static MockInformation GetInformation(string source, string targetTypeName,
		BuildType buildType, bool treatWarningsAsErrors = false,
		IAssemblySymbol? containingAssembly = null)
	{
		var (typeSymbol, model) = MockInformationTests.GetType(source, targetTypeName);
		return new MockInformation(typeSymbol, containingAssembly ?? typeSymbol.ContainingAssembly, model,
			new ConfigurationValues(IndentStyle.Tab, 3, treatWarningsAsErrors), buildType);
	}
}