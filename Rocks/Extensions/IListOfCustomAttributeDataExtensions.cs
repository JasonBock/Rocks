using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Rocks.Extensions
{
	internal static class IListOfCustomAttributeDataExtensions
	{
		private const string AttributeName = "Attribute";

		internal static string GetAttributes(this IList<CustomAttributeData> @this, bool isReturn, SortedSet<string> namespaces, ParameterInfo? parameter)
		{
			var attributes = new List<string>();

			foreach (var attributeData in @this)
			{
				if(!attributeData.IsNullableAttribute() && !(parameter != null && 
					parameter.IsOut && parameter.ParameterType.IsByRef && typeof(OutAttribute).IsAssignableFrom(attributeData.AttributeType) ||
					typeof(ParamArrayAttribute).IsAssignableFrom(attributeData.AttributeType) ||
					typeof(OptionalAttribute).IsAssignableFrom(attributeData.AttributeType)))
				{
					var attributeType = attributeData.AttributeType;
					var name = TypeDissector.Create(attributeType).SafeName;
					namespaces.Add(attributeType.Namespace);

					if (name.EndsWith(IListOfCustomAttributeDataExtensions.AttributeName, StringComparison.Ordinal))
					{
						name = name.Substring(0, name.LastIndexOf(IListOfCustomAttributeDataExtensions.AttributeName, StringComparison.Ordinal));
					}

					var constructorArguments = string.Join(", ",
						(from argument in attributeData.ConstructorArguments
						 let argumentType = argument.ArgumentType
						 let namespaceAdd = namespaces.Add(argumentType.Namespace)
						 let typeCast = argumentType.IsEnum ? $"({TypeDissector.Create(argumentType).SafeName})" : string.Empty
						 let argumentValue = typeof(string).IsAssignableFrom(argumentType) ? $"\"{argument.Value}\"" : 
							typeof(bool).IsAssignableFrom(argumentType) ? argument.Value.ToString().ToLower(CultureInfo.CurrentCulture) : argument.Value
						 select $"{typeCast}{argumentValue}").ToArray());
					var namedArguments = !typeof(MarshalAsAttribute).IsAssignableFrom(attributeData.AttributeType) ?
						string.Join(", ",
							(from argument in attributeData.NamedArguments
							 let argumentType = argument.TypedValue.ArgumentType
							 let namespaceAdd = namespaces.Add(argumentType.Namespace)
							 let typeCast = argumentType.IsEnum ? $"({TypeDissector.Create(argumentType).SafeName})" : string.Empty
							 let argumentValue = typeof(string).IsAssignableFrom(argumentType) ? $"\"{argument.TypedValue.Value}\"" :
								typeof(bool).IsAssignableFrom(argumentType) ? argument.TypedValue.Value.ToString().ToLower(CultureInfo.CurrentCulture) : argument.TypedValue.Value
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