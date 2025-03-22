using Rocks.Models;

namespace Rocks.Builders.Create;

internal sealed class TypeArgumentsNamingContext
	: NamingContext
{
	internal TypeArgumentsNamingContext()
		: base() { }

	internal TypeArgumentsNamingContext(ITypeReferenceModel type)
		: base([.. type.TypeArguments.Select(_ => _.FullyQualifiedName)])
	{ }

	internal TypeArgumentsNamingContext(MethodModel method)
		: base([.. method.MockType.TypeArguments.Intersect(method.TypeArguments, new TypeReferenceModelEqualityComparer()).Select(_ => _.FullyQualifiedName)])
	{ }

	private sealed class TypeReferenceModelEqualityComparer
		: EqualityComparer<ITypeReferenceModel>
	{
		public override bool Equals(ITypeReferenceModel x, ITypeReferenceModel y) =>
			x.FullyQualifiedName == y.FullyQualifiedName;

		public override int GetHashCode(ITypeReferenceModel obj) => obj.FullyQualifiedName.GetHashCode();
	}
}