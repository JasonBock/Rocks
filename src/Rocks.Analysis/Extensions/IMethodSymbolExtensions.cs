using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;
using Rocks.Analysis.Models;
using System.Collections.Immutable;

namespace Rocks.Analysis.Extensions;

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

	internal static bool CanBeSeenByContainingAssembly(this IMethodSymbol self, IAssemblySymbol assembly,
		Compilation compilation) =>
		((ISymbol)self).CanBeSeenByContainingAssembly(assembly, compilation) &&
			self.Parameters.All(_ => _.Type.CanBeSeenByContainingAssembly(assembly, compilation)) &&
			self.TypeParameters.All(_ => _.CanBeSeenByContainingAssembly(assembly, compilation)) &&
			(self.ReturnsVoid || self.ReturnType.CanBeSeenByContainingAssembly(assembly, compilation));

	internal static bool IsMarkedWithDoesNotReturn(this IMethodSymbol self, Compilation compilation) =>
		self.GetAttributes().Any(
			_ => _.AttributeClass?.GetFullyQualifiedName(compilation).EndsWith(DoesNotReturnAttributeName) ?? false);

	internal static bool RequiresProjectedDelegate(this IMethodSymbol self, Compilation compilation) =>
		self.Parameters.Length > 16 ||
		self.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out || _.IsScoped() || _.Type.RequiresProjectedArgument(compilation)) ||
			!self.ReturnsVoid && self.ReturnType.RequiresProjectedArgument(compilation);

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
		static bool IsAnnotated(ITypeSymbol type, ITypeParameterSymbol typeParameter) =>
			(type.Equals(typeParameter) && type.NullableAnnotation == NullableAnnotation.Annotated) ||
				type.IsOpenGeneric() && ((type as INamedTypeSymbol)?.TypeArguments.Any(_ => IsAnnotated(_, typeParameter)) ?? false);

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
					constraints.Add(new Constraints(typeParameter.GetName(), ["class"]));
				}
				else if (typeParameter.HasValueTypeConstraint)
				{
					constraints.Add(new Constraints(typeParameter.GetName(), ["struct"]));
				}
				else if (self.Parameters.Any(_ => IsAnnotated(_.Type, typeParameter)) ||
					(!self.ReturnsVoid && IsAnnotated(self.ReturnType, typeParameter)))
				{
					constraints.Add(new Constraints(typeParameter.GetName(), ["default"]));
				}
			}

			return [.. constraints];
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

		return [.. constraints];
	}

	internal static MethodMatch Match(this IMethodSymbol self, IMethodSymbol other, Compilation compilation)
	{
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

			var hasDiffByConstraint = false;

			for (var i = 0; i < selfParameters.Length; i++)
			{
				var selfParameter = selfParameters[i];
				var otherParameter = otherParameters[i];

				if (selfParameter.Type is IArrayTypeSymbol selfArray && otherParameter.Type is IArrayTypeSymbol otherArray)
				{
					if (selfArray.Rank != otherArray.Rank ||
						selfArray.ElementType.GetMatch(otherArray.ElementType, compilation) == TypeMatch.None)
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

					var parameterTypeMatch = selfParameterType.GetMatch(otherParameterType, compilation);

					if (parameterTypeMatch is TypeMatch.None)
					{
						return MethodMatch.None;
					}
					else
					{
						hasDiffByConstraint |= parameterTypeMatch == TypeMatch.DifferByConstraintsOnly;
					}
				}
			}

			return self.ReturnType.GetMatch(other.ReturnType, compilation) == TypeMatch.Exact ?
				hasDiffByConstraint ?
					MethodMatch.DifferByReturnTypeOrConstraintOnly :
					MethodMatch.Exact :
				MethodMatch.DifferByReturnTypeOrConstraintOnly;
		}
	}
}