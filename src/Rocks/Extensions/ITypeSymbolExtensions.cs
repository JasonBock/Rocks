using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class ITypeSymbolExtensions
{
	internal static bool IsOpenGeneric(this ITypeSymbol self)
	{
		if (self is INamedTypeSymbol namedType)
		{
			for (var i = 0; i < namedType.TypeParameters.Length; i++)
			{
				if (namedType.TypeArguments[i].Name == namedType.TypeParameters[i].Name)
				{
					return true;
				}
			}
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

	// Gets the name of the type, useful for getting the name of a type
	// that will be used in a parameter name.
	// For example if the type is "FileTransform", which is a nested type,
	// this will return "FileSystemEnumerable<object>.FindTransform"
	internal static string GetReferenceableName(this ITypeSymbol self)
	{
		if (self.Kind == SymbolKind.PointerType || self.Kind == SymbolKind.FunctionPointerType)
		{
			return self.ToDisplayString();
		}
		else if(self.Kind == SymbolKind.TypeParameter)
		{
			return $"{self.Name}{(self.NullableAnnotation == NullableAnnotation.Annotated ? "?" : string.Empty)}";
		}
		else
		{
			var names = new List<string>();

			var currentSelf = self;

			while(currentSelf is not null)
			{
				names.Insert(0, currentSelf.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
				currentSelf = currentSelf.ContainingType;
			}

			return string.Join(".", names);
		}
	}

	// TODO: This method really needs to change.
	// It's doing WAY too much in too many different contexts.
	// I need to split this out and have methods that are well-focus and defined.
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
				return self.ToDisplayString().Replace(".", "_").Replace("*", "Pointer");
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

	internal static ImmutableArray<IMethodSymbol> GetMockableConstructors(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol) =>
			self.TypeKind == TypeKind.Class ?
				self.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Constructor &&
						_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)).ToImmutableArray() :
				Array.Empty<IMethodSymbol>().ToImmutableArray();

	internal static ImmutableArray<EventMockableResult> GetMockableEvents(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol)
	{
		var events = ImmutableArray.CreateBuilder<EventMockableResult>();

		if (self.TypeKind == TypeKind.Interface)
		{
			foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
				.Where(_ => !_.IsStatic && _.CanBeReferencedByName && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No));
			}

			var baseInterfaceEventsGroups = new List<List<IEventSymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseEvent in selfBaseInterface.GetMembers().OfType<IEventSymbol>()
					.Where(_ => _.CanBeReferencedByName && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
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
					(_.IsAbstract || _.IsVirtual) && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes));
			}
		}

		return events.ToImmutable();
	}

	internal static ImmutableArray<MethodMockableResult> GetMockableMethods(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, Compilation compilation, ref uint memberIdentifier)
	{
		var methods = ImmutableArray.CreateBuilder<MethodMockableResult>();

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

			foreach (var selfMethod in self.GetMembers().OfType<IMethodSymbol>()
				.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName &&
					_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				var selfMethodRequiresExplicit = objectMethods.Any(
					_ => _.Match(selfMethod) switch
					{
						MethodMatch.DifferByReturnTypeOnly or MethodMatch.Exact => true,
						_ => false
					}) ? RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No;
				methods.Add(new(selfMethod, self, selfMethodRequiresExplicit, RequiresOverride.No, memberIdentifier));

				if (selfMethod.IsVirtual)
				{
					shims.Add(self);
				}

				memberIdentifier++;
			}

			var baseInterfaceMethodGroups = new List<List<IMethodSymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName &&
						_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
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

			if(baseInterfaceMethodGroups.Count > 0)
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
				foreach (var hierarchyMethod in hierarchyType.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName &&
						!_.IsStatic && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					if ((hierarchyMethod.IsAbstract || hierarchyMethod.IsOverride || hierarchyMethod.IsVirtual) &&
						(!self.IsRecord || hierarchyMethod.Name != nameof(object.Equals)))
					{
						var methodToRemove = methods.SingleOrDefault(_ => _.Value.Match(hierarchyMethod) == MethodMatch.Exact &&
							!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));

						if (methodToRemove is not null)
						{
							methods.Remove(methodToRemove);
						}

						if (!hierarchyMethod.IsSealed)
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
			}
		}

		return methods.ToImmutable();
	}

	internal static ImmutableArray<PropertyMockableResult> GetMockableProperties(
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

		if (self.TypeKind == TypeKind.Interface)
		{
			foreach (var selfProperty in self.GetMembers().OfType<IPropertySymbol>()
				.Where(_ => (_.IsIndexer || _.CanBeReferencedByName) && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				var accessors = selfProperty.GetAccessors();
				properties.Add(new(selfProperty, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, accessors, memberIdentifier));

				if (selfProperty.IsVirtual)
				{
					shims.Add(self);
				}

				memberIdentifier++;

				if (accessors == PropertyAccessor.GetAndSet || accessors == PropertyAccessor.GetAndInit)
				{
					memberIdentifier++;
				}
			}

			var baseInterfacePropertyGroups = new List<List<IPropertySymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseProperty in selfBaseInterface.GetMembers().OfType<IPropertySymbol>()
					.Where(_ => (_.IsIndexer || _.CanBeReferencedByName) && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					if (!properties.Any(_ => _.Value.Name == selfBaseProperty.Name))
					{
						var foundMatch = false;

						foreach (var baseInterfacePropertyGroup in baseInterfacePropertyGroups)
						{
							if (baseInterfacePropertyGroup.Any(_ => _.Name == selfBaseProperty.Name))
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

			foreach (var baseInterfacePropertyGroup in baseInterfacePropertyGroups)
			{
				if (baseInterfacePropertyGroup.Count == 1)
				{
					var accessors = baseInterfacePropertyGroup[0].GetAccessors();
					properties.Add(new(baseInterfacePropertyGroup[0], self,
						RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, accessors, memberIdentifier));

					if (baseInterfacePropertyGroup[0].IsVirtual)
					{
						shims.Add(baseInterfacePropertyGroup[0].ContainingType);
					}

					memberIdentifier++;

					if (accessors == PropertyAccessor.GetAndSet || accessors == PropertyAccessor.GetAndInit)
					{
						memberIdentifier++;
					}
				}
				else
				{
					foreach (var baseInterfaceProperty in baseInterfacePropertyGroup)
					{
						var accessors = baseInterfaceProperty.GetAccessors();
						properties.Add(new(baseInterfaceProperty, self,
							RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, accessors, memberIdentifier));

						if (baseInterfaceProperty.IsVirtual)
						{
							shims.Add(baseInterfaceProperty.ContainingType);
						}

						memberIdentifier++;

						if (accessors == PropertyAccessor.GetAndSet || accessors == PropertyAccessor.GetAndInit)
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
					.Where(_ => !_.IsStatic && (_.IsIndexer || _.CanBeReferencedByName) &&
						_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
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
							var accessors = hierarchyProperty.GetAccessors();
							properties.Add(new(hierarchyProperty, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, accessors, memberIdentifier));

							if (hierarchyProperty.ContainingType.TypeKind == TypeKind.Interface && hierarchyProperty.IsVirtual)
							{
								shims.Add(hierarchyProperty.ContainingType);
							}

							memberIdentifier++;

							if (accessors == PropertyAccessor.GetAndSet || accessors == PropertyAccessor.GetAndInit)
							{
								memberIdentifier++;
							}
						}
					}
				}
			}
		}


		return properties.ToImmutable();
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