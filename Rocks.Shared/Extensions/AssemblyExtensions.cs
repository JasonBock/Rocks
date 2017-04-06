using Rocks.Construction;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rocks.Extensions
{
	internal static class AssemblyExtensions
	{
		internal static bool CanBeSeenByMockAssembly(this Assembly @this, bool isMemberPublic, 
			bool isMemberPrivate, bool isMemberFamily, bool isMemberFamilyOrAssembly, NameGenerator generator)
		{
			return isMemberPublic || isMemberFamily || isMemberFamilyOrAssembly ||
				(!isMemberPrivate && @this.GetCustomAttributes<InternalsVisibleToAttribute>()
					.Where(_ => _.AssemblyName == generator.AssemblyName).Any());
		}
	}
}
