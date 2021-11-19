using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

internal static class ITypeParameterSymbolExtensions
{
	internal static string GetConstraints(this ITypeParameterSymbol self)
	{
		var constraints = new List<string>();
		constraints.AddRange(self.ConstraintTypes.Select(_ => _.GetName()));

		if (self.HasReferenceTypeConstraint)
		{
			constraints.Add(self.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated ? "class?" : "class");
		}

		if (self.HasConstructorConstraint)
		{
			constraints.Add("new()");
		}

		if (self.HasNotNullConstraint)
		{
			constraints.Add("notnull");
		}

		if (self.HasUnmanagedTypeConstraint)
		{
			constraints.Add("unmanaged");
		}

		if (self.HasValueTypeConstraint)
		{
			constraints.Add("struct");
		}

		return constraints.Count == 0 ? string.Empty :
			$"where {self.Name} : {string.Join(", ", constraints)}";
	}
}