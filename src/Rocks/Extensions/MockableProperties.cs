using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal sealed class MockableProperties
{
	internal MockableProperties(ImmutableArray<PropertyMockableResult> results, ImmutableArray<IPropertySymbol> inaccessibleAbstractMembers) =>
		(this.Results, this.InaccessibleAbstractMembers) = (results, inaccessibleAbstractMembers);

	internal ImmutableArray<IPropertySymbol> InaccessibleAbstractMembers { get; }
	internal ImmutableArray<PropertyMockableResult> Results { get; }
}