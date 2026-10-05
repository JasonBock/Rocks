using System.Collections.Immutable;

namespace Rocks.Analysis.Discovery;

internal sealed class MockableProperties
{
	internal MockableProperties(
		ImmutableArray<MockablePropertyResult> results,
		ImmutableArray<MockablePropertyResult> inaccessibleAbstractMembers,
		bool hasStaticAbstractMembers) =>
		(this.Results, this.InaccessibleAbstractMembers, this.HasStaticAbstractMembers) =
			(results, inaccessibleAbstractMembers, hasStaticAbstractMembers);

	internal bool HasStaticAbstractMembers { get; }
	internal ImmutableArray<MockablePropertyResult> InaccessibleAbstractMembers { get; }
	internal ImmutableArray<MockablePropertyResult> Results { get; }
}