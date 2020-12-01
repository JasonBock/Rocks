﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static partial class ITypeSymbolExtensions
	{
		private sealed class EventSymbolEqualityComparer
			: IEqualityComparer<IEventSymbol?>
		{
			private static readonly Lazy<EventSymbolEqualityComparer> defaultValue = new(() => new());

			private EventSymbolEqualityComparer()
				: base() { }

			public bool Equals(IEventSymbol? x, IEventSymbol? y) =>
				(x?.Name.Equals(y?.Name) ?? false) && (x?.Type.Equals(y?.Type, SymbolEqualityComparer.Default) ?? false);

			public int GetHashCode(IEventSymbol? obj) => (obj?.Name, obj?.Type).GetHashCode();

			public static EventSymbolEqualityComparer Default { get; } = EventSymbolEqualityComparer.defaultValue.Value;
		}

		internal static string GetName(this ITypeSymbol self, TypeNameOption options = TypeNameOption.IncludeGenerics)
		{
			static string GetNameWithFlattenGenerics(INamedTypeSymbol flattenedName, TypeNameOption flattenedOptions)
			{
				if(flattenedName.TypeArguments.Length == 0)
				{
					return flattenedName.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
				}
				else
				{
					return $"{flattenedName.Name}Of{string.Join("_", flattenedName.TypeArguments.Select(_ => _.GetName(flattenedOptions)))}";
				}
			}

			if(options == TypeNameOption.IncludeGenerics)
			{
				return self.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			}
			else if(options == TypeNameOption.FlattenGenerics && self is INamedTypeSymbol namedSelf)
			{
				return GetNameWithFlattenGenerics(namedSelf, options);
			}
			else
			{
				return self.Name;
			}
		}

		internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this ITypeSymbol self)
		{
			var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

			namespaces.Add(self.ContainingNamespace);

			if(self is INamedTypeSymbol namedSelf)
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

		// NOTE: They're really not "mockable"...
		internal static ImmutableArray<EventMockableResult> GetMockableEvents(
			this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol)
		{
			var events = ImmutableArray.CreateBuilder<EventMockableResult>();

			if (self.TypeKind == TypeKind.Interface)
			{
				foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
					.Where(_ => !_.IsStatic && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, MustBeImplemented.Yes, RequiresOverride.No));
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
								baseInterfaceEventsGroups.Add(new List<IEventSymbol> { selfBaseEvent });
							}
						}
					}
				}

				foreach (var baseInterfaceEventGroup in baseInterfaceEventsGroups)
				{
					if (baseInterfaceEventGroup.Count == 1)
					{
						events.Add(new(baseInterfaceEventGroup[0], 
							RequiresExplicitInterfaceImplementation.No, MustBeImplemented.Yes, RequiresOverride.No));
					}
					else
					{
						foreach (var baseInterfaceEvent in baseInterfaceEventGroup)
						{
							events.Add(new(baseInterfaceEvent,
								RequiresExplicitInterfaceImplementation.Yes, MustBeImplemented.Yes, RequiresOverride.No));
						}
					}
				}
			}
			else
			{
				foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
					.Where(_ => !_.IsStatic && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No,
						selfEvent.IsAbstract ? MustBeImplemented.Yes : MustBeImplemented.No, RequiresOverride.Yes));
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
								baseInterfaceMethodGroups.Add(new List<IMethodSymbol> { selfBaseMethod });
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
						if (hierarchyMethod.IsAbstract || (hierarchyMethod.IsVirtual && !hierarchyMethod.IsSealed))
						{
							methods.Add(new(hierarchyMethod, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, memberIdentifier));
							memberIdentifier++;
						}
						else if (hierarchyMethod.IsOverride || (hierarchyMethod.IsVirtual && hierarchyMethod.IsSealed))
						{
							var methodToRemove = methods.SingleOrDefault(_ => _.Value.Match(hierarchyMethod) == MethodMatch.Exact);

							if (methodToRemove is not null)
							{
								methods.Remove(methodToRemove);
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
								baseInterfacePropertyGroups.Add(new List<IPropertySymbol> { selfBaseProperty });
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
						if (hierarchyProperty.IsAbstract || (hierarchyProperty.IsVirtual && !hierarchyProperty.IsSealed))
						{
							var accessors = hierarchyProperty.GetAccessors();
							properties.Add(new(hierarchyProperty, self, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, accessors, memberIdentifier));
							memberIdentifier++;

							if (accessors == PropertyAccessor.GetAndSet)
							{
								memberIdentifier++;
							}
						}
						else if (hierarchyProperty.IsOverride || (hierarchyProperty.IsVirtual && hierarchyProperty.IsSealed))
						{
							var propertyToRemove = properties.SingleOrDefault(_ => _.Value.Name == hierarchyProperty.Name);

							if (propertyToRemove is not null)
							{
								properties.Remove(propertyToRemove);
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
}