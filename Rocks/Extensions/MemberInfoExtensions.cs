using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class MemberInfoExtensions
	{
		private const string AttributeName = "Attribute";

		internal static string GetAttributes(this MemberInfo @this)
		{
			return @this.GetAttributes(new SortedSet<string>());
		}

		internal static string GetAttributes(this MemberInfo @this, SortedSet<string> namespaces)
		{
			var attributes = new List<string>();

			foreach(var attributeData in @this.GetCustomAttributesData())
			{
				var name = attributeData.AttributeType.GetSafeName(null, namespaces);

				if(name.EndsWith(MemberInfoExtensions.AttributeName))
				{
					name = name.Substring(0, name.LastIndexOf(MemberInfoExtensions.AttributeName));
				}

				var constructorArguments = string.Join(", ",
					(from argument in attributeData.ConstructorArguments
					 let argumentType = argument.ArgumentType
					 let typeCast = argumentType.IsEnum ? $"({argumentType.GetSafeName(null, namespaces)})" : string.Empty
					 let argumentValue = typeof(string).IsAssignableFrom(argumentType) ? $"\"{argument.Value}\"" : argument.Value
					 select $"{typeCast}{argumentValue}").ToArray());
				var namedArguments = string.Join(", ",
					(from argument in attributeData.NamedArguments
					 let argumentType = argument.TypedValue.ArgumentType
					 let typeCast = argumentType.IsEnum ? $"({argumentType.GetSafeName(null, namespaces)})" : string.Empty
					 let argumentValue = typeof(string).IsAssignableFrom(argumentType) ? $"\"{argument.TypedValue.Value}\"" : argument.TypedValue.Value
					 select $"{argument.MemberName} = {typeCast}{argumentValue}").ToArray());
				var arguments = !string.IsNullOrWhiteSpace(constructorArguments) && !string.IsNullOrWhiteSpace(namedArguments) ?
					$"({constructorArguments}, {namedArguments})" :
					!string.IsNullOrWhiteSpace(constructorArguments) ? $"({constructorArguments})" :
					!string.IsNullOrWhiteSpace(constructorArguments) ? $"({namedArguments})" : string.Empty;
				attributes.Add($"{name}{arguments}");
			}

			return attributes.Count == 0 ? string.Empty : $"[{string.Join(", ", attributes)}]";
      }
	}
}
