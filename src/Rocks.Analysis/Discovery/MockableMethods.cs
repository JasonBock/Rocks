using System.Collections.Immutable;

namespace Rocks.Analysis.Discovery;

internal sealed class MockableMethods
{
   internal MockableMethods(
		ImmutableArray<MockableMethodResult> results,
	   bool hasInaccessibleAbstractMembers,
		bool hasStaticAbstractMembers) =>
		(this.Results, this.HasInaccessibleAbstractMembers, this.HasStaticAbstractMembers) =
			(results, hasInaccessibleAbstractMembers, hasStaticAbstractMembers);

	internal bool HasStaticAbstractMembers { get; }
	internal bool HasInaccessibleAbstractMembers { get; }
   internal ImmutableArray<MockableMethodResult> Results { get; }
}