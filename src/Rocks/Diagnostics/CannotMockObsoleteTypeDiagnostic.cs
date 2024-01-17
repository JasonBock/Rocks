using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class CannotMockObsoleteTypeDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(CannotMockObsoleteTypeDescriptor.Create(),
			node.GetLocation(), type.GetName());
}