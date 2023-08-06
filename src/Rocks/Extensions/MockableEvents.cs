using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal sealed class MockableEvents
{
	internal MockableEvents(ImmutableArray<EventMockableResult> results, ImmutableArray<IEventSymbol> inaccessibleAbstractMembers) =>
		(this.Results, this.InaccessibleAbstractMembers) = (results, inaccessibleAbstractMembers);

	internal ImmutableArray<IEventSymbol> InaccessibleAbstractMembers { get; }
	internal ImmutableArray<EventMockableResult> Results { get; }
}