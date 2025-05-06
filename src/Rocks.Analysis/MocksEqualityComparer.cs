using Microsoft.CodeAnalysis;

namespace Rocks.Analysis;

internal sealed class MocksEqualityComparer
	: IEqualityComparer<(ITypeSymbol, BuildType)>
{
   public bool Equals((ITypeSymbol, BuildType) x, (ITypeSymbol, BuildType) y) =>
		SymbolEqualityComparer.Default.Equals(x.Item1, y.Item1) &&
		x.Item2 == y.Item2;
 
	public int GetHashCode((ITypeSymbol, BuildType) obj) => 
		obj.GetHashCode();
}
