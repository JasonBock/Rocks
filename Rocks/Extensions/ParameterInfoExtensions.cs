using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class ParameterInfoExtensions
	{
		internal static string GetAttributes(this ParameterInfo @this) =>
			@this.GetAttributes(false, new SortedSet<string>());

		internal static string GetAttributes(this ParameterInfo @this, bool isReturn) =>
			@this.GetAttributes(isReturn, new SortedSet<string>());

		internal static string GetAttributes(this ParameterInfo @this, SortedSet<string> namespaces) =>
			@this.GetAttributes(false, namespaces);

		internal static string GetAttributes(this ParameterInfo @this, bool isReturn, SortedSet<string> namespaces) =>
			@this.GetCustomAttributesData().GetAttributes(isReturn, namespaces, @this);

		internal static string GetModifier(this ParameterInfo @this) =>
			@this.GetModifier(false);

		internal static string GetModifier(this ParameterInfo @this, bool ignoreParams) =>
			(@this.IsOut && !@this.IsIn && @this.ParameterType.IsByRef) ? "out " :
				@this.ParameterType.IsByRef ? "ref " :
				ignoreParams ? string.Empty :
				@this.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0 ? "params " : string.Empty;

		internal static bool IsNullableReference(this ParameterInfo @this)
		{
			var type = @this.ParameterType;

			if (!type.IsValueType)
			{
				foreach (var attribute in @this.GetCustomAttributesData())
				{
					if (attribute.IsNullableAttribute())
					{
						if (attribute.ConstructorArguments.Count > 0)
						{
							var nullableCtor = attribute.ConstructorArguments[0];

							// https://codeblog.jonskeet.uk/2019/02/10/nullableattribute-and-c-8/
							if ((byte)nullableCtor.Value == 2)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}
   }
}