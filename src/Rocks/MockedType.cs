using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks;

internal sealed class MockedType
{
	internal MockedType(ITypeSymbol type) =>
		(this.Type, this.FlattenedName, this.ReferenceableName) = 
			(type, type.GetName(TypeNameOption.Flatten), type.GetFullyQualifiedName());

	internal string FlattenedName { get; }
	internal string ReferenceableName { get; }
	internal ITypeSymbol Type { get; }
}