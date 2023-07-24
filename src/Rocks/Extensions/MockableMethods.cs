using System.Collections.Immutable;

namespace Rocks.Extensions;

internal sealed class MockableMethods
{
	internal MockableMethods(
		ImmutableArray<MethodMockableResult> results, bool hasInaccessibleAbstractMembers,
		bool hasMatchWithNonVirtual) =>
			(this.Results, this.HasInaccessibleAbstractMembers, this.HasMatchWithNonVirtual) = 
				(results, hasInaccessibleAbstractMembers, hasMatchWithNonVirtual);

	internal bool HasInaccessibleAbstractMembers { get; }
	internal bool HasMatchWithNonVirtual { get; }
	internal ImmutableArray<MethodMockableResult> Results { get; }
}