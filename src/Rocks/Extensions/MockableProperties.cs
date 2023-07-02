using System.Collections.Immutable;

namespace Rocks.Extensions;

internal sealed class MockableProperties
{
	internal MockableProperties(ImmutableArray<PropertyMockableResult> results, bool hasInaccessibleAbstractMembers) =>
		(this.Results, this.HasInaccessibleAbstractMembers) = (results, hasInaccessibleAbstractMembers);

	internal bool HasInaccessibleAbstractMembers { get; }
	internal ImmutableArray<PropertyMockableResult> Results { get; }
}