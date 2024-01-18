using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class TypeErrorDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(TypeErrorDescriptor.Create(),
			node.GetLocation(), type.GetName());
}