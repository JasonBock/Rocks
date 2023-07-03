using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class ITypeParameterSymbolExtensions
{
	internal static string GetConstraints(this ITypeParameterSymbol self)
	{
		var constraints = new List<string>();

		// Based on what I've read here:
		// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#1425-type-parameter-constraints
		// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/where-generic-type-constraint
		// ...
		// Things like notnull and unmanaged should go first

		// According to CS0449, if any of these constraints exist: 
		// 'class', 'struct', 'unmanaged', 'notnull', and 'default'
		// they should not be duplicated.
		// Side note, I don't know how to find if the 'default'
		// constraint exists.
		if (self.HasUnmanagedTypeConstraint)
		{
			constraints.Add("unmanaged");
		}
		else if (self.IsNotNullRequired())
		{
			constraints.Add("notnull");
		}
		// Then class constraint (HasReferenceTypeConstraint) or struct (HasValueTypeConstraint)
		else if (self.HasReferenceTypeConstraint)
		{
			constraints.Add(self.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated ? "class?" : "class");
		}
		else if (self.HasValueTypeConstraint)
		{
			constraints.Add("struct");
		}

		// Then type constraints (classes first, then interfaces, then other generic type parameters)
		constraints.AddRange(self.ConstraintTypes.Where(_ => _.TypeKind == TypeKind.Class).Select(_ => _.GetFullyQualifiedName()));
		constraints.AddRange(self.ConstraintTypes.Where(_ => _.TypeKind == TypeKind.Interface).Select(_ => _.GetFullyQualifiedName()));
		constraints.AddRange(self.ConstraintTypes.Where(_ => _.TypeKind == TypeKind.TypeParameter).Select(_ => _.GetFullyQualifiedName()));

		// Then constructor constraint
		if (self.HasConstructorConstraint)
		{
			constraints.Add("new()");
		}

		return constraints.Count == 0 ? string.Empty :
			$"where {self.Name} : {string.Join(", ", constraints)}";
	}

	private static bool IsNotNullRequired(this ITypeParameterSymbol self)
	{
		if (self.HasNotNullConstraint)
		{
			return true;
		}

		// This gets complicated. I need to look at the original definition, and see if
		// there are any type parameter constraints on it if it's an interface.
		// If there are, THEN go look
		// to see if that is now defined on the type, and if that type doesn't have
		// nullability on it.
		var originalSelf = self.OriginalDefinition;

		if (originalSelf.ContainingType.TypeKind == TypeKind.Interface)
		{
			foreach (var originalConstraintType in originalSelf.ConstraintTypes.Where(_ => _.TypeKind == TypeKind.TypeParameter))
			{
				var selfContainingType = self.ContainingType;
				var selfContainingTypeParameter = selfContainingType.TypeParameters.SingleOrDefault(_ => _.Name == originalConstraintType.Name);

				if(selfContainingTypeParameter is not null)
				{
					var typeParameterIndex = selfContainingType.TypeParameters.IndexOf(selfContainingTypeParameter);

					if (selfContainingType.TypeArguments[typeParameterIndex].NullableAnnotation != NullableAnnotation.Annotated)
					{
						return true;
					}
				}
			}
		}

		return false;
	}
}