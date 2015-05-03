using Rocks.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class MethodInfoExtensions
	{
		internal static bool CanBeSeenByMockAssembly(this MethodInfo @this, NameGenerator generator)
		{
			if(!@this.IsPublic && (@this.IsPrivate || (!@this.IsFamily && !@this.DeclaringType.CanBeSeenByMockAssembly(generator))))
			{
				return false;
			}

			foreach(var parameter in @this.GetParameters())
			{
				if(!parameter.ParameterType.CanBeSeenByMockAssembly(generator))
				{
					return false;
				}
			}

			if(@this.ReturnType != typeof(void))
			{
				if(!@this.ReturnType.CanBeSeenByMockAssembly(generator))
				{
					return false;
				}
			}

			return true;
		}

		internal static MethodMatch Match(this MethodInfo @this, MethodInfo other)
		{
			if(@this.Name != other.Name)
			{
				return MethodMatch.None;
			}
			else
			{
				var thisParameters = @this.GetParameters().ToList();
				var otherParameters = other.GetParameters().ToList();

				if(thisParameters.Count != otherParameters.Count)
				{
					return MethodMatch.None;
				}

				for (var i = 0; i < thisParameters.Count; i++)
				{
					if ((thisParameters[i].ParameterType != otherParameters[i].ParameterType) ||
						(thisParameters[i].GetModifier() != otherParameters[i].GetModifier()))
               {
						return MethodMatch.None;
					}
				}

				return @this.ReturnType != other.ReturnType ? MethodMatch.DifferByReturnTypeOnly : MethodMatch.Exact;
			}
		}

		internal static bool IsUnsafeToMock(this MethodInfo @this)
		{
			return @this.IsUnsafeToMock(true);
		}

		internal static bool IsUnsafeToMock(this MethodInfo @this, bool checkForSpecialName)
		{
			var specialNameFlag = checkForSpecialName ? @this.IsSpecialName : false;

			return !specialNameFlag && ((@this.IsPublic && @this.IsVirtual && !@this.IsFinal) || 
				((@this.IsAssembly || @this.IsFamily) && @this.IsAbstract)) &&
				(@this.ReturnType.IsPointer || @this.GetParameters().Where(param => param.ParameterType.IsPointer).Any());
		}

		internal static bool ContainsDelegateConditions(this MethodInfo @this)
		{
			return (from parameter in @this.GetParameters()
					  let parameterType = parameter.ParameterType
					  where parameter.IsOut || parameterType.IsByRef || 
						typeof(TypedReference).IsAssignableFrom(parameterType) ||
						typeof(RuntimeArgumentHandle).IsAssignableFrom(parameterType) ||
						typeof(ArgIterator).IsAssignableFrom(parameterType) || 
						new TypeDissector(parameterType).IsPointer
					  select parameter).Any() || new TypeDissector(@this.ReturnType).IsPointer;
		}

		internal static string GetOutInitializers(this MethodInfo @this)
		{
			return string.Join(Environment.NewLine,
				from parameter in @this.GetParameters()
				where parameter.IsOut
				select $"{parameter.Name} = default({parameter.ParameterType.GetFullName()});");
		}

		internal static string GetDelegateCast(this MethodInfo @this)
		{
			var parameters = @this.GetParameters();
			var methodKind = @this.ReturnType != typeof(void) ? "Func" : "Action";

			if (parameters.Length == 0)
			{
				return @this.ReturnType != typeof(void) ?
					$"{methodKind}<{@this.ReturnType.GetSafeName()}{@this.ReturnType.GetGenericArguments(new SortedSet<string>()).Arguments}>" : $"{methodKind}";
			}
			else
			{
				var genericArgumentTypes = string.Join(", ", parameters.Select(_ => $"{_.ParameterType.GetSafeName()}{_.ParameterType.GetGenericArguments(new SortedSet<string>()).Arguments}"));
				return @this.ReturnType != typeof(void) ?
					$"{methodKind}<{genericArgumentTypes}, {@this.ReturnType.GetSafeName()}{@this.ReturnType.GetGenericArguments(new SortedSet<string>()).Arguments}>" : $"{methodKind}<{genericArgumentTypes}>";
			}
		}

		internal static string GetExpectationChecks(this MethodInfo @this)
		{
			return string.Join(" && ",
				@this.GetParameters()
				.Where(_ => !new TypeDissector(_.ParameterType).IsPointer)
				.Select(_ => CodeTemplates.GetExpectationTemplate(_.Name, 
					$"{_.ParameterType.GetSafeName()}{_.ParameterType.GetGenericArguments(new SortedSet<string>()).Arguments}")));
		}

		internal static string GetMethodDescription(this MethodInfo @this)
		{
			return @this.GetMethodDescription(new SortedSet<string>(), false);
		}

		internal static void AddNamespaces(this MethodInfo @this, SortedSet<string> namespaces)
		{
			namespaces.Add(@this.ReturnType.Namespace);
			@this.GetParameters(namespaces);

			if (@this.IsGenericMethodDefinition)
			{
				@this.GetGenericArguments(namespaces);
			}
		}

		internal static string GetMethodDescription(this MethodInfo @this, SortedSet<string> namespaces)
		{
			return @this.GetMethodDescription(namespaces, false, false);
		}

		internal static string GetMethodDescription(this MethodInfo @this, SortedSet<string> namespaces, bool includeOverride)
		{
			return @this.GetMethodDescription(namespaces, includeOverride, false);
		}

		internal static string GetMethodDescription(this MethodInfo @this, SortedSet<string> namespaces, bool includeOverride, bool requiresExplicitInterfaceImplementation)
		{
			if (@this.IsGenericMethod)
			{
				@this = @this.GetGenericMethodDefinition();
			}

			@this.ReturnType.AddNamespaces(namespaces);

			var isOverride = includeOverride ? (@this.DeclaringType.IsClass ? "override " : string.Empty) : string.Empty;
			var returnType = @this.ReturnType == typeof(void) ?
				"void" : $"{@this.ReturnType.GetSafeName()}{@this.ReturnType.GetGenericArguments(namespaces).Arguments}";

			var methodName = @this.Name;
			var generics = string.Empty;
			var constraints = string.Empty;

			if (@this.IsGenericMethodDefinition)
			{
				var result = @this.GetGenericArguments(namespaces);
				generics = result.Arguments;
				constraints = result.Constraints.Length == 0 ? string.Empty : $" {result.Constraints}";
			}

			var parameters = @this.GetParameters(namespaces);
			var explicitInterfaceName = requiresExplicitInterfaceImplementation ?
				$"{@this.DeclaringType.GetSafeName()}{@this.DeclaringType.GetGenericArguments(namespaces).Arguments}." : string.Empty;

			return $"{isOverride}{returnType} {explicitInterfaceName}{methodName}{generics}({parameters}){constraints}";
		}
	}
}
