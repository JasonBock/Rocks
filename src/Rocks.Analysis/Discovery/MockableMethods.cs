using System.Collections.Immutable;

namespace Rocks.Analysis.Discovery;

internal sealed class MockableMethods
{
	internal MockableMethods(
		ImmutableArray<MockableMethodResult> results,
		ImmutableArray<MockableMethodResult> inaccessibleAbstractMembers,
		bool hasStaticAbstractMembers) =>
		(this.Results, this.InaccessibleAbstractMembers, this.HasStaticAbstractMembers) =
			(results, inaccessibleAbstractMembers, hasStaticAbstractMembers);

	internal bool HasStaticAbstractMembers { get; }
	internal ImmutableArray<MockableMethodResult> InaccessibleAbstractMembers { get; }
	internal ImmutableArray<MockableMethodResult> Results { get; }
}