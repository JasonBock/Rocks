using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class MethodBaseExtensions
	{
		internal static bool IsExtern(this MethodBase @this) =>
			(@this.MethodImplementationFlags & MethodImplAttributes.InternalCall) != 0;

		internal static GenericArgumentsResult GetGenericArguments(this MethodBase @this, SortedSet<string> namespaces)
		{
			var arguments = string.Empty;
			var constraints = string.Empty;

			if (@this.IsGenericMethodDefinition)
			{
				var genericArguments = new List<string>();
				var genericConstraints = new List<string>();

				foreach (var argument in @this.GetGenericArguments())
				{
#if !NETCOREAPP1_1
					var attributeData = argument.GetCustomAttributesData();
#else
					var attributeData = argument.GetTypeInfo().CustomAttributes.ToList();
#endif
					genericArguments.Add($"{attributeData.GetAttributes(false, namespaces, null)}{new TypeDissector(argument).SafeName}");
					var constraint = argument.GetConstraints(namespaces);

					if (!string.IsNullOrWhiteSpace(constraint))
					{
						genericConstraints.Add(constraint);
					}
				}

				arguments = $"<{string.Join(", ", genericArguments)}>";
				// TODO: This should not add a space in front. The Maker class
				// should adjust the constraints to have a space in front.
				constraints = genericConstraints.Count == 0 ?
					string.Empty : $"{string.Join(" ", genericConstraints)}";
			}

			return new GenericArgumentsResult(arguments, constraints);
		}

		internal static string GetArgumentNameList(this MethodBase @this) =>
			string.Join(", ",
				(from parameter in @this.GetParameters()
				 let modifier = parameter.GetModifier(true)
				 select $"{modifier}{parameter.Name}"));

		internal static string GetExpectationExceptionMessage(this MethodBase @this)
		{
			var hasPointerTypes = @this.GetParameters()
				.Where(_ => new TypeDissector(_.ParameterType).IsPointer).Any();
			var argumentlist = hasPointerTypes ? @this.GetParameters(new SortedSet<string>()) : @this.GetLiteralArgumentNameList();
			return $"{@this.Name}{@this.GetGenericArguments(new SortedSet<string>()).Arguments}({argumentlist})";
		}

		internal static string GetLiteralArgumentNameList(this MethodBase @this) =>
			string.Join(", ",
				(from parameter in @this.GetParameters()
				 select $"{{{parameter.Name}}}"));

		internal static string GetParameters(this MethodBase @this, SortedSet<string> namespaces) =>
			string.Join(", ",
				from parameter in @this.GetParameters()
				let attributes = parameter.GetAttributes(namespaces)
				let type = parameter.ParameterType
				let optionalValue = parameter.IsOptional && parameter.HasDefaultValue ? 
					(parameter.RawDefaultValue == null ? " = null" : 
						(typeof(string).IsAssignableFrom(type) ? $" = \"{parameter.RawDefaultValue}\"" : 
						(typeof(bool).IsAssignableFrom(type) ? $" = {((bool)parameter.RawDefaultValue).GetValue()}" : $" = {parameter.RawDefaultValue}"))) : string.Empty
				let _ = type.AddNamespaces(namespaces)
				let modifier = parameter.GetModifier()
				select $"{attributes}{modifier}{type.GetFullName(namespaces)} {parameter.Name}{optionalValue}");
	}
}
