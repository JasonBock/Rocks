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

		if (options == TypeNameOption.IncludeGenerics)
		{
			return self.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
		}
		else if (options == TypeNameOption.Flatten)
		{
			if (self.Kind == SymbolKind.PointerType)
			{
				return self.ToDisplayString().Replace("*", "Pointer");
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
						ISymbolExtensions.CanBeSeenByContainingAssembly(_, containingAssemblyOfInvocationSymbol)).ToImmutableArray() :
				Array.Empty<IMethodSymbol>().ToImmutableArray();

	internal static ImmutableArray<EventMockableResult> GetMockableEvents(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol)
	{
		var events = ImmutableArray.CreateBuilder<EventMockableResult>();

		if (self.TypeKind == TypeKind.Interface)
		{
			foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
				.Where(_ => !_.IsStatic && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No));
			}

			var baseInterfaceEventsGroups = new List<List<IEventSymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseEvent in selfBaseInterface.GetMembers().OfType<IEventSymbol>()
					.Where(_ => _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
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
				.Where(_ => !_.IsStatic && (_.IsAbstract || _.IsVirtual) && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes));
			}
		}

		return events.ToImmutable();
	}

	internal static ImmutableArray<MethodMockableResult> GetMockableMethods(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, ref uint memberIdentifier)
	{
		var methods = ImmutableArray.CreateBuilder<MethodMockableResult>();

		if (self.TypeKind == TypeKind.Interface)
		{
			foreach (var selfMethod in self.GetMembers().OfType<IMethodSymbol>()
				.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				methods.Add(new(selfMethod, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, memberIdentifier));
				memberIdentifier++;
			}

			var baseInterfaceMethodGroups = new List<List<IMethodSymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
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

			foreach (var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
			{
				if (baseInterfaceMethodGroup.Count == 1)
				{
					methods.Add(new(baseInterfaceMethodGroup[0], self,
						RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, memberIdentifier));
					memberIdentifier++;
				}
				else
				{
					foreach (var baseInterfaceMethod in baseInterfaceMethodGroup)
					{
						methods.Add(new(baseInterfaceMethod, self,
							RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, memberIdentifier));
						memberIdentifier++;
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
					.Where(_ => _.MethodKind == MethodKind.Ordinary &&
						_.DeclaredAccessibility == Accessibility.Public && !_.IsStatic &&
						_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					if (hierarchyMethod.IsAbstract || hierarchyMethod.IsOverride || hierarchyMethod.IsVirtual)
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
							memberIdentifier++;
						}
					}
				}
			}
		}

		return methods.ToImmutable();
	}

	internal static ImmutableArray<PropertyMockableResult> GetMockableProperties(
		this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, ref uint memberIdentifier)
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
				.Where(_ => _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
			{
				var accessors = selfProperty.GetAccessors();
				properties.Add(new(selfProperty, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, accessors, memberIdentifier));

				memberIdentifier++;

				if (accessors == PropertyAccessor.GetAndSet)
				{
					memberIdentifier++;
				}
			}

			var baseInterfacePropertyGroups = new List<List<IPropertySymbol>>();

			foreach (var selfBaseInterface in self.AllInterfaces)
			{
				foreach (var selfBaseProperty in selfBaseInterface.GetMembers().OfType<IPropertySymbol>()
					.Where(_ => _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
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
					memberIdentifier++;

					if (accessors == PropertyAccessor.GetAndSet)
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
						memberIdentifier++;

						if (accessors == PropertyAccessor.GetAndSet)
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
					.Where(_ => _.DeclaredAccessibility == Accessibility.Public && !_.IsStatic &&
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
							memberIdentifier++;

							if (accessors == PropertyAccessor.GetAndSet)
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