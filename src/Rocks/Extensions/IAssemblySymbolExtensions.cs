using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Rocks.Extensions;

internal static class IAssemblySymbolExtensions
{
	internal static bool ExposesInternalsTo(this IAssemblySymbol self, IAssemblySymbol other)
	{
		var internalsVisibleToAttribute = typeof(InternalsVisibleToAttribute);

		return self.GetAttributes().Any(
			_ => _.AttributeClass is not null && _.AttributeClass.Name == internalsVisibleToAttribute.Name &&
				_.AttributeClass.ContainingNamespace.ToDisplayString() == internalsVisibleToAttribute.Namespace &&
				_.AttributeClass.ContainingAssembly.Name == internalsVisibleToAttribute.Assembly.GetName().Name &&
				(string)_.ConstructorArguments[0].Value! == other.Name);
	}
}