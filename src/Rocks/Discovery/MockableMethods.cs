using System.Collections.Immutable;

namespace Rocks.Discovery;

internal sealed class MockableMethods
{
   internal MockableMethods(ImmutableArray<MockableMethodResult> results,
	   bool hasInaccessibleAbstractMembers, bool hasMatchWithNonVirtual) =>
		   (this.Results, this.HasInaccessibleAbstractMembers, this.HasMatchWithNonVirtual) =
			   (results, hasInaccessibleAbstractMembers, hasMatchWithNonVirtual);

   internal bool HasInaccessibleAbstractMembers { get; }
   internal bool HasMatchWithNonVirtual { get; }
   internal ImmutableArray<MockableMethodResult> Results { get; }
}