using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Rocks
{
	public sealed class RockCreateReceiver
		: ISyntaxReceiver
	{
		public List<InvocationExpressionSyntax> Candidates { get; } = new List<InvocationExpressionSyntax>();

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			var repo = new RockRepository();
			repo.Create<ISyntaxReceiver>();

			if (syntaxNode is InvocationExpressionSyntax invocation &&
				invocation.Expression is MemberAccessExpressionSyntax access &&
				access.Expression is IdentifierNameSyntax accessIdentifier &&
				access.Name is GenericNameSyntax accessName)
			{
				if ((accessIdentifier.Identifier.Text == nameof(Rock) &&
					accessName.Identifier.Text == nameof(Rock.Create)) ||
					(accessName.Identifier.Text == nameof(RockRepository.Create)))
				{
					this.Candidates.Add(invocation);
				}
			}
		}
	}
}