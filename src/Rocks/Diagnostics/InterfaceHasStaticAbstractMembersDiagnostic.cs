using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class InterfaceHasStaticAbstractMembersDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(InterfaceHasStaticAbstractMembersDescriptor.Create(),
			node.GetLocation(), type.GetName());
}