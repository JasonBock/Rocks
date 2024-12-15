using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Rocks.Extensions;

internal static class IAssemblySymbolExtensions
{
	internal static bool ExposesInternalsTo(this IAssemblySymbol self, IAssemblySymbol other,
		Compilation compilation)
	{
		var internalsVisibleToAttribute = compilation.GetTypeByMetadataName(typeof(InternalsVisibleToAttribute).FullName);

		return self.GetAttributes().Any(
			_ => _.AttributeClass is not null && 
			 	SymbolEqualityComparer.Default.Equals(_.AttributeClass, internalsVisibleToAttribute) &&
				(string)_.ConstructorArguments[0].Value! == other.Name);
	}
}