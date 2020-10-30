using System;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class TypeExtensions
	{
		internal static string? GetMemberDescription(this Type @this, int identifier) =>
			(from method in @this.GetMethods()
			 let memberIdentifier = method.GetCustomAttributes<MemberIdentifierAttribute>().SingleOrDefault()
			 where memberIdentifier is not null 
			 where memberIdentifier.Value == identifier
			 select memberIdentifier.Description).FirstOrDefault();
	}
}