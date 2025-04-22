using System.Collections.Immutable;

namespace Rocks.Analysis.Discovery;

internal sealed class MockableEvents
{
   internal MockableEvents(ImmutableArray<MockableEventResult> results, bool hasInaccessibleAbstractMembers) =>
	   (this.Results, this.HasInaccessibleAbstractMembers) = (results, hasInaccessibleAbstractMembers);

   internal bool HasInaccessibleAbstractMembers { get; }
   internal ImmutableArray<MockableEventResult> Results { get; }
}