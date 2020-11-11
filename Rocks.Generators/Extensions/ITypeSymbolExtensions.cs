using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
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
			this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, Compilation compilation,
			ref uint memberIdentifier)
		{
			var methods = ImmutableArray.CreateBuilder<MethodMockableResult>();

			if (self.TypeKind == TypeKind.Interface)
			{
				// TODO: I wonder if there's a way to get an object symbol without passing in the compilation.
				var objectMethods = compilation.GetTypeByMetadataName(typeof(object).FullName)!
					.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && (_.IsVirtual || _.IsStatic)).ToImmutableArray();

				foreach (var selfMethod in self.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
				{
					methods.Add(new(selfMethod, self,
						objectMethods.Any(_ => _.Match(selfMethod) == MethodMatch.Exact) ?
							RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
						RequiresOverride.No, memberIdentifier));
					memberIdentifier++;
				}

				var baseInterfaceMethodGroups = new List<List<IMethodSymbol>>();

				foreach (var selfBaseInterface in self.AllInterfaces)
				{
					foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
						.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)))
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

				foreach (var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
				{
					if (baseInterfaceMethodGroup.Count == 1)
					{
						methods.Add(new(baseInterfaceMethodGroup[0], self,
							methods.Any(_ => _.Value.Match(baseInterfaceMethodGroup[0]) == MethodMatch.Exact) ||
								objectMethods.Any(_ => _.Match(baseInterfaceMethodGroup[0]) == MethodMatch.Exact) ?
								RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
							RequiresOverride.No, memberIdentifier));
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
							_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)).Cast<IMethodSymbol>())
					{
						if (hierarchyMethod.IsAbstract || (hierarchyMethod.IsVirtual && !hierarchyMethod.IsSealed))
						{
							methods.Add(new(hierarchyMethod, self,
								RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, memberIdentifier));
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
#pragma warning disable CA1801 // Review unused parameters
			this ITypeSymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol, ref uint memberIdentifier)
#pragma warning restore CA1801 // Review unused parameters
		{
			var events = ImmutableArray.CreateBuilder<PropertyMockableResult>();

			return events.ToImmutable();
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