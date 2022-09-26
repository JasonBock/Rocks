using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks;

internal sealed class MockedType
{
	internal MockedType(ITypeSymbol type) =>
		(this.Type, this.FlattenedName, this.GenericName, this.ReferenceableName) = 
			(type, type.GetName(TypeNameOption.Flatten), type.GetName(TypeNameOption.IncludeGenerics), type.GetReferenceableName());

	internal string GenericName { get; }
	// TODO: I'm guessing GenericName can eventually go away
	// with ReferenceableName. FlattenedName I think has to stay. 
	internal string FlattenedName { get; }
	internal string ReferenceableName { get; }
	internal ITypeSymbol Type { get; }
}