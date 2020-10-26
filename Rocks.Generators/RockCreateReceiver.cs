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
			if (syntaxNode is InvocationExpressionSyntax invocation &&
				invocation.Expression is MemberAccessExpressionSyntax access &&
				access.Expression is IdentifierNameSyntax accessIdentifier &&
				accessIdentifier.Identifier.Text == "Rock" &&
				access.Name is GenericNameSyntax accessName &&
				accessName.Identifier.Text == "Create")
			{
				this.Candidates.Add(invocation);
			}
		}
	}
}