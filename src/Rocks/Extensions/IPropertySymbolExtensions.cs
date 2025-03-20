using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IPropertySymbolExtensions
{
   internal static Diagnostic? GetObsoleteDiagnostic(
	   this IPropertySymbol self, SyntaxNode node, INamedTypeSymbol obsoleteAttribute) => 
			self.Parameters.Any(_ => _.Type.IsObsolete(obsoleteAttribute)) ||
				self.Type.IsObsolete(obsoleteAttribute) ? 
				MemberUsesObsoleteTypeDiagnostic.Create(node, self) : null;

   internal static bool CanBeSeenByContainingAssembly(this IPropertySymbol self, IAssemblySymbol assembly,
		Compilation compilation) =>
		((ISymbol)self).CanBeSeenByContainingAssembly(assembly, compilation) &&
			self.Type.CanBeSeenByContainingAssembly(assembly, compilation) &&
			self.Parameters.All(_ => _.Type.CanBeSeenByContainingAssembly(assembly, compilation));

	internal static ImmutableArray<AttributeData> GetAllAttributes(this IPropertySymbol self)
	{
		var attributes = self.GetAttributes().ToList();

		// [AllowNull] shows up on the value parameter of the setter if the property
		// is not abstract (and has a setter), so we need to do go deeper.
		if (!self.IsAbstract && self.SetMethod is not null)
		{
			attributes.AddRange(self.SetMethod.Parameters[self.SetMethod.Parameters.Length - 1].GetAttributes());
		}

		return [.. attributes];
	}

	internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IPropertySymbol self)
	{
		static IEnumerable<INamespaceSymbol> GetParameterNamespaces(IParameterSymbol parameter)
		{
			foreach (var parameterTypeNamespaces in parameter.Type.GetNamespaces())
			{
				yield return parameterTypeNamespaces;
			}

			foreach (var attributeNamespace in parameter.GetAttributes().SelectMany(_ => _.GetNamespaces()))
			{
				yield return attributeNamespace;
			}
		}

		var namespaces = new HashSet<INamespaceSymbol>();

		namespaces.AddRange(self.GetAllAttributes().SelectMany(_ => _.GetNamespaces()));
		namespaces.AddRange(self.Type.GetNamespaces());

		if (self.IsIndexer)
		{
			namespaces.AddRange(self.Parameters.SelectMany(_ => GetParameterNamespaces(_)));
		}

		return [.. namespaces];
	}

	internal static bool IsUnsafe(this IPropertySymbol self) =>
		self.IsIndexer ? (self.Parameters.Any(_ => _.Type.IsPointer()) || self.Type.IsPointer()) : self.Type.IsPointer();

	internal static PropertyAccessor GetAccessors(this IPropertySymbol self)
	{
		if (self.GetMethod is not null && self.SetMethod is not null)
		{
			return self.SetMethod.IsInitOnly ? PropertyAccessor.GetAndInit : PropertyAccessor.GetAndSet;
		}
		else if (self.GetMethod is not null)
		{
			return PropertyAccessor.Get;
		}
		else
		{
			return self.SetMethod!.IsInitOnly ? PropertyAccessor.Init : PropertyAccessor.Set;
		}
	}
}