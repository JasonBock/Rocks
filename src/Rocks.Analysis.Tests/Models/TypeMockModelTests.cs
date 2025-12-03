using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class TypeMockModelTests
{
	[Test]
	public static async Task VerifyMemberCountWhenNoMethodsExistAsync()
	{
		(var node, var type, var model) = await TypeMockModelTests.GetInformationAsync(
			"""
			using System;
			
			public interface ITarget
			{
				string Data { get; set; }
			}

			""");
		var mockModel = MockModel.Create(node, type, null, model, BuildType.Create, false);
		var typeModel = mockModel.Information!.Type;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(typeModel.MemberCount.MethodCount, Is.EqualTo(0));
			Assert.That(typeModel.MemberCount.PropertyCount, Is.EqualTo(2));
			Assert.That(typeModel.MemberCount.TotalCount, Is.EqualTo(2));
		}
	}

	[Test]
	public static async Task VerifyMemberCountWhenNoPropertiesExistAsync()
	{
		(var node, var type, var model) = await TypeMockModelTests.GetInformationAsync(
			"""
			using System;
			
			public interface ITarget
			{
				void Work();
			}

			""");
		var mockModel = MockModel.Create(node, type, null, model, BuildType.Create, false);
		var typeModel = mockModel.Information!.Type;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(typeModel.MemberCount.MethodCount, Is.EqualTo(1));
			Assert.That(typeModel.MemberCount.PropertyCount, Is.EqualTo(0));
			Assert.That(typeModel.MemberCount.TotalCount, Is.EqualTo(1));
		}
	}

	[Test]
	public static async Task VerifyMemberCountWithMethodPropertyMixAsync()
	{
		(var node, var type, var modelContext) = await TypeMockModelTests.GetInformationAsync(
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
		var mockModel = MockModel.Create(node, type, null, modelContext, BuildType.Create, false);
		var typeModel = mockModel.Information!.Type;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(typeModel.MemberCount.MethodCount, Is.EqualTo(3));
			Assert.That(typeModel.MemberCount.PropertyCount, Is.EqualTo(4));
			Assert.That(typeModel.MemberCount.TotalCount, Is.EqualTo(7));
		}
	}

	private static async Task<(SyntaxNode, ITypeSymbol, ModelContext)> GetInformationAsync(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		return (typeSyntax, model.GetDeclaredSymbol(typeSyntax)!, new(model));
	}
}