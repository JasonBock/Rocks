using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class TypeIsClosedGenericDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(TypeIsClosedGenericDescriptor.Create(),
			node.GetLocation(), type.GetName());
}