using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class TypeHasNoAccessibleConstructorsDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(TypeHasNoAccessibleConstructorsDescriptor.Create(),
			node.GetLocation(), type.GetName());
}