using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	public static class RockCreateReceiverTests
	{
		[Test]
		public static async Task FindCandidatesWhenInvocationIsRockCreate()
		{
			var classDeclaration = (await SyntaxFactory.ParseSyntaxTree("var setups = Rock.Create<int>();")
				.GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

			var receiver = new RockCreateReceiver();
			receiver.OnVisitSyntaxNode(classDeclaration);

			Assert.Multiple(() =>
			{
				Assert.That(receiver.Candidates.Count, Is.EqualTo(1));
			});
		}

		[Test]
		public static async Task FindCandidatesWhenInvocationIsNotRockCreate()
		{
			var classDeclaration = (await SyntaxFactory.ParseSyntaxTree("var isEmpty = string.IsNullOrEmpty(\"a\");")
				.GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

			var receiver = new RockCreateReceiver();
			receiver.OnVisitSyntaxNode(classDeclaration);

			Assert.Multiple(() =>
			{
				Assert.That(receiver.Candidates.Count, Is.EqualTo(0));
			});
		}
	}
}