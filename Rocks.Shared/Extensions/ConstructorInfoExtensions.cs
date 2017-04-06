using Rocks.Construction;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class ConstructorInfoExtensions
	{
		internal static bool CanBeSeenByMockAssembly(this ConstructorInfo @this, NameGenerator generator)
		{
			foreach (var parameter in @this.GetParameters())
			{
				if (!parameter.ParameterType.CanBeSeenByMockAssembly(generator))
				{
					return false;
				}
			}

			return true;
		}

		internal static bool IsUnsafeToMock(this ConstructorInfo @this)
		{
			return (@this.IsPublic || @this.IsAssembly || @this.IsFamily) &&
				@this.GetParameters().Where(param => param.ParameterType.IsPointer).Any();
		}
	}
}
