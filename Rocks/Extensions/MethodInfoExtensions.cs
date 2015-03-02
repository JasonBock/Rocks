using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class MethodInfoExtensions
	{
		internal static bool ContainsRefAndOrOutParameters(this MethodInfo @this)
		{
			return (from parameter in @this.GetParameters()
					  where parameter.IsOut || parameter.ParameterType.IsByRef
					  select parameter).Any();
		}

		internal static string GetOutInitializers(this MethodInfo @this)
		{
			return string.Join(Environment.NewLine,
				from parameter in @this.GetParameters()
				where parameter.IsOut
				select $"{parameter.Name} = default({parameter.ParameterType.GetElementType().GetSafeName()});");
      }

		internal static string GetDelegateCast(this MethodInfo @this)
		{
			var parameters = @this.GetParameters();
			var methodKind = @this.ReturnType != typeof(void) ? "Func" : "Action";

         if (parameters.Length == 0)
			{
				return @this.ReturnType != typeof(void) ?
					$"{methodKind}<{@this.ReturnType.GetSafeName()}>" : $"{methodKind}";
         }
			else
			{
				var genericArgumentTypes = string.Join(", ", parameters.Select(_ => _.ParameterType.GetSafeName()));
				return @this.ReturnType != typeof(void) ?
					$"{methodKind}<{genericArgumentTypes}, {@this.ReturnType.GetSafeName()}>" : $"{methodKind}<{genericArgumentTypes}>";
         }
		}

		internal static string GetExpectationChecks(this MethodInfo @this)
		{
			return string.Join(Environment.NewLine,
				@this.GetParameters().Select(_ =>
					string.Format(Constants.CodeTemplates.ExpectationTemplate, _.Name, _.ParameterType.GetSafeName())));
		}

		internal static string GetMethodDescription(this MethodInfo @this, SortedSet<string> namespaces)
		{
			if(@this.IsGenericMethod)
			{
				@this = @this.GetGenericMethodDefinition();
			}

			namespaces.Add(@this.ReturnType.Namespace);

			var isOverride = @this.DeclaringType.IsClass ? "override " : string.Empty;
			var returnType = @this.ReturnType == typeof(void) ?
				"void" :  @this.ReturnType.GetSafeName();

			var methodName = @this.Name;
			var generics = string.Empty;
			var constraints = string.Empty;

			if(@this.IsGenericMethodDefinition)
			{
				var result = @this.GetGenericArguments(namespaces);
				generics = result.Arguments;
				constraints = result.Constraints.Length == 0 ? string.Empty : $" {result.Constraints}";
			}

			var parameters = @this.GetParameters(namespaces);

			return $"{isOverride}{returnType} {methodName}{generics}({parameters}){constraints}";
      }
	}
}
