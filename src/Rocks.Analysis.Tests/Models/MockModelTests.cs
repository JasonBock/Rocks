using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class MockModelTests
{
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public UsesObsolete(DoNotUse use) { } public virtual void Foo() { } }")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual void ObsoleteMethod(DoNotUse use) { } }")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual DoNotUse ObsoleteMethod() => default!; }")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual DoNotUse ObsoleteProperty { get; } }")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual DoNotUse ObsoleteProperty { get; } }")]
	[TestCase("using System; [Obsolete(\"Old\", error: true)]public class DoNotUse { } public class UsesObsolete { public virtual int this[DoNotUse value] { get; } }")]
	public static async Task CreateWhenMemberUsesObsoleteTypeAsync(string code)
	{
		var model = await GetModelAsync(code, "UsesObsolete", BuildType.Create);
		Assert.That(model.Information, Is.Null);
	}

	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal abstract void Work(); }", (int)BuildType.Create, true)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work(); }", (int)BuildType.Create, true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal abstract string Work { get; } }", (int)BuildType.Create, true)]
	[TestCase("public interface InternalTargets { string VisibleWork { get; } internal string Work { get; } }", (int)BuildType.Create, true)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal abstract event EventHandler Work; }", (int)BuildType.Create, true)]
	[TestCase("using System; public interface InternalTargets { event EventHandler VisibleWork; internal event EventHandler Work; }", (int)BuildType.Create, true)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal abstract void Work(); }", (int)BuildType.Make, true)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work(); }", (int)BuildType.Make, true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal abstract string Work { get; } }", (int)BuildType.Make, true)]
	[TestCase("public interface InternalTargets { string VisibleWork { get; } internal string Work { get; } }", (int)BuildType.Make, true)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal abstract event EventHandler Work; }", (int)BuildType.Make, true)]
	[TestCase("using System; public interface InternalTargets { event EventHandler VisibleWork; internal event EventHandler Work; }", (int)BuildType.Make, true)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal virtual void Work() { } }", (int)BuildType.Create, false)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work() { } }", (int)BuildType.Create, true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal virtual string Work { get; } }", (int)BuildType.Create, false)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal virtual event EventHandler Work; }", (int)BuildType.Create, false)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal virtual void Work() { } }", (int)BuildType.Make, false)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work() { } }", (int)BuildType.Make, true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal virtual string Work { get; } }", (int)BuildType.Make, false)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal virtual event EventHandler Work; }", (int)BuildType.Make, false)]
	public static async Task CreateWhenTargetHasInternalAbstractMembersAsync(string code, int buildType, bool isMockNull)
	{
		const string targetTypeName = "InternalTargets";
		var (invocation, internalSymbol, internalModel) = await GetTypeAsync(code, targetTypeName);

		var syntaxTree = CSharpSyntaxTree.ParseText(
			$"public class Target {{ public void Test({targetTypeName} a) {{ }} }}");
		var references = Shared.References.Value
			.Concat(
			[
				MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location)
			])
			.Cast<MetadataReference>()
			.ToList();
		references.Add(internalModel.SemanticModel.Compilation.ToMetadataReference());

		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var compilationModel = compilation.GetSemanticModel(syntaxTree, true);
		var parameterSymbol = compilationModel.GetDeclaredSymbol(
			(await syntaxTree.GetRootAsync()).DescendantNodes(_ => true).OfType<ParameterSyntax>().Single());

		var model = MockModel.Create(invocation, parameterSymbol!.Type, null, new(compilationModel), (BuildType)buildType, true);

		Assert.That(model.Information is null, Is.EqualTo(isMockNull));
	}

	[Test]
	public static async Task CreateWhenInterfaceHasStaticAbstractMethodAsync()
	{
		const string targetTypeName = "IHaveStaticAbstractMethod";
		var code =
			$$"""
			public interface {{targetTypeName}} 
			{ 
				static abstract void Foo();
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassDerivesFromEnumAsync()
	{
		const string targetTypeName = "EnumType";
		var code = $"public enum {targetTypeName} {{ }}";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassIsEnumAsync()
	{
		const string targetTypeName = "EnumType";
		var code = $"public enum {targetTypeName} {{ }}";
		var (invocation, type, semanticModel) = await GetTypeAsync(code, targetTypeName);
		var model = MockModel.Create(invocation, type.BaseType!, null, semanticModel, BuildType.Create, true);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassIsValueTypeAsync()
	{
		const string targetTypeName = "ValueTypeType";
		var code = $"public struct {targetTypeName} {{ }}";
		var (invocation, type, semanticModel) = await GetTypeAsync(code, targetTypeName);
		var model = MockModel.Create(invocation, type.BaseType!, null, semanticModel, BuildType.Create, true);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassDerivesFromValueTypeAsync()
	{
		const string targetTypeName = "StructType";
		var code = $"public struct {targetTypeName} {{ }}";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassIsSealedAsync()
	{
		const string targetTypeName = "SealedType";
		var code = $"public sealed class {targetTypeName} {{ }}";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenTypeIsObsoleteAndErrorIsTrueAsync()
	{
		const string targetTypeName = "ObsoleteType";
		var code =
			$$"""
			using System;

			[Obsolete("a", true)]
			public class {{targetTypeName}} { }
			""";

		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenTypeIsObsoleteAndErrorIsSetToFalseAndTreatWarningsAsErrorsAsTrueAsync()
	{
		const string targetTypeName = "ObsoleteType";
		var code =
			$$"""
			using System;

			[Obsolete("a", false)]
			public class {{targetTypeName}} { }
			""";

		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Not.Null);
	}

	[Test]
	public static async Task CreateWhenTypeIsObsoleteAndErrorIsSetToFalseAndTreatWarningsAsErrorsAsFalseAsync()
	{
		const string targetTypeName = "ObsoleteType";
		var code =
			$$"""
			using System;

			[Obsolete("a", false)]
			public class {{targetTypeName}}
			{
				public virtual void Foo() { }
			}
			""";

		var model = await GetModelAsync(code, targetTypeName, BuildType.Create, ReportDiagnostic.Default);

		Assert.That(model.Information, Is.Not.Null);
	}

	[Test]
	public static async Task CreateWhenClassDerivesFromDelegateAsync()
	{
		const string targetTypeName = "MySpecialMethod";
		var code = $"public delegate void {targetTypeName}();";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassIsMulticastDelegateAsync()
	{
		const string targetTypeName = "MySpecialMethod";
		var code = $"public delegate void {targetTypeName}();";
		var (invocation, type, semanticModel) = await GetTypeAsync(code, targetTypeName);

		while (type is not null && type.SpecialType != SpecialType.System_MulticastDelegate)
		{
			type = type.BaseType;
		}

		var model = MockModel.Create(invocation, type!, null, semanticModel, BuildType.Create, true);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassIsDelegateAsync()
	{
		const string targetTypeName = "MySpecialMethod";
		var code = $"public delegate void {targetTypeName}();";
		var (invocation, type, semanticModel) = await GetTypeAsync(code, targetTypeName);

		while (type is not null && type.SpecialType != SpecialType.System_Delegate)
		{
			type = type.BaseType;
		}

		var model = MockModel.Create(invocation, type!, null, semanticModel, BuildType.Create, true);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassHasNoMockableMembersAsync()
	{
		const string targetTypeName = "NoMockables";
		var code =
			$$"""
			public class {{targetTypeName}}
			{
				public override sealed bool Equals(object? obj) => base.Equals(obj);
				public override sealed int GetHashCode() => base.GetHashCode();
				public override sealed string? ToString() => base.ToString();
			}
			""";

		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenClassHasNoMockableMembersAndBuildTypeIsMakeAsync()
	{
		const string targetTypeName = "NoMockables";
		var code =
			$$"""
			public class {{targetTypeName}}
			{
				public override sealed bool Equals(object? obj) => base.Equals(obj);
				public override sealed int GetHashCode() => base.GetHashCode();
				public override sealed string? ToString() => base.ToString();
			}
			""";

		var model = await GetModelAsync(code, targetTypeName, BuildType.Make);

		Assert.That(model.Information, Is.Not.Null);
	}

	[Test]
	public static async Task CreateWhenInterfaceHasNoMockableMembersAsync()
	{
		const string targetTypeName = "NoMockables";
		var code = $"public interface {targetTypeName} {{ }}";

		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenInterfaceHasNoMockableMembersAndBuildTypeIsMakeAsync()
	{
		const string targetTypeName = "NoMockables";
		var code = $"public interface {targetTypeName} {{ }}";

		var model = await GetModelAsync(code, targetTypeName, BuildType.Make);

		Assert.That(model.Information, Is.Not.Null);
	}

	[Test]
	public static async Task CreateWhenClassHasNoAccessibleConstructorsAsync()
	{
		const string targetTypeName = "SealedType";
		var code =
			$$"""
			public class {{targetTypeName}} 
			{
				private {{targetTypeName}}() { }
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		Assert.That(model.Information, Is.Null);
	}

	[Test]
	public static async Task CreateWhenInterfaceHasMethodsAsync()
	{
		const string targetTypeName = "InterfaceWithMembers";
		var code =
			$$"""
			using System;

			public interface {{targetTypeName}} 
			{
				void Foo();
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Diagnostics, Is.Empty);
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(1));
			Assert.That(model.Information.Type.Properties, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(0));
		}
	}

	[Test]
	public static async Task CreateWhenInterfaceHasPropertiesAsync()
	{
		const string targetTypeName = "InterfaceWithMembers";
		var code =
			$$"""
			using System;

			public interface {{targetTypeName}}
			{
				string Data { get; set; }
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Diagnostics, Is.Empty);
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Properties, Has.Length.EqualTo(1));
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(0));
		}
	}

	[Test]
	public static async Task CreateWhenInterfaceHasEventsAsync()
	{
		const string targetTypeName = "InterfaceWithMembers";
		var code =
			$$"""
			using System;

			public interface {{targetTypeName}}
			{
				void Foo();
				event EventHandler TargetEvent;
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Diagnostics, Is.Empty);
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(1));
			Assert.That(model.Information.Type.Properties, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(1));
		}
	}

	[Test]
	public static async Task CreateWhenInterfaceAndBaseInterfaceHasIndexersAsync()
	{
		const string targetTypeName = "InterfaceWithMembers";
		var code =
			$$"""
			using System;

			public interface IBase
			{
				int this[string key] { get; }
			}

			public interface {{targetTypeName}}
				: IBase
			{
				int this[string key, int value] { get; }
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Diagnostics, Is.Empty);
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Properties, Has.Length.EqualTo(2));
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(0));
		}
	}

	[Test]
	public static async Task CreateWhenClassHasMethodsAsync()
	{
		const string targetTypeName = "ClassWithMembers";
		const string fooMethodName = "Foo";

		var code =
			$$"""
			using System;

			public class {{targetTypeName}}
			{
				public virtual void {{fooMethodName}}() { }
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Diagnostics, Is.Empty);
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(1));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(4));

			var fooMethod = model.Information.Type.Methods.Single(_ => _.Name == fooMethodName);
			Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var getHashCodeMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var equalsMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var toStringMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			Assert.That(model.Information.Type.Properties, Has.Length.EqualTo(0));
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(0));
		}
	}

	[Test]
	public static async Task CreateWhenClassHasPropertiesAsync()
	{
		const string targetTypeName = "ClassWithMembers";
		var code =
			$$"""
			using System;

			public class {{targetTypeName}} 
			{
				public virtual string Data { get; set; }
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(1));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(3));

			var getHashCodeMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var equalsMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var toStringMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			Assert.That(model.Information.Type.Properties, Has.Length.EqualTo(1));
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(0));
		}
	}

	[Test]
	public static async Task CreateWhenClassHasEventsAsync()
	{
		const string targetTypeName = "ClassWithMembers";

		var code =
			$$"""
			using System;

			public class {{targetTypeName}}
			{
				public virtual event EventHandler TargetEvent;
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(1));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(3));

			var getHashCodeMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var equalsMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var toStringMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			Assert.That(model.Information.Type.Properties, Is.Empty);
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(1));
		}
	}

	[Test]
	public static async Task CreateWhenClassHasConstructorsAsync()
	{
		const string targetTypeName = "ClassWithMembers";
		var code =
			$$"""
			using System;

			public class {{targetTypeName}}
			{
				public {{targetTypeName}}() { }

				public {{targetTypeName}}(string a) { }

				public virtual event EventHandler TargetEvent;
			}
			""";
		var model = await GetModelAsync(code, targetTypeName, BuildType.Create);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model!.Information!.Type.Constructors, Has.Length.EqualTo(2));
			Assert.That(model.Information.Type.Methods, Has.Length.EqualTo(3));

			var getHashCodeMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var equalsMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var toStringMethod = model.Information.Type.Methods.Single(_ => _.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			Assert.That(model.Information.Type.Properties, Is.Empty);
			Assert.That(model.Information.Type.Events, Has.Length.EqualTo(1));
		}
	}

	private static async Task<(InvocationExpressionSyntax, ITypeSymbol, ModelContext)> GetTypeAsync(string source, string targetTypeName,
		ReportDiagnostic generalDiagnosticOption = ReportDiagnostic.Error)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = Shared.References.Value
			.Concat([MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location)]);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, generalDiagnosticOption: generalDiagnosticOption));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var descendantNodes = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true);

#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection
		if (descendantNodes.OfType<BaseTypeDeclarationSyntax>()
			 .SingleOrDefault(_ => _.Identifier.Text == targetTypeName) is not MemberDeclarationSyntax typeSyntax)
		{
			typeSyntax = descendantNodes.OfType<DelegateDeclarationSyntax>()
				.Single(_ => _.Identifier.Text == targetTypeName);
		}
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection<

		var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression("public static void Foo() { }"));

		return (invocation, (model.GetDeclaredSymbol(typeSyntax)! as ITypeSymbol)!, new(model));
	}

	private static async Task<MockModel> GetModelAsync(string source, string targetTypeName,
		BuildType buildType, ReportDiagnostic generalDiagnosticOption = ReportDiagnostic.Error)
	{
		var (invocation, typeSymbol, modelContext) =
			await GetTypeAsync(source, targetTypeName, generalDiagnosticOption);
		return MockModel.Create(invocation, typeSymbol!, null, modelContext, buildType, true);
	}
}