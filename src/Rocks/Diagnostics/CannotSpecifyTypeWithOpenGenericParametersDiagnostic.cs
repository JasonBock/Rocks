using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;

namespace Rocks.Diagnostics;

internal static class CannotSpecifyTypeWithOpenGenericParametersDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(CannotSpecifyTypeWithOpenGenericParametersDescriptor.Create(),
			node.GetLocation(), type.GetName());
}