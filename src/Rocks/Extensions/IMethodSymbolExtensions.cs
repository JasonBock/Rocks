using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IMethodSymbolExtensions
{
	private const string DoesNotReturnAttributeName =
		"System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute";

   internal static Diagnostic? GetObsoleteDiagnostic(
	   this IMethodSymbol self, SyntaxNode node, INamedTypeSymbol obsoleteAttribute) =>
			self.Parameters.Any(_ => _.Type.IsObsolete(obsoleteAttribute)) ||
				self.TypeParameters.Any(_ => _.IsObsolete(obsoleteAttribute)) ||
				!self.ReturnsVoid && self.ReturnType.IsObsolete(obsoleteAttribute) ? 
				MemberUsesObsoleteTypeDiagnostic.Create(node, self) : null;

   internal static bool CanBeSeenByContainingAssembly(this IMethodSymbol self, IAssemblySymbol assembly) =>
		((ISymbol)self).CanBeSeenByContainingAssembly(assembly) &&
			self.Parameters.All(_ => _.Type.CanBeSeenByContainingAssembly(assembly)) &&
			self.TypeParameters.All(_ => _.CanBeSeenByContainingAssembly(assembly)) &&
			(self.ReturnsVoid || self.ReturnType.CanBeSeenByContainingAssembly(assembly));

	internal static bool IsMarkedWithDoesNotReturn(this IMethodSymbol self, Compilation compilation) =>
		self.GetAttributes().Any(
			_ => _.AttributeClass?.GetFullyQualifiedName(compilation).EndsWith(IMethodSymbolExtensions.DoesNotReturnAttributeName) ?? false);

	internal static bool RequiresProjectedDelegate(this IMethodSymbol self) =>
		self.Parameters.Length > 16 ||
		self.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out || _.Type.IsEsoteric()) ||
			!self.ReturnsVoid && self.ReturnType.IsEsoteric();

	/// <summary>
	/// This is needed because if a method has a generic parameter that is used 
	/// either with a method parameter or the return value and declares the type to be nullable,
	/// the override must create a <c>where T : default</c> constraint.
	/// Otherwise, CS0508 and CS0453 will occur.
	/// </summary>
	/// <param name="self">The target method.</param>
	/// <returns>A list of default constraints.</returns>
	internal static EquatableArray<Constraints> GetDefaultConstraints(this IMethodSymbol self)
	{
		static bool IsAnnotated(ITypeSymbol type, ITypeParameterSymbol typeParameter)
		{
			return (type.Equals(typeParameter) && type.NullableAnnotation == NullableAnnotation.Annotated) ||
				type.IsOpenGeneric() && ((type as INamedTypeSymbol)?.TypeArguments.Any(_ => IsAnnotated(_, typeParameter)) ?? false);
		}

		if (self.TypeParameters.Length > 0)
		{
			var constraints = new List<Constraints>();

			foreach (var typeParameter in self.TypeParameters)
			{
				// TODO: This is starting to get convoluted.
				// Arguably, it would be good to have one method that gets
				// all constraints for a method, rather than doing it via
				// GetConstraints() and GetDefaultConstraints().
				if (typeParameter.HasReferenceTypeConstraint)
				{
					constraints.Add(new Constraints(typeParameter.GetName(), new[] { "class" }.ToImmutableArray()));
				}
				else if (typeParameter.HasValueTypeConstraint)
				{
					constraints.Add(new Constraints(typeParameter.GetName(), new[] { "struct" }.ToImmutableArray()));
				}
				else if (self.Parameters.Any(_ => IsAnnotated(_.Type, typeParameter)) ||
					(!self.ReturnsVoid && IsAnnotated(self.ReturnType, typeParameter)))
				{
					constraints.Add(new Constraints(typeParameter.GetName(), new[] { "default" }.ToImmutableArray()));
				}
			}

			return constraints.ToImmutableArray();
		}
		else
		{
			return ImmutableArray<Constraints>.Empty;
		}
	}

	internal static bool IsUnsafe(this IMethodSymbol self) =>
		self.Parameters.Any(_ => _.Type.IsPointer()) || (!self.ReturnsVoid && self.ReturnType.IsPointer());

	internal static EquatableArray<Constraints> GetConstraints(this IMethodSymbol self, Compilation compilation)
	{
		var constraints = new List<Constraints>();

		if (self.TypeParameters.Length > 0)
		{
			for (var i = 0; i < self.TypeParameters.Length; i++)
			{
				var typeParameter = self.TypeParameters[i];

				if (typeParameter.Equals(self.TypeArguments[i]))
				{
					var constraint = typeParameter.GetConstraints(compilation);

					if (constraint is not null)
					{
						constraints.Add(constraint);
					}
				}
			}
		}

		return constraints.ToImmutableArray();
	}

	internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IMethodSymbol self)
	{
		static IEnumerable<INamespaceSymbol> GetParameterNamespaces(IParameterSymbol parameter)
		{
			foreach (var parameterTypeNamespace in parameter.Type.GetNamespaces())
			{
				yield return parameterTypeNamespace;
			}

			foreach (var attributeNamespace in parameter.GetAttributes().SelectMany(_ => _.GetNamespaces()))
			{
				yield return attributeNamespace;
			}
		}

		var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

		if (!self.ReturnsVoid)
		{
			namespaces.AddRange(self.ReturnType.GetNamespaces());
			namespaces.AddRange(self.GetReturnTypeAttributes().SelectMany(_ => _.GetNamespaces()));
		}

		namespaces.AddRange(self.GetAttributes().SelectMany(_ => _.GetNamespaces()));
		namespaces.AddRange(self.Parameters.SelectMany(_ => GetParameterNamespaces(_)));

		return namespaces.ToImmutable();
	}

	internal static MethodMatch Match(this IMethodSymbol self, IMethodSymbol other)
	{
		static bool DoTypesMatch(ITypeSymbol left, ITypeSymbol right)
		{
			if (left is INamedTypeSymbol namedLeft && right is INamedTypeSymbol namedRight)
			{
				var leftType = namedLeft.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
				var rightType = namedRight.WithNullableAnnotation(NullableAnnotation.NotAnnotated);

				var symbolFormatterNoGenerics = SymbolDisplayFormat.FullyQualifiedFormat
					.WithGenericsOptions(SymbolDisplayGenericsOptions.None)
					.AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

				if (leftType.ToDisplayString(symbolFormatterNoGenerics) !=
					rightType.ToDisplayString(symbolFormatterNoGenerics))
				{
					return false;
				}
				else
				{
					// Now we know the type name sans generics are the same. Now we check each type parameter
					// if the type is generic. And we need to be recursive about it.
					if (namedLeft.TypeParameters.Length != namedRight.TypeParameters.Length)
					{
						return false;
					}
					else
					{
						for (var i = 0; i < namedLeft.TypeParameters.Length; i++)
						{
							if (!(namedLeft.TypeArguments[i].TypeKind == TypeKind.TypeParameter &&
								namedRight.TypeArguments[i].TypeKind == TypeKind.TypeParameter))
							{
								// At this point, we know that the type arguments have been provided with types
								// i.e. they're "closed". Therefore, we can continue, because comparing names like
								// "T" and "U" is meaningless.
								if (!DoTypesMatch(namedLeft.TypeArguments[i], namedRight.TypeArguments[i]))
								{
									return false;
								}
							}
						}
					}

					return true;
				}
			}
			else
			{
				return SymbolEqualityComparer.Default.Equals(left, right);
			}
		}

		if (self.Name != other.Name)
		{
			return MethodMatch.None;
		}
		else
		{
			if (self.TypeParameters.Length != other.TypeParameters.Length)
			{
				return MethodMatch.None;
			}

			var selfParameters = self.Parameters;
			var otherParameters = other.Parameters;

			if (selfParameters.Length != otherParameters.Length)
			{
				return MethodMatch.None;
			}

			for (var i = 0; i < selfParameters.Length; i++)
			{
				var selfParameter = selfParameters[i];
				var otherParameter = otherParameters[i];

				if (selfParameter.Type is IArrayTypeSymbol selfArray && otherParameter.Type is IArrayTypeSymbol otherArray)
				{
					if (selfArray.Rank != otherArray.Rank ||
						!DoTypesMatch(selfArray.ElementType, otherArray.ElementType))
					{
						return MethodMatch.None;
					}
					else
					{
						continue;
					}
				}
				else if (!(selfParameter.RefKind == otherParameter.RefKind ||
					(selfParameter.RefKind == RefKind.Ref && otherParameter.RefKind == RefKind.Out) ||
					(selfParameter.RefKind == RefKind.Out && otherParameter.RefKind == RefKind.Ref)) ||
					selfParameter.IsParams != otherParameter.IsParams)
				{
					return MethodMatch.None;
				}
				else
				{
					var selfParameterType = selfParameter.Type;
					var otherParameterType = otherParameter.Type;

					if ((selfParameterType.TypeKind == TypeKind.TypeParameter && otherParameterType.TypeKind == TypeKind.TypeParameter) ||
						DoTypesMatch(selfParameterType, otherParameterType))
					{
						// These are type parameters so we don't need to compare them, or the types match, move on.
						continue;
					}
					else
					{
						return MethodMatch.None;
					}
				}
			}

			return DoTypesMatch(self.ReturnType, other.ReturnType) ?
				MethodMatch.Exact : MethodMatch.DifferByReturnTypeOnly;
		}
	}
}