using System.Collections.Immutable;

namespace Rocks.Extensions;

internal sealed class MockableEvents
{
	internal MockableEvents(ImmutableArray<EventMockableResult> results, bool hasInaccessibleAbstractMembers) =>
		(this.Results, this.HasInaccessibleAbstractMembers) = (results, hasInaccessibleAbstractMembers);

	internal bool HasInaccessibleAbstractMembers { get; }
	internal ImmutableArray<EventMockableResult> Results { get; }
}