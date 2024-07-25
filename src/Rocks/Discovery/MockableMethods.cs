using System.Collections.Immutable;

namespace Rocks.Discovery;

internal sealed class MockableMethods
{
   internal MockableMethods(ImmutableArray<MockableMethodResult> results,
	   bool hasInaccessibleAbstractMembers) =>
		   (this.Results, this.HasInaccessibleAbstractMembers) =
			   (results, hasInaccessibleAbstractMembers);

   internal bool HasInaccessibleAbstractMembers { get; }
   internal ImmutableArray<MockableMethodResult> Results { get; }
}