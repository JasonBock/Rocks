//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using NUnit.Framework;

//namespace Rocks.Tests;

//public static class RockCreateReceiverTests
//{
//	[Test]
//	public static async Task FindCandidatesWhenInvocationIsRockCreateAsync()
//	{
//		var code =
//@"using Rocks;

//public static class Invoker
//{
//	public static void Invoke()
//	{
//		var setups = Rock.Create<int>();
//	}
//}";
//		var invocationNode = (await SyntaxFactory.ParseSyntaxTree(code)
//			.GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

//		var receiver = new RockCreateReceiver();
//		var context = RockCreateReceiverTests.GetContext(invocationNode);
//		receiver.OnVisitSyntaxNode(context);

//		Assert.That(receiver.Targets.Count, Is.EqualTo(1));
//	}

//	[Test]
//	public static async Task FindCandidatesWhenInvocationIsCreateAsync()
//	{
//		var code =
//@"using Rocks;

//public static class Invoker
//{
//	public static void Invoke()
//	{
//		var repo = new RockRepository();
//		var setups = repo.Create<int>();
//	}
//}";
//		var invocationNode = (await SyntaxFactory.ParseSyntaxTree(code)
//			.GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

//		var receiver = new RockCreateReceiver();
//		var context = RockCreateReceiverTests.GetContext(invocationNode);
//		receiver.OnVisitSyntaxNode(context);

//		Assert.That(receiver.Targets.Count, Is.EqualTo(1));
//	}

//	[Test]
//	public static async Task FindCandidatesWhenInvocationIsNotRockCreateAsync()
//	{
//		var code =
//@"using Rocks;

//public static class Invoker
//{
//	public static void Invoke()
//	{
//		var isEmpty = string.IsNullOrEmpty(""a"");
//	}
//}";
//		var invocationNode = (await SyntaxFactory.ParseSyntaxTree(code)
//			.GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

//		var receiver = new RockCreateReceiver();
//		var context = RockCreateReceiverTests.GetContext(invocationNode);
//		receiver.OnVisitSyntaxNode(context);

//		Assert.That(receiver.Targets.Count, Is.EqualTo(0));
//	}

//	private static GeneratorSyntaxContext GetContext(SyntaxNode node)
//	{
//		var tree = node.SyntaxTree;
//		var references = AppDomain.CurrentDomain.GetAssemblies()
//			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
//			.Select(_ => MetadataReference.CreateFromFile(_.Location))
//			.Concat(new[] { MetadataReference.CreateFromFile(typeof(RockCreateGenerator).Assembly.Location) });
//		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { tree },
//			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
//		var model = compilation.GetSemanticModel(tree);

//		return GeneratorSyntaxContextFactory.Create(node, model);
//	}
//}