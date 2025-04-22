using Microsoft.CodeAnalysis;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Diagnostics;

internal static class InterfaceHasStaticAbstractMembersDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(InterfaceHasStaticAbstractMembersDescriptor.Create(),
			node.GetLocation(), type.GetName());
}