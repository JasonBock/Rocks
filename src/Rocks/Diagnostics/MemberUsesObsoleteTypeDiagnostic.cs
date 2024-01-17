using Microsoft.CodeAnalysis;
using Rocks.Descriptors;

namespace Rocks.Diagnostics;

internal static class MemberUsesObsoleteTypeDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ISymbol symbol) =>
		Diagnostic.Create(MemberUsesObsoleteTypeDescriptor.Create(),
			node.GetLocation(), symbol.Name);
}