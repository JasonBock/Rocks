using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	public interface IA
	{
		int this[int a] { get; }
	}

	internal static class ITypeSymbolExtensions
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
				var seenEvents = new HashSet<IEventSymbol>(EventSymbolEqualityComparer.Default);

				foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
					.Where(_ => !_.IsStatic && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					if (seenEvents.Add(selfEvent))
					{
						events.Add(new(selfEvent, MustBeImplemented.Yes, RequiresOverride.No));
					}
				}
			}
			else
			{
				foreach (var selfEvent in self.GetMembers().OfType<IEventSymbol>()
					.Where(_ => !_.IsStatic && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					events.Add(new(selfEvent, selfEvent.IsAbstract ? MustBeImplemented.Yes : MustBeImplemented.No, RequiresOverride.Yes));
				}
			}

			return events.ToImmutable();
		}

		internal static ImmutableArray<MethodMockableResult> GetMockableMethods(
			this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol,
			ref uint memberIdentifier)
		{
			var methods = ImmutableArray.CreateBuilder<MethodMockableResult>();

			if (self.TypeKind == TypeKind.Interface)
			{
				foreach (var selfMethod in self.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					methods.Add(new(selfMethod, self, RequiresOverride.No, memberIdentifier));
					memberIdentifier++;
				}

				var baseInterfaceMethods = new List<IMethodSymbol>();

				foreach (var selfBaseInterface in self.AllInterfaces)
				{
					foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
						.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
					{
						if (!baseInterfaceMethods.Any(_ => _.Match(selfBaseMethod) == MethodMatch.Exact))
						{
							baseInterfaceMethods.Add(selfBaseMethod);
						}
					}
				}

				foreach (var baseInterfaceMethod in baseInterfaceMethods)
				{
					methods.Add(new(baseInterfaceMethod, self, RequiresOverride.No, memberIdentifier));
					memberIdentifier++;
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
							methods.Add(new(hierarchyMethod, self, RequiresOverride.Yes, memberIdentifier));
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
					properties.Add(new(selfProperty, self, accessors, memberIdentifier));

					memberIdentifier++;

					if (accessors == PropertyAccessor.GetAndSet)
					{
						memberIdentifier++;
					}
				}

				var baseInterfaceProperties = new List<IPropertySymbol>();

				foreach (var selfBaseInterface in self.AllInterfaces)
				{
					foreach (var selfBaseProperty in selfBaseInterface.GetMembers().OfType<IPropertySymbol>()
						.Where(_ => _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
					{
						if (!baseInterfaceProperties.Any(_ => _.Name == selfBaseProperty.Name))
						{
							baseInterfaceProperties.Add(selfBaseProperty);
						}
					}
				}

				foreach (var baseInterfaceProperty in baseInterfaceProperties)
				{
					var accessors = baseInterfaceProperty.GetAccessors();
					properties.Add(new(baseInterfaceProperty, self, accessors, memberIdentifier));
					memberIdentifier++;

					if (accessors == PropertyAccessor.GetAndSet)
					{
						memberIdentifier++;
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
							properties.Add(new(hierarchyProperty, self, accessors, memberIdentifier));
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