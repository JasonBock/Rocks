using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class ITypeSymbolExtensions
{
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
			self is INamedTypeSymbol namedSelf ?
				namedSelf.TypeArguments.All(_ => _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)) :
				true;

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

	internal static bool IsOpenGeneric(this ITypeSymbol self)
	{
		if (self.TypeKind == TypeKind.TypeParameter)
		{
			return true;
		}
		else if (self is INamedTypeSymbol namedType)
		{
			return namedType.HasOpenGenerics();
		}

		return false;
	}

	internal static bool IsPointer(this ITypeSymbol self) =>
		self.Kind == SymbolKind.PointerType || self.Kind == SymbolKind.FunctionPointerType;

	internal static bool IsEsoteric(this ITypeSymbol self) => self.IsPointer() || self.IsRefLikeType;

	internal static bool ContainsDiagnostics(this ITypeSymbol self)
	{
		foreach (var declaringTypeToMockSyntax in self.DeclaringSyntaxReferences)
		{
			if (declaringTypeToMockSyntax.GetSyntax().GetDiagnostics().Any(_ => _.Severity == DiagnosticSeverity.Error))
			{
				return true;
			}
		}

		return false;
	}

	internal static string GetFullyQualifiedName(this ITypeSymbol self, Compilation compilation)
	{
		const string GlobalPrefix = "global::";

		var symbolFormatter = SymbolDisplayFormat.FullyQualifiedFormat
			.AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);
		var symbolName = self.ToDisplayString(symbolFormatter);

		// If the symbol name has "global::" at the start,
		// then see if the type's assembly has at least one alias.
		// If there is one, then replace "global::" with "{alias}::"

		if (symbolName.StartsWith(GlobalPrefix))
		{
			var aliases = compilation.GetMetadataReference(self.ContainingAssembly)?.Properties.Aliases ?? ImmutableArray<string>.Empty;

			if (aliases.Length > 0)
			{
				symbolName = symbolName.Replace(GlobalPrefix, $"{aliases[0]}::");
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
	// If/when I do "Rocks Engine v3", I'll address this.
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

	// We can't use constructors that are obsolete in error.
	internal static ImmutableArray<IMethodSymbol> GetMockableConstructors(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, INamedTypeSymbol obsoleteAttribute) =>
			self.TypeKind == TypeKind.Class ?
				self.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Constructor &&
						_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol) &&
						!_.GetAttributes().Any(
							a => a.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
								(a.ConstructorArguments.Any(_ => _.Value is bool error && error)))).ToImmutableArray() :
				Array.Empty<IMethodSymbol>().ToImmutableArray();

	internal static MockableEvents GetMockableEvents(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol)
	{
		var events = ImmutableArray.CreateBuilder<EventMockableResult>();
		var inaccessibleAbstractMembers = false;

		if (self.TypeKind == TypeKind.Interface)
		{
			foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
				.Where(_ => !_.IsStatic && _.CanBeReferencedByName))
			{
				if (!selfEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
				{
					inaccessibleAbstractMembers = true;
				}
				else
				{
					events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No));
				}
			}

			var baseInterfaceEventsGroups = new List<List<IEventSymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseEvent in selfBaseInterface.GetMembers().OfType<IEventSymbol>()
					.Where(_ => _.CanBeReferencedByName))
				{
					if (!selfBaseEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
					{
						inaccessibleAbstractMembers = true;
					}
					else
					{
						if (!events.Any(_ => _.Value.Name == selfBaseEvent.Name))
						{
							var foundMatch = false;

							foreach (var baseInterfaceEventGroup in baseInterfaceEventsGroups)
							{
								if (baseInterfaceEventGroup.Any(_ => _.Name == selfBaseEvent.Name))
								{
									baseInterfaceEventGroup.Add(selfBaseEvent);
									foundMatch = true;
									break;
								}
							}

							if (!foundMatch)
							{
								baseInterfaceEventsGroups.Add(new List<IEventSymbol>(1) { selfBaseEvent });
							}
						}
					}
				}
			}

			foreach (var baseInterfaceEventGroup in baseInterfaceEventsGroups)
			{
				if (baseInterfaceEventGroup.Count == 1)
				{
					events.Add(new(baseInterfaceEventGroup[0],
						RequiresExplicitInterfaceImplementation.No, RequiresOverride.No));
				}
				else
				{
					foreach (var baseInterfaceEvent in baseInterfaceEventGroup)
					{
						events.Add(new(baseInterfaceEvent,
							RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No));
					}
				}
			}
		}
		else
		{
			foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
				.Where(_ => !_.IsStatic && _.CanBeReferencedByName &&
					(_.IsAbstract || _.IsVirtual)))
			{
				var canBeSeen = selfEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);

				if (!canBeSeen && selfEvent.IsAbstract)
				{
					inaccessibleAbstractMembers = true;
				}
				else if (canBeSeen)
				{
					events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes));
				}
			}
		}

		return new(events.ToImmutable(), inaccessibleAbstractMembers);
	}

	internal static MockableMethods GetMockableMethods(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, Compilation compilation, ref uint memberIdentifier)
	{
		var methods = ImmutableArray.CreateBuilder<MethodMockableResult>();
		var inaccessibleAbstractMembers = false;
		var hasMatchWithNonVirtual = false;

		if (self.TypeKind == TypeKind.Interface)
		{
			// We need to get the set of methods from object (static, instance, virtual, non-virtual, doesn't matter)
			// because the mock object will derive from object,
			// and interfaces like IEquatable<T> have a method (Equals(T? other))
			// that have the possibility of colliding with methods from interfaces.
			// We don't want to include them, we just need to consider them to see
			// if a method requires explicit implementation
			var objectSymbol = compilation.GetTypeByMetadataName(typeof(object).FullName)!;
			var objectMethods = objectSymbol.GetMembers().OfType<IMethodSymbol>()
				.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName &&
					_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol));

			// We don't want to include non-virtual static methods.
			// Later on in MockModel I make a check for static abstract methods.
			foreach (var selfMethod in self.GetMembers().OfType<IMethodSymbol>()
				.Where(_ => !(_.IsStatic && !_.IsAbstract && !_.IsVirtual) && _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName))
			{
				if (!selfMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol) &&
					selfMethod.IsAbstract)
				{
					inaccessibleAbstractMembers = true;
				}
				else
				{
					// We need to explicitly implement matching methods from the object type,
					// but if they exactly match from methods on the type itself,
					// we don't even add it to the list.
					if (!methods.Any(_ => _.Value.Match(selfMethod) == MethodMatch.Exact))
					{
						var selfMethodRequiresExplicit = objectMethods.Any(
							_ => _.Match(selfMethod) switch
							{
								MethodMatch.DifferByReturnTypeOnly or MethodMatch.Exact => true,
								_ => false
							}) || methods.Any(
							_ => _.Value.Match(selfMethod) switch
							{
								MethodMatch.DifferByReturnTypeOnly => true,
								_ => false
							}) ? RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No;
						methods.Add(new(selfMethod, self, selfMethodRequiresExplicit, RequiresOverride.No, memberIdentifier));

						if (selfMethod.IsVirtual)
						{
							shims.Add(self);
						}

						memberIdentifier++;
					}
				}
			}

			var baseInterfaceMethodGroups = new List<List<IMethodSymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => !(_.IsStatic && !_.IsAbstract && !_.IsVirtual) && _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName))
				{
					if (!selfBaseMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
					{
						inaccessibleAbstractMembers = true;
					}
					else
					{
						if (!methods.Any(_ => _.Value.Match(selfBaseMethod) == MethodMatch.Exact))
						{
							var foundMatch = false;

							foreach (var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
							{
								if (baseInterfaceMethodGroup.Any(_ => _.Match(selfBaseMethod) == MethodMatch.Exact))
								{
									baseInterfaceMethodGroup.Add(selfBaseMethod);
									foundMatch = true;
									break;
								}
							}

							if (!foundMatch)
							{
								baseInterfaceMethodGroups.Add(new List<IMethodSymbol>(1) { selfBaseMethod });
							}
						}
					}
				}
			}

			if (baseInterfaceMethodGroups.Count > 0)
			{
				foreach (var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
				{
					if (baseInterfaceMethodGroup.Count == 1)
					{
						// If this one method differs by return type only
						// from any other method currently captured in the list
						// (think IEnumerable<T> -> IEnumerable and their GetEnumerator() methods),
						// or any of the virtual methods from object,
						// then we must require explicit implementation.
						var requiresExplicitImplementation = methods.Any(
							_ => _.Value.Match(baseInterfaceMethodGroup[0]) == MethodMatch.DifferByReturnTypeOnly) ||
							objectMethods.Any(_ => _.Match(baseInterfaceMethodGroup[0]) switch
								{
									MethodMatch.DifferByReturnTypeOnly or MethodMatch.Exact => true,
									_ => false
								}) ?
							RequiresExplicitInterfaceImplementation.Yes :
							RequiresExplicitInterfaceImplementation.No;

						methods.Add(new(baseInterfaceMethodGroup[0], self,
							requiresExplicitImplementation, RequiresOverride.No, memberIdentifier));

						if (baseInterfaceMethodGroup[0].IsVirtual)
						{
							shims.Add(baseInterfaceMethodGroup[0].ContainingType);
						}

						memberIdentifier++;
					}
					else
					{
						foreach (var baseInterfaceMethod in baseInterfaceMethodGroup)
						{
							methods.Add(new(baseInterfaceMethod, self,
								RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, memberIdentifier));

							if (baseInterfaceMethod.IsVirtual)
							{
								shims.Add(baseInterfaceMethod.ContainingType);
							}

							memberIdentifier++;
						}
					}
				}
			}
		}
		else
		{
			var hierarchy = self.GetInheritanceHierarchy();

			foreach (var hierarchyType in hierarchy)
			{
				var hierarchyMethods = hierarchyType.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName).ToArray();

				if (hierarchyMethods.Length > 0)
				{
					var hierarchyNonMockableMethods = hierarchyType.GetMembers().OfType<IMethodSymbol>()
						.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName &&
							!(_.IsAbstract || _.IsOverride || _.IsVirtual)).ToArray();

					foreach (var hierarchyMethod in hierarchyMethods)
					{
						if (hierarchyMethod.IsStatic && hierarchyMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
						{
							// This is the case where a class does something like this:
							// `protected static new string ToString()`
							// We can see it, and it's hiding a method from a class in the hierarchy,
							// so we have to remove the one that we currently have in the list.
							// If it "shows up" again in a class lower in hierarchy,
							// we'll just add it again.
							var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod) == MethodMatch.None) &&
								!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));

							if (methodToRemove is not null)
							{
								methods.Remove(methodToRemove);
							}
						}
						else if (!hierarchyMethod.IsStatic && (!self.IsRecord || hierarchyMethod.Name != nameof(object.Equals)))
						{
							if (hierarchyMethod.IsAbstract || hierarchyMethod.IsOverride || hierarchyMethod.IsVirtual)
							{
								var canBeSeen = hierarchyMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);

								if (!canBeSeen && hierarchyMethod.IsAbstract)
								{
									inaccessibleAbstractMembers = true;
								}
								else if (canBeSeen)
								{
									var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod) == MethodMatch.None));

									if (methodToRemove is not null)
									{
										methods.Remove(methodToRemove);
									}

									if (hierarchyNonMockableMethods.Any(_ => _.Match(hierarchyMethod) != MethodMatch.None) &&
										!hierarchyMethod.IsStatic && hierarchyMethod.IsAbstract)
									{
										// This is a case where the mockable method matches a non-virtual method
										// on the type and it's abstract. This effectively makes the entire type 
										// not mockable.
										hasMatchWithNonVirtual = true;
									}
									else if ((methodToRemove is null || !methodToRemove.Value.ContainingType.Equals(hierarchyMethod.ContainingType)) &&
										!hierarchyMethod.IsSealed)
									{
										methods.Add(new(hierarchyMethod, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, memberIdentifier));

										if (hierarchyMethod.ContainingType.TypeKind == TypeKind.Interface && hierarchyMethod.IsVirtual)
										{
											shims.Add(hierarchyMethod.ContainingType);
										}

										memberIdentifier++;
									}
								}
							}
							else
							{
								// This is an instance method that could be hiding a virtual method in a base class.
								// If it is, then we need to remove that base method from the list.
								var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod) == MethodMatch.None) &&
									!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));

								if (methodToRemove is not null)
								{
									methods.Remove(methodToRemove);
								}
							}
						}
					}

					// One more sweep. If any of the candidates match a non-mockable method, 
					// we have to remove it.
					methods.RemoveAll(_ => hierarchyNonMockableMethods.Any(
						hierarchyNonMockable => !(_.Value.Match(hierarchyNonMockable) == MethodMatch.None)));
				}
			}
		}

		return new(methods.ToImmutable(), inaccessibleAbstractMembers, hasMatchWithNonVirtual);
	}

	internal static MockableProperties GetMockableProperties(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ref uint memberIdentifier)
	{
		static bool AreParametersEqual(IPropertySymbol property1, IPropertySymbol property2)
		{
			if (property1.Parameters.Length == property2.Parameters.Length)
			{
				for (var i = 0; i < property1.Parameters.Length; i++)
				{
					var property1Parameter = property1.Parameters[i];
					var property2Parameter = property2.Parameters[i];

					if (!property1Parameter.Type.Equals(property2Parameter.Type))
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}

		var properties = ImmutableArray.CreateBuilder<PropertyMockableResult>();
		var inaccessibleAbstractMembers = false;

		if (self.TypeKind == TypeKind.Interface)
		{
			foreach (var selfProperty in self.GetMembers().OfType<IPropertySymbol>()
				.Where(_ => !(_.IsStatic && !_.IsAbstract && !_.IsVirtual) && (_.IsIndexer || _.CanBeReferencedByName)))
			{
				if (!selfProperty.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
				{
					inaccessibleAbstractMembers = true;
				}
				else
				{
					var result = new PropertyMockableResult(selfProperty, self,
						RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, memberIdentifier);
					properties.Add(result);

					if (selfProperty.IsVirtual)
					{
						shims.Add(self);
					}

					memberIdentifier++;

					if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
					{
						memberIdentifier++;
					}
				}
			}

			var baseInterfacePropertyGroups = new List<List<IPropertySymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseProperty in selfBaseInterface.GetMembers().OfType<IPropertySymbol>()
					.Where(_ => !(_.IsStatic && !_.IsAbstract && !_.IsVirtual) && (_.IsIndexer || _.CanBeReferencedByName)))
				{
					if (!selfBaseProperty.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
					{
						inaccessibleAbstractMembers = true;
					}
					else
					{
						if (!properties.Any(_ =>
							(selfBaseProperty.IsIndexer && _.Value.IsIndexer &&
								(selfBaseProperty.GetMethod is not null && _.Value.GetMethod is not null && selfBaseProperty.GetMethod.Match(_.Value.GetMethod) == MethodMatch.Exact) ||
								(selfBaseProperty.SetMethod is not null && _.Value.SetMethod is not null && selfBaseProperty.SetMethod.Match(_.Value.SetMethod) == MethodMatch.Exact)) ||
							(!selfBaseProperty.IsIndexer && !_.Value.IsIndexer &&
								_.Value.Name == selfBaseProperty.Name &&
								SymbolEqualityComparer.Default.Equals(_.Value.Type, selfBaseProperty.Type) &&
								_.Value.GetAccessors() == selfBaseProperty.GetAccessors())))
						{
							var foundMatch = false;

							foreach (var baseInterfacePropertyGroup in baseInterfacePropertyGroups)
							{
								if (baseInterfacePropertyGroup.Any(_ =>
									(selfBaseProperty.IsIndexer && _.IsIndexer &&
										(selfBaseProperty.GetMethod is not null && _.GetMethod is not null && selfBaseProperty.GetMethod.Match(_.GetMethod) == MethodMatch.Exact) ||
										(selfBaseProperty.SetMethod is not null && _.SetMethod is not null && selfBaseProperty.SetMethod.Match(_.SetMethod) == MethodMatch.Exact)) ||
									(!selfBaseProperty.IsIndexer && !_.IsIndexer &&
										_.Name == selfBaseProperty.Name &&
										SymbolEqualityComparer.Default.Equals(_.Type, selfBaseProperty.Type) &&
										_.GetAccessors() == selfBaseProperty.GetAccessors())))
								{
									baseInterfacePropertyGroup.Add(selfBaseProperty);
									foundMatch = true;
									break;
								}
							}

							if (!foundMatch)
							{
								baseInterfacePropertyGroups.Add(new List<IPropertySymbol>(1) { selfBaseProperty });
							}
						}
					}
				}
			}

			foreach (var baseInterfacePropertyGroup in baseInterfacePropertyGroups)
			{
				if (baseInterfacePropertyGroup.Count == 1)
				{
					// If there are any properties that have the same name
					// but different types or different accessors.
					// then we must require explicit implementation.
					var requiresExplicitImplementation = properties.Any(
						_ => baseInterfacePropertyGroup[0].Name == _.Value.Name &&
							(!SymbolEqualityComparer.Default.Equals(baseInterfacePropertyGroup[0].Type, _.Value.Type) ||
							baseInterfacePropertyGroup[0].GetAccessors() != _.Value.GetAccessors())) ?
							RequiresExplicitInterfaceImplementation.Yes :
							RequiresExplicitInterfaceImplementation.No;

					var result = new PropertyMockableResult(baseInterfacePropertyGroup[0], self,
						requiresExplicitImplementation, RequiresOverride.No, memberIdentifier);
					var accessors = baseInterfacePropertyGroup[0].GetAccessors();
					properties.Add(result);

					if (baseInterfacePropertyGroup[0].IsVirtual)
					{
						shims.Add(baseInterfacePropertyGroup[0].ContainingType);
					}

					memberIdentifier++;

					if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
					{
						memberIdentifier++;
					}
				}
				else
				{
					foreach (var baseInterfaceProperty in baseInterfacePropertyGroup)
					{
						var result = new PropertyMockableResult(baseInterfaceProperty, self,
							RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, memberIdentifier);
						var accessors = baseInterfaceProperty.GetAccessors();
						properties.Add(result);

						if (baseInterfaceProperty.IsVirtual)
						{
							shims.Add(baseInterfaceProperty.ContainingType);
						}

						memberIdentifier++;

						if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
						{
							memberIdentifier++;
						}
					}
				}
			}
		}
		else
		{
			var hierarchy = self.GetInheritanceHierarchy();

			foreach (var hierarchyType in hierarchy)
			{
				foreach (var hierarchyProperty in hierarchyType.GetMembers().OfType<IPropertySymbol>()
					.Where(_ => (_.IsIndexer || _.CanBeReferencedByName)))
				{
					if (hierarchyProperty.IsStatic && hierarchyProperty.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
					{
						// This is the case where a class does something like this:
						// `protected static new int Data { get; }`
						// We can see it, and it's hiding a property from a class in the hierarchy,
						// so we have to remove the one that we currently have in the list.
						// If it "shows up" again in a class lower in hierarchy,
						// we'll just add it again.
						var propertyToRemove = properties.SingleOrDefault(_ => _.Value.Name == hierarchyProperty.Name &&
							!_.Value.ContainingType.Equals(hierarchyProperty.ContainingType) &&
							AreParametersEqual(_.Value, hierarchyProperty));

						if (propertyToRemove is not null)
						{
							properties.Remove(propertyToRemove);
						}
					}
					else if (!hierarchyProperty.IsStatic)
					{
						var canBeSeen = hierarchyProperty.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);

						if (!canBeSeen && hierarchyProperty.IsAbstract)
						{
							inaccessibleAbstractMembers = true;
						}
						else if (canBeSeen)
						{
							if (hierarchyProperty.IsAbstract || hierarchyProperty.IsOverride || hierarchyProperty.IsVirtual)
							{
								var propertyToRemove = properties.SingleOrDefault(_ => _.Value.Name == hierarchyProperty.Name &&
									!_.Value.ContainingType.Equals(hierarchyProperty.ContainingType) &&
									AreParametersEqual(_.Value, hierarchyProperty));

								if (propertyToRemove is not null)
								{
									properties.Remove(propertyToRemove);
								}

								if (!hierarchyProperty.IsSealed)
								{
									var result = new PropertyMockableResult(
										hierarchyProperty, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, memberIdentifier);
									properties.Add(result);

									if (hierarchyProperty.ContainingType.TypeKind == TypeKind.Interface && hierarchyProperty.IsVirtual)
									{
										shims.Add(hierarchyProperty.ContainingType);
									}

									memberIdentifier++;

									if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
									{
										memberIdentifier++;
									}
								}
							}
						}
					}
				}
			}
		}

		return new(properties.ToImmutable(), inaccessibleAbstractMembers);
	}

	private static ImmutableArray<ITypeSymbol> GetInheritanceHierarchy(this ITypeSymbol self)
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