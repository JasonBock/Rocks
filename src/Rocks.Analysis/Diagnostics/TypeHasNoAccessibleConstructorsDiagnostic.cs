using Microsoft.CodeAnalysis;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Diagnostics;

internal static class TypeHasNoAccessibleConstructorsDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(TypeHasNoAccessibleConstructorsDescriptor.Create(),
			node.GetLocation(), type.GetName());
}