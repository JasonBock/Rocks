using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Models;

namespace Rocks.Tests.Models;

public static class TypeMockModelTests
{
	[Test]
	public static void VerifyMemberCountWhenNoMethodsExist()
	{
		(var node, var type, var model) = TypeMockModelTests.GetInformation(
			"""
			using System;
			
			public interface ITarget
			{
				string Data { get; set; }
			}

			""");
		var mockModel = MockModel.Create(node, type, model, BuildType.Create, false);
		var typeModel = mockModel.Information!.Type;

		Assert.Multiple(() =>
		{
			Assert.That(typeModel.MemberCount.MethodCount, Is.EqualTo(0));
			Assert.That(typeModel.MemberCount.PropertyCount, Is.EqualTo(2));
			Assert.That(typeModel.MemberCount.TotalCount, Is.EqualTo(2));
		});
	}

	[Test]
	public static void VerifyMemberCountWhenNoPropertiesExist()
	{
		(var node, var type, var model) = TypeMockModelTests.GetInformation(
			"""
			using System;
			
			public interface ITarget
			{
				void Work();
			}

			""");
		var mockModel = MockModel.Create(node, type, model, BuildType.Create, false);
		var typeModel = mockModel.Information!.Type;

		Assert.Multiple(() =>
		{
			Assert.That(typeModel.MemberCount.MethodCount, Is.EqualTo(1));
			Assert.That(typeModel.MemberCount.PropertyCount, Is.EqualTo(0));
			Assert.That(typeModel.MemberCount.TotalCount, Is.EqualTo(1));
		});
	}

	[Test]
	public static void VerifyMemberCountWithMethodPropertyMix()
	{
		(var node, var type, var model) = TypeMockModelTests.GetInformation(
			"""
			using System;
			
			public interface ITarget
			{
				void Work();
				void Process();
				void DoSomething();

				string Data { get; set; }
				string Name { get; }
				Guid Id { init ; }
			}

			""");
		var mockModel = MockModel.Create(node, type, model, BuildType.Create, false);
		var typeModel = mockModel.Information!.Type;

		Assert.Multiple(() =>
		{
			Assert.That(typeModel.MemberCount.MethodCount, Is.EqualTo(3));
			Assert.That(typeModel.MemberCount.PropertyCount, Is.EqualTo(4));
			Assert.That(typeModel.MemberCount.TotalCount, Is.EqualTo(7));
		});
	}

	private static (SyntaxNode, ITypeSymbol, SemanticModel) GetInformation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		return (typeSyntax, model.GetDeclaredSymbol(typeSyntax)!, model);
	}
}