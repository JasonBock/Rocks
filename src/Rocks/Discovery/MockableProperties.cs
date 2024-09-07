using System.Collections.Immutable;

namespace Rocks.Discovery;

internal sealed class MockableProperties
{
	internal MockableProperties(
		ImmutableArray<MockablePropertyResult> results,
		bool hasInaccessibleAbstractMembers,
		bool hasStaticAbstractMembers) =>
		(this.Results, this.HasInaccessibleAbstractMembers, this.HasStaticAbstractMembers) =
			(results, hasInaccessibleAbstractMembers, hasStaticAbstractMembers);

	internal bool HasInaccessibleAbstractMembers { get; }
	internal bool HasStaticAbstractMembers { get; }
	internal ImmutableArray<MockablePropertyResult> Results { get; }
}