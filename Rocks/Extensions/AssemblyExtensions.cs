using Rocks.Construction;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rocks.Extensions
{
	internal static class AssemblyExtensions
	{
		internal static bool CanBeSeenByMockAssembly(this Assembly @this, bool isMemberPublic, 
			bool isMemberPrivate, bool isMemberFamily, NameGenerator generator)
		{
			return isMemberPublic || isMemberFamily || (!isMemberPrivate && @this.GetCustomAttributes<InternalsVisibleToAttribute>()
				.Where(_ => _.AssemblyName == generator.AssemblyName).Any());
		}
	}
}
