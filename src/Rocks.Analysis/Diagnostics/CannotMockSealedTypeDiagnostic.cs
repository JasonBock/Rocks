using Microsoft.CodeAnalysis;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Diagnostics;

internal static class CannotMockSealedTypeDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(CannotMockSealedTypeDescriptor.Create(),
			node.GetLocation(), type.GetName());
}