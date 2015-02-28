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
				var genericArguments = new List<string>();
				var genericConstraints = new List<string>();

				foreach(var argument in @this.GetGenericArguments())
				{
					genericArguments.Add(argument.Name);
					var constraint = MethodInfoExtensions.GetConstraints(argument, namespaces);

					if(!string.IsNullOrWhiteSpace(constraint))
					{
						genericConstraints.Add(constraint);
					}
				}

				generics = $"<{string.Join(", ", genericArguments)}>";
				constraints = genericConstraints.Count == 0 ? 
					string.Empty : $" {string.Join(" ", genericConstraints)}";
			}

			var parameters = @this.GetParameters(namespaces);

			return $"{isOverride}{returnType} {methodName}{generics}({parameters}){constraints}";
      }

		private static string GetConstraints(Type argument, SortedSet<string> namespaces)
		{
			var constraints = argument.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
			var constraintedTypes = argument.GetGenericParameterConstraints();

			if(constraints == GenericParameterAttributes.None && constraintedTypes.Length == 0)
			{
				return string.Empty;
			}
			else
			{
				var constraintValues = new List<string>();

				if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
				{
					constraintValues.Add("struct");
				}
				else
				{
					foreach (var constraintedType in constraintedTypes.OrderBy(_ => _.IsClass ? 0 : 1))
					{
						constraintValues.Add(constraintedType.GetSafeName());
						namespaces.Add(constraintedType.Namespace);
					}

					if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
					{
						constraintValues.Add("class");
					}
					if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
					{
						constraintValues.Add("new()");
					}
				}

				return $"where {argument.GetSafeName()} : {string.Join(", ", constraintValues)}";
			}
		}
	}
}
