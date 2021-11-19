using System.Reflection;

namespace Rocks.Extensions;

internal static class TypeExtensions
{
	internal static string? GetMemberDescription(this Type self, int identifier) =>
		(from member in self.GetMembers()
		 let memberIdentifier = member.GetCustomAttributes<MemberIdentifierAttribute>().SingleOrDefault()
		 where memberIdentifier is not null
		 where memberIdentifier.Value == identifier
		 select memberIdentifier.Description).FirstOrDefault();
}