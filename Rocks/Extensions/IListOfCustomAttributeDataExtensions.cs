using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Rocks.Extensions
{
	internal static class IListOfCustomAttributeDataExtensions
	{
		private const string AttributeName = "Attribute";

		internal static string GetAttributes(this IList<CustomAttributeData> @this, bool isReturn, SortedSet<string> namespaces, ParameterInfo parameter)
		{
			var attributes = new List<string>();

			foreach (var attributeData in @this)
			{
				if(!(parameter != null && (parameter.IsOut && parameter.ParameterType.IsByRef &&
					typeof(OutAttribute).IsAssignableFrom(attributeData.AttributeType)) ||
					(typeof(ParamArrayAttribute).IsAssignableFrom(attributeData.AttributeType)) ||
					(typeof(OptionalAttribute).IsAssignableFrom(attributeData.AttributeType))))
				{
					var name = attributeData.AttributeType.GetSafeName(namespaces);

					if (name.EndsWith(IListOfCustomAttributeDataExtensions.AttributeName))
					{
						name = name.Substring(0, name.LastIndexOf(IListOfCustomAttributeDataExtensions.AttributeName));
					}

					var constructorArguments = string.Join(", ",
						(from argument in attributeData.ConstructorArguments
						 let argumentType = argument.ArgumentType
						 let typeCast = argumentType.IsEnum ? $"({argumentType.GetSafeName(namespaces)})" : string.Empty
						 let argumentValue = typeof(string).IsAssignableFrom(argumentType) ? $"\"{argument.Value}\"" : argument.Value
						 select $"{typeCast}{argumentValue}").ToArray());
					var namedArguments = !typeof(MarshalAsAttribute).IsAssignableFrom(attributeData.AttributeType) ?
						string.Join(", ",
							(from argument in attributeData.NamedArguments
							 let argumentType = argument.TypedValue.ArgumentType
							 let typeCast = argumentType.IsEnum ? $"({argumentType.GetSafeName(namespaces)})" : string.Empty
							 let argumentValue = typeof(string).IsAssignableFrom(argumentType) ? $"\"{argument.TypedValue.Value}\"" : argument.TypedValue.Value
							 select $"{argument.MemberName} = {typeCast}{argumentValue}").ToArray()) : string.Empty;
					var arguments = !string.IsNullOrWhiteSpace(constructorArguments) && !string.IsNullOrWhiteSpace(namedArguments) ?
						$"({constructorArguments}, {namedArguments})" :
						!string.IsNullOrWhiteSpace(constructorArguments) ? $"({constructorArguments})" :
						!string.IsNullOrWhiteSpace(constructorArguments) ? $"({namedArguments})" : string.Empty;
					attributes.Add($"{name}{arguments}");
				}
			}

			var returnTarget = isReturn ? "return: " : string.Empty;
			return attributes.Count == 0 ? string.Empty : $"[{returnTarget}{string.Join(", ", attributes)}]";
		}
	}
}
