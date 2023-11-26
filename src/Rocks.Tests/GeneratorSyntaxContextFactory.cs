using Microsoft.CodeAnalysis;
using System.Reflection;

namespace Rocks.Tests;

internal static class GeneratorSyntaxContextFactory
{
	internal static GeneratorSyntaxContext Create(SyntaxNode node, SemanticModel model)
	{
		// I. Am. A. STUBBORN. Person.
		// http://sourceroslyn.io/#Microsoft.CodeAnalysis/SourceGeneration/GeneratorContexts.cs,184
		var contextConstructor = typeof(GeneratorSyntaxContext).GetConstructor(
			BindingFlags.Instance | BindingFlags.NonPublic, null,
			[typeof(SyntaxNode), typeof(SemanticModel)], null)!;
		return (GeneratorSyntaxContext)contextConstructor.Invoke(new object[] { node, model });
	}
}