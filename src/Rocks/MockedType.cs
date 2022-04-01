using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks;

internal sealed class MockedType
{
	internal MockedType(ITypeSymbol type) =>
		(this.Type, this.FlattenedName, this.GenericName) = 
			(type, type.GetName(TypeNameOption.Flatten), type.GetName(TypeNameOption.IncludeGenerics));

	internal string GenericName { get; }
	internal string FlattenedName { get; }
	internal ITypeSymbol Type { get; }
}