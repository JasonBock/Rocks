using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class CannotMockSealedTypeDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(CannotMockSealedTypeDescriptor.Create(),
			node.GetLocation(), type.GetName());
}