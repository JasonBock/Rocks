using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class ITypeSymbolExtensions
	{
		internal static ImmutableArray<MethodMockableResult> GetMockableMethods(this ITypeSymbol self, Compilation compilation)
		{
			var methods = ImmutableHashSet.CreateBuilder<MockableResult<IMethodSymbol>>();
			var objectSymbol = compilation.GetTypeByMetadataName(typeof(object).FullName)!;

			// Making sure object methods like GetHashCode() and ToString() are in the list.
			if(self.TypeKind == TypeKind.Interface)
			{
				foreach(var objectMethod in objectSymbol.GetMembers()
					.Where(_ => _.Kind == SymbolKind.Method && (_.IsExtern || _.IsVirtual)).Cast<IMethodSymbol>())
				{
					methods.Add(new MockableResult<IMethodSymbol>(objectMethod, RequiresExplicitInterfaceImplementation.No));
				}
			}

			// TODO: Special names?
			foreach(var selfMethod in self.GetMembers()
				.Where(_ => _ is IMethodSymbol methodSymbol && methodSymbol.DeclaredAccessibility == Accessibility.Public &&
					(methodSymbol.IsAbstract || (methodSymbol.IsVirtual && !methodSymbol.IsSealed)) && methodSymbol.CanBeSeenByMockAssembly()).Cast<IMethodSymbol>())
			{
				methods.Add(new MockableResult<IMethodSymbol>(selfMethod, RequiresExplicitInterfaceImplementation.No));
			}

			// TODO: Need to finish this out.
			return methods.Select(_ => new MethodMockableResult(_.Value, _.RequiresExplicitInterfaceImplementation, RequiresIsNewImplementation.No))
				.ToImmutableArray();
		}

		// TODO: Finish correctly
		private static bool CanBeSeenByMockAssembly(this IMethodSymbol self) => true;
	}
}