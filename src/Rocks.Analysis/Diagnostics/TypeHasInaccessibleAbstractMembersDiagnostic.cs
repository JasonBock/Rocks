using Microsoft.CodeAnalysis;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Diagnostics;

internal static class TypeHasInaccessibleAbstractMembersDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(TypeHasInaccessibleAbstractMembersDescriptor.Create(),
			node.GetLocation(), type.GetName());
}