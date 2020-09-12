using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class ITypeSymbolExtensions
	{
		internal static ImmutableArray<IMethodSymbol> GetConstructors(this ITypeSymbol self) => 
			self.TypeKind == TypeKind.Class ?
				self.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Constructor && _.CanBeSeenByMockAssembly()).ToImmutableArray() :
				Array.Empty<IMethodSymbol>().ToImmutableArray();

		internal static ImmutableArray<MethodMockableResult> GetMockableMethods(this ITypeSymbol self, Compilation compilation)
		{
			var methods = ImmutableArray.CreateBuilder<MethodMockableResult>();

			if (self.TypeKind == TypeKind.Interface)
			{
				var objectMethods = compilation.GetTypeByMetadataName(typeof(object).FullName)!
					.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && (_.IsVirtual || _.IsStatic)).ToImmutableArray();

				foreach (var selfMethod in self.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByMockAssembly()))
				{
					methods.Add(new MethodMockableResult(selfMethod,
						objectMethods.Any(_ => _.Match(selfMethod) == MethodMatch.Exact) ? 
							RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
						RequiresOverride.No));
				}

				var baseInterfaceMethodGroups = new List<List<IMethodSymbol>>();

				foreach (var selfBaseInterface in self.AllInterfaces)
				{
					foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
						.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeSeenByMockAssembly()))
					{
						var foundMatch = false;

						foreach(var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
						{
							if(baseInterfaceMethodGroup.Any(_ => _.Match(selfBaseMethod) == MethodMatch.Exact))
							{
								baseInterfaceMethodGroup.Add(selfBaseMethod);
								foundMatch = true;
								break;
							}
						}

						if(!foundMatch)
						{
							baseInterfaceMethodGroups.Add(new List<IMethodSymbol> { selfBaseMethod });
						}
					}
				}

				foreach(var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
				{
					if(baseInterfaceMethodGroup.Count == 1)
					{
						methods.Add(new MethodMockableResult(baseInterfaceMethodGroup[0],
							methods.Any(_ => _.Value.Match(baseInterfaceMethodGroup[0]) == MethodMatch.Exact) ||
								objectMethods.Any(_ => _.Match(baseInterfaceMethodGroup[0]) == MethodMatch.Exact) ?
								RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
							RequiresOverride.No));
					}
					else
					{
						foreach (var baseInterfaceMethod in baseInterfaceMethodGroup)
						{
							methods.Add(new MethodMockableResult(baseInterfaceMethod,
								RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No));
						}
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