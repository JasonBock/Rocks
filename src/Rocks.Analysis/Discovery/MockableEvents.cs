using System.Collections.Immutable;

namespace Rocks.Analysis.Discovery;

internal sealed class MockableEvents
{
	internal MockableEvents(
		ImmutableArray<MockableEventResult> results,
		ImmutableArray<MockableEventResult> inaccessibleAbstractMembers) =>
		(this.Results, this.InaccessibleAbstractMembers) = (results, inaccessibleAbstractMembers);

	internal ImmutableArray<MockableEventResult> InaccessibleAbstractMembers { get; }
	internal ImmutableArray<MockableEventResult> Results { get; }
}