using System.Collections.Immutable;

namespace Rocks.Discovery;

internal sealed class MockableProperties
{
   internal MockableProperties(ImmutableArray<MockablePropertyResult> results, bool hasInaccessibleAbstractMembers) =>
	   (this.Results, this.HasInaccessibleAbstractMembers) = (results, hasInaccessibleAbstractMembers);

   internal bool HasInaccessibleAbstractMembers { get; }
   internal ImmutableArray<MockablePropertyResult> Results { get; }
}