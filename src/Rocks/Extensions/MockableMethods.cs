using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal sealed class MockableMethods
{
	internal MockableMethods(ImmutableArray<MethodMockableResult> results, 
		ImmutableArray<IMethodSymbol> inaccessibleAbstractMembers, bool hasMatchWithNonVirtual) =>
			(this.Results, this.InaccessibleAbstractMembers, this.HasMatchWithNonVirtual) = 
				(results, inaccessibleAbstractMembers, hasMatchWithNonVirtual);

	internal ImmutableArray<IMethodSymbol> InaccessibleAbstractMembers { get; }
	internal bool HasMatchWithNonVirtual { get; }
	internal ImmutableArray<MethodMockableResult> Results { get; }
}