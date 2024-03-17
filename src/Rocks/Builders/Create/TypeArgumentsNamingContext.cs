using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal sealed class TypeArgumentsNamingContext
	: NamingContext
{
	internal TypeArgumentsNamingContext()
		: base() { }

	internal TypeArgumentsNamingContext(TypeReferenceModel type)
		: base(type.TypeArguments.Select(_ => _.FullyQualifiedName).ToImmutableHashSet())
	{ }

	internal TypeArgumentsNamingContext(MethodModel method)
		: base(method.MockType.TypeArguments.Intersect(method.TypeArguments, new TypeReferenceModelEqualityComparer()).Select(_ => _.FullyQualifiedName).ToImmutableHashSet())
	{ }

	private sealed class TypeReferenceModelEqualityComparer
		: EqualityComparer<TypeReferenceModel>
	{
		public override bool Equals(TypeReferenceModel x, TypeReferenceModel y) =>
			x.FullyQualifiedName == y.FullyQualifiedName;

		public override int GetHashCode(TypeReferenceModel obj) => obj.FullyQualifiedName.GetHashCode();
	}
}