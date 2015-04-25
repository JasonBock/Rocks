using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class ConstructorInfoExtensions
	{
		internal static bool IsUnsafeToMock(this ConstructorInfo @this)
		{
			return (@this.IsPublic || @this.IsAssembly || @this.IsFamily) &&
				@this.GetParameters().Where(param => param.ParameterType.IsPointer).Any();
		}
	}
}
