using System.Reflection;

namespace Rocks.Extensions
{
	internal static class CustomAttributeDataExtensions
	{
		internal static bool IsNullableAttribute(this CustomAttributeData @this)
		{
			var attributeType = @this.AttributeType;
			return attributeType.Name == "NullableAttribute" && 
				attributeType.Namespace == "System.Runtime.CompilerServices";
		}
	}
}