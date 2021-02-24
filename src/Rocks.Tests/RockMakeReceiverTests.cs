using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	public static class RockMakeReceiverTests
	{
		[Test]
		public static async Task FindCandidatesWhenInvocationIsRockMake()
		{
			var classDeclaration = (await SyntaxFactory.ParseSyntaxTree("var setups = Rock.Make<int>();")
				.GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

			var receiver = new RockMakeReceiver();
			receiver.OnVisitSyntaxNode(classDeclaration);

			Assert.That(receiver.Candidates.Count, Is.EqualTo(1));
		}

		[Test]
		public static async Task FindCandidatesWhenInvocationIsNotRockCreate()
		{
			var classDeclaration = (await SyntaxFactory.ParseSyntaxTree("var isEmpty = string.IsNullOrEmpty(\"a\");")
				.GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

			var receiver = new RockMakeReceiver();
			receiver.OnVisitSyntaxNode(classDeclaration);

			Assert.That(receiver.Candidates.Count, Is.EqualTo(0));
		}
	}
}