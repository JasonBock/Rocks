using System.Collections.Immutable;

namespace Rocks.Extensions;

internal sealed class MockableMethods
{
	internal MockableMethods(ImmutableArray<MethodMockableResult> results, bool hasInaccessibleAbstractMembers) =>
		(this.Results, this.HasInaccessibleAbstractMembers) = (results, hasInaccessibleAbstractMembers);

	internal bool HasInaccessibleAbstractMembers { get; }
	internal ImmutableArray<MethodMockableResult> Results { get; }
}