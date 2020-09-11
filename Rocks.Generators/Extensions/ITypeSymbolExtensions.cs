using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class ITypeSymbolExtensions
	{
		internal static ImmutableArray<MethodMockableResult> GetMockableMethods(this ITypeSymbol self, Compilation compilation)
		{
			var methods = ImmutableArray.CreateBuilder<MethodMockableResult>();
			var objectSymbol = compilation.GetTypeByMetadataName(typeof(object).FullName)!;
			var objectMethods = objectSymbol.GetMembers().OfType<IMethodSymbol>()
				.Where(_ => _.MethodKind == MethodKind.Ordinary && (_.IsVirtual || _.IsStatic)).ToImmutableArray();

			if (self.TypeKind == TypeKind.Interface)
			{
				foreach (var selfMethod in self.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByMockAssembly()))
				{
					methods.Add(new MethodMockableResult(selfMethod,
						objectMethods.Any(_ => _.Match(selfMethod) == MethodMatch.Exact) ? 
							RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
						RequiresOverride.No));
				}

				foreach (var selfBaseInterface in self.AllInterfaces)
				{
					foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
						.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByMockAssembly()))
					{
						methods.Add(new MethodMockableResult(selfBaseMethod,
							methods.Any(_ => _.Value.Match(selfBaseMethod) == MethodMatch.Exact) ||
								objectMethods.Any(_ => _.Match(selfBaseMethod) == MethodMatch.Exact) ?
								RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
							RequiresOverride.No));
					}
				}
			}
			else
			{
				var hierarchy = self.GetInheritanceHierarchy();

				foreach(var hierarchyType in hierarchy)
				{
					foreach (var hierarchyMethod in hierarchyType.GetMembers().OfType<IMethodSymbol>()
						.Where(_ => _.MethodKind == MethodKind.Ordinary &&
							_.DeclaredAccessibility == Accessibility.Public && !_.IsStatic &&
							_.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
					{
						if (hierarchyMethod.IsAbstract || (hierarchyMethod.IsVirtual && !hierarchyMethod.IsSealed))
						{
							methods.Add(new MethodMockableResult(hierarchyMethod,
								RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes));
						}
						else if(hierarchyMethod.IsOverride || (hierarchyMethod.IsVirtual && hierarchyMethod.IsSealed))
						{
							var methodToRemove = methods.SingleOrDefault(_ => _.Value.Match(hierarchyMethod) == MethodMatch.Exact);

							if(methodToRemove is not null)
							{
								methods.Remove(methodToRemove);
							}
						}
					}
				}
			}

			return methods.ToImmutable();
		}

		// TODO: Finish correctly
		private static bool CanBeSeenByMockAssembly(this IMethodSymbol self) => true;

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