﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	public static class RockReceiverTests
	{
		[Test]
		public static async Task FindCandidatesWhenInvocationIsRockCreate()
		{
			var classDeclaration = (await SyntaxFactory.ParseSyntaxTree(
@"var setups = Rock.Create<int>()").GetRootAsync().ConfigureAwait(false)).DescendantNodes(_ => true).OfType<InvocationExpressionSyntax>().First();

			var receiver = new RockReceiver();
			receiver.OnVisitSyntaxNode(classDeclaration);

			Assert.Multiple(() =>
			{
				Assert.That(receiver.Candidates.Count, Is.EqualTo(1));
			});
		}
	}
}