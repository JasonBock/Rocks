using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class ITypeSymbolExtensions
	{
		internal static ImmutableArray<MethodMockableResult> GetMockableMethods(this ITypeSymbol self, Compilation compilation)
		{
			var methods = ImmutableHashSet.CreateBuilder<(IMethodSymbol, RequiresExplicitInterfaceImplementation, RequiresOverride)>();
			var objectSymbol = compilation.GetTypeByMetadataName(typeof(object).FullName)!;
			var objectMethods = objectSymbol.GetMembers()
				.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.Ordinary &&
					(methodSymbol.IsExtern || methodSymbol.IsVirtual)).Cast<IMethodSymbol>().ToImmutableArray();

			if (self.TypeKind == TypeKind.Interface)
			{
				foreach (var selfMethod in self.GetMembers()
					.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.Ordinary && 
					methodSymbol.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
				{
					methods.Add((selfMethod, RequiresExplicitInterfaceImplementation.No,
						objectMethods.Any(_ => _.Match(selfMethod) == MethodMatch.Exact) ? RequiresOverride.Yes : RequiresOverride.No));
				}

				foreach (var selfBaseInterface in self.AllInterfaces)
				{
					foreach (var selfBaseMethod in selfBaseInterface.GetMembers()
						.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
					{
						methods.Add((selfBaseMethod, methods.Any(_ => _.Item1.Match(selfBaseMethod) == MethodMatch.Exact) ?
							RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
							RequiresOverride.No));
					}
				}
			}
			else
			{
				foreach (var selfMethod in self.GetMembers()
					.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.Ordinary && 
						methodSymbol.DeclaredAccessibility == Accessibility.Public &&
						(methodSymbol.IsAbstract || (methodSymbol.IsVirtual && !methodSymbol.IsSealed)) &&
						methodSymbol.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
				{
				}
			}

			// TODO: Need to finish this out.
			return methods.Select(_ => new MethodMockableResult(_.Item1, _.Item2, RequiresIsNewImplementation.No, _.Item3))
				.ToImmutableArray();
		}

		// TODO: Finish correctly
		private static bool CanBeSeenByMockAssembly(this IMethodSymbol self) => true;
	}
}