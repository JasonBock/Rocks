using Microsoft.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rocks.Extensions
{
	internal static class IAssemblySymbolExtensions
	{
		internal static bool ExposesInternalsTo(this IAssemblySymbol self, IAssemblySymbol other)
		{
			var internalsVisibleToAttribute = typeof(InternalsVisibleToAttribute);
			var internalsVisibleTo = self.GetAttributes().SingleOrDefault(
				_ => _.AttributeClass is not null && _.AttributeClass.Name == internalsVisibleToAttribute.Name &&
					_.AttributeClass.ContainingNamespace.ToDisplayString() == internalsVisibleToAttribute.Namespace &&
					_.AttributeClass.ContainingAssembly.Name == internalsVisibleToAttribute.Assembly.GetName().Name);

			if(internalsVisibleTo is not null)
			{
				if((string)internalsVisibleTo.ConstructorArguments[0].Value! == other.Name)
				{
					return true;
				}
			}

			return false;
		}
	}
}