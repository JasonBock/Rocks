using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class TypeHasMatchWithNonVirtualDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(TypeHasMatchWithNonVirtualDescriptor.Create(),
			node.GetLocation(), type.GetName());
}