using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class ITypeSymbolExtensions
{
	internal static bool HasErrors(this ITypeSymbol self) =>
		self.TypeKind == TypeKind.Error ||
			(self is INamedTypeSymbol namedSelf && namedSelf.TypeArguments.Any(_ => _.HasErrors()));

	internal static (int count, ITypeSymbol pointerType) GetPointerInformation(this ITypeSymbol self)
	{
		var count = 0;
		var pointedAt = self;

		while (pointedAt.TypeKind == TypeKind.Pointer)
		{
			pointedAt = ((IPointerTypeSymbol)pointedAt).PointedAtType;
			count++;
		}

		return (count, pointedAt);
	}

	/*
	Does the given type has at leat one member that:
	* Is a method or property (but not an indexer)
	* Is not static
	* Is abstract
	* Cannot be referenced by name
	* Cannot be seen by containing assembly of mock invocation
	*/
	internal static bool HasInaccessibleAstractMembersWithInvalidIdentifiers(this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol)
	{
		static bool HasInaccessibleMembers(ITypeSymbol type, IAssemblySymbol containingAssemblyOfInvocationSymbol) =>
			type.GetMembers().Any(_ => (_.Kind == SymbolKind.Method || (_.Kind == SymbolKind.Property && !(_ as IPropertySymbol)!.IsIndexer)) &&
				!_.IsStatic && _.IsAbstract && !_.CanBeReferencedByName &&
				!_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol));

		return self.TypeKind == TypeKind.Interface ?
			HasInaccessibleMembers(self, containingAssemblyOfInvocationSymbol) ||
				self.AllInterfaces.Any(_ => HasInaccessibleMembers(_, containingAssemblyOfInvocationSymbol)) :
			self.GetInheritanceHierarchy().Any(_ => HasInaccessibleMembers(_, containingAssemblyOfInvocationSymbol));
	}

	internal static bool IsObsolete(this ITypeSymbol self, INamedTypeSymbol obsoleteAttribute) =>
		self.GetAttributes().Any(
			_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
				(_.ConstructorArguments.Any(_ => _.Value is bool error && error))) ||
		(self is INamedTypeSymbol namedSelf &&
			(namedSelf.TypeArguments.Any(
				_ => !_.Equals(self, SymbolEqualityComparer.Default) && _.IsObsolete(obsoleteAttribute)) ||
			namedSelf.TypeParameters.Any(
				_ => !_.Equals(self, SymbolEqualityComparer.Default) && _.ConstraintTypes.Any(
					_ => !_.Equals(self, SymbolEqualityComparer.Default) && _.IsObsolete(obsoleteAttribute)))));

	internal static bool CanBeSeenByContainingAssembly(this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol)
	{
		static bool AreTypeParametersVisible(ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol) =>
			self is not INamedTypeSymbol namedSelf ||
				namedSelf.TypeArguments.All(_ => _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol));

		if (self.TypeKind == TypeKind.TypeParameter ||
			self.TypeKind == TypeKind.Dynamic)
		{
			return true;
		}
		else if (self is IFunctionPointerTypeSymbol functionPointerSymbol)
		{
			var signature = functionPointerSymbol.Signature;
			return signature.Parameters.All(_ => _.Type.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)) &&
				(signature.ReturnsVoid || signature.ReturnType.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol));
		}
		else if (self is IPointerTypeSymbol pointerSymbol)
		{
			return pointerSymbol.PointedAtType.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);
		}
		else if (self is IArrayTypeSymbol arraySymbol)
		{
			return arraySymbol.ElementType.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);
		}
		else
		{
			if (self.DeclaredAccessibility == Accessibility.Public)
			{
				return AreTypeParametersVisible(self, containingAssemblyOfInvocationSymbol);
			}
			else if (self.DeclaredAccessibility == Accessibility.Internal ||
				self.DeclaredAccessibility == Accessibility.ProtectedOrInternal)
			{
				return (self.ContainingAssembly.Equals(containingAssemblyOfInvocationSymbol, SymbolEqualityComparer.Default) ||
					self.ContainingAssembly.ExposesInternalsTo(containingAssemblyOfInvocationSymbol)) &&
					AreTypeParametersVisible(self, containingAssemblyOfInvocationSymbol);
			}
			else
			{
				return false;
			}
		}
	}

	internal static bool IsOpenGeneric(this ITypeSymbol self) =>
		self switch
		{
			{ TypeKind: TypeKind.TypeParameter } => true,
			INamedTypeSymbol namedType => namedType.HasOpenGenerics(),
			_ => false
		};

	internal static bool IsPointer(this ITypeSymbol self) =>
		self.Kind == SymbolKind.PointerType || self.Kind == SymbolKind.FunctionPointerType;

	internal static bool IsEsoteric(this ITypeSymbol self) => self.IsPointer() || self.IsRefLikeType;

	internal static bool ContainsDiagnostics(this ITypeSymbol self) =>
		self.DeclaringSyntaxReferences.Any(syntax =>
			syntax.GetSyntax().GetDiagnostics().Any(_ => _.Severity == DiagnosticSeverity.Error));

	internal static string GetFullyQualifiedName(this ITypeSymbol self, Compilation compilation)
	{
		const string GlobalPrefix = "global::";

		var symbolFormatter = SymbolDisplayFormat.FullyQualifiedFormat
			.AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);
		var symbolName = self.ToDisplayString(symbolFormatter);

		// If the symbol name has "global::" at the start,
		// then see if the type's assembly has at least one alias.
		// If there is one, then replace "global::" with "{alias}::",
		// but only the FIRST "global::"

		// TODO: self could be a closed generic where the
		// type arguments need aliases. I should add a test for that to see
		// what ToDisplayString() would do in that case.

		if (symbolName.StartsWith(GlobalPrefix))
		{
			var aliases = compilation.GetMetadataReference(self.ContainingAssembly)?.Properties.Aliases ?? ImmutableArray<string>.Empty;

			if (aliases.Length > 0)
			{
				symbolName = $"{aliases[0]}::{symbolName.Remove(0, GlobalPrefix.Length)}";
			}
		}

		return symbolName;
	}

	// TODO: This method really needs to change.
	// It's doing WAY too much in too many different contexts.
	// I need to split this out and have methods that are well-focus and defined.
	// In fact, I think GetReferenceableName() is going to do most of the work,
	// and this can probably just be "GetFlattenedName()", which is needed
	// in project type name creation.
	internal static string GetName(this ITypeSymbol self, TypeNameOption options = TypeNameOption.IncludeGenerics)
	{
		static string GetFlattenedName(INamedTypeSymbol flattenedName, TypeNameOption flattenedOptions)
		{
			if (flattenedName.TypeArguments.Length == 0)
			{
				return flattenedName.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			}
			else
			{
				return $"{flattenedName.Name}Of{string.Join("_", flattenedName.TypeArguments.Select(_ => _.GetName(flattenedOptions)))}";
			}
		}

		static INamedTypeSymbol? GetContainingType(ITypeSymbol symbol)
		{
			if (symbol is IPointerTypeSymbol pointerSymbol)
			{
				return pointerSymbol.PointedAtType.ContainingType;
			}
			else
			{
				return symbol.ContainingType;
			}
		}

		if (options == TypeNameOption.IncludeGenerics)
		{
			if (self.Kind == SymbolKind.PointerType)
			{
				var name = self.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
				var containingType = GetContainingType(self);

				if (containingType is not null)
				{
					return $"{containingType.GetName(options)}.{name}";
				}
				else
				{
					return name;
				}
			}
			else
			{
				return self.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			}
		}
		else if (options == TypeNameOption.Flatten)
		{
			if (self.Kind == SymbolKind.PointerType)
			{
				return self.ToDisplayString().Replace(".", "_").Replace("<", "Of").Replace(">", string.Empty).Replace("*", "Pointer");
			}
			else if (self.Kind == SymbolKind.FunctionPointerType)
			{
				// delegate* unmanaged[Stdcall, SuppressGCTransition] <int, int>;
				return self.ToDisplayString().Replace("*", "Pointer").Replace(" ", "_")
					.Replace("[", "_").Replace(",", "_").Replace("]", "_")
					.Replace("<", "Of").Replace(">", string.Empty);
			}
			else if (self is INamedTypeSymbol namedSelf)
			{
				return GetFlattenedName(namedSelf, options);
			}
			else
			{
				return self.Name;
			}
		}
		else
		{
			return self.Kind == SymbolKind.PointerType || self.Kind == SymbolKind.FunctionPointerType ?
				self.ToDisplayString() : self.Name;
		}
	}

	internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this ITypeSymbol self)
	{
		var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

		namespaces.Add(self.ContainingNamespace);

		if (self is INamedTypeSymbol namedSelf)
		{
			namespaces.AddRange(namedSelf.TypeArguments.SelectMany(_ => _.GetNamespaces()));
		}

		return namespaces.ToImmutable();
	}

	internal static ImmutableArray<ITypeSymbol> GetInheritanceHierarchy(this ITypeSymbol self)
	{
		var hierarchy = ImmutableArray.CreateBuilder<ITypeSymbol>();

		var targetClassSymbol = self;

		while (targetClassSymbol is not null)
		{
			hierarchy.Insert(0, targetClassSymbol);
			targetClassSymbol = targetClassSymbol.BaseType;
		}

		return hierarchy.ToImmutable();
	}
}