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
			var objectMethods = objectSymbol.GetMembers()
				.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.Ordinary &&
					(methodSymbol.IsVirtual || methodSymbol.IsStatic)).Cast<IMethodSymbol>().ToImmutableArray();

			if (self.TypeKind == TypeKind.Interface)
			{
				foreach (var selfMethod in self.GetMembers()
					.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.Ordinary && 
					methodSymbol.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
				{
					methods.Add(new MethodMockableResult(selfMethod,
						objectMethods.Any(_ => _.Match(selfMethod) == MethodMatch.Exact) ? 
							RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
						RequiresOverride.No));
				}

				foreach (var selfBaseInterface in self.AllInterfaces)
				{
					foreach (var selfBaseMethod in selfBaseInterface.GetMembers()
						.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.Ordinary && 
						methodSymbol.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
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
				var targetClassSymbol = self;

				while (targetClassSymbol is not null)
				{
					// Just need to find all abstract or non-sealed virtual methods all the way up to object
					foreach (var targetClassSymbolMethod in targetClassSymbol.GetMembers()
						.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.Ordinary &&
							methodSymbol.DeclaredAccessibility == Accessibility.Public &&
							(methodSymbol.IsAbstract || (methodSymbol.IsVirtual && !methodSymbol.IsSealed)) &&
							methodSymbol.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
					{
						methods.Add(new MethodMockableResult(targetClassSymbolMethod,
							RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes));
					}

					targetClassSymbol = targetClassSymbol.BaseType;
				}
			}

			return methods.ToImmutable();
		}

		// TODO: Finish correctly
		private static bool CanBeSeenByMockAssembly(this IMethodSymbol self) => true;
	}
}