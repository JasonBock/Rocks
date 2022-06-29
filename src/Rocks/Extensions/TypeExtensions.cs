using System.Reflection;

namespace Rocks.Extensions;

internal static class TypeExtensions
{
	internal static string? GetMemberDescription(this Type self, int identifier) =>
		(from member in self.GetMembers()
		 from memberIdentifier in member.GetCustomAttributes<MemberIdentifierAttribute>()
		 where memberIdentifier is not null
		 where memberIdentifier.Value == identifier
		 select memberIdentifier.Description).FirstOrDefault();
}