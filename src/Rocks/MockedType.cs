using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks;

internal sealed class MockedType
{
	internal MockedType(ITypeSymbol type, Compilation compilation) =>
		(this.Type, this.FlattenedName, this.ReferenceableName) = 
			(type, type.GetName(TypeNameOption.Flatten), type.GetFullyQualifiedName(compilation));

	internal string FlattenedName { get; }
	internal string ReferenceableName { get; }
	internal ITypeSymbol Type { get; }
}