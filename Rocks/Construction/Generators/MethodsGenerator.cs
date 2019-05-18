using Rocks.Extensions;
using Rocks.Templates;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.MethodBaseExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction.Generators
{
	internal static class MethodsGenerator
	{
		internal static GenerateResults Generate(Type baseType, SortedSet<string> namespaces,
			NameGenerator generator, MethodInformationBuilder informationBuilder, bool isMake,
			Action<MethodInfo, MethodInformation> handleRefOutMethod, bool hasEvents)
		{
			var requiresObsoleteSuppression = false;
			var generatedMethods = new List<string>();

			foreach (var method in baseType.GetMockableMethods(generator))
			{
				var methodInformation = informationBuilder.Build(method);
				var baseMethod = method.Value;
				var argumentNameList = baseMethod.GetArgumentNameList();
				var outInitializers = !methodInformation.ContainsDelegateConditions ? string.Empty : baseMethod.GetOutInitializers();

				if (baseMethod.IsPublic)
				{
					var visibility = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
						string.Empty : CodeTemplates.Public;

					// Either the base method contains no Span<T> or ReadOnlySpan<T> 
					// and it doesn't have refs /outs, or the user specified a delegate
					// to use to handle that method (remember, types with methods with refs/outs are gen'd
					// each time, and that's the only reason the handlers are passed in).
					if(!methodInformation.IsSpanLike)
					{
						if (isMake || !methodInformation.ContainsDelegateConditions || !string.IsNullOrWhiteSpace(methodInformation.DelegateCast))
						{
							if (!methodInformation.ContainsDelegateConditions && baseMethod.GetParameters().Length > 0)
							{
								generatedMethods.Add(MethodsGenerator.GenerateMethodWithNoRefOutParameters(
									baseMethod, methodInformation.DelegateCast, argumentNameList, outInitializers,
									methodInformation.DescriptionWithOverride, visibility,
									method.RequiresNewImplementation, namespaces, isMake, hasEvents));
							}
							else
							{
								generatedMethods.Add(MethodsGenerator.GenerateMethodWithRefOutOrNoParameters(
									baseMethod, methodInformation.DelegateCast, argumentNameList, outInitializers, methodInformation.DescriptionWithOverride,
									visibility, method.RequiresNewImplementation,
									namespaces, isMake, hasEvents));

								if (methodInformation.ContainsDelegateConditions)
								{
									handleRefOutMethod(baseMethod, methodInformation);
								}
							}
						}
						else
						{
							generatedMethods.Add(MethodTemplates.GetNotImplementedMethod(methodInformation.DescriptionWithOverride));
						}
					}
					else
					{
						generatedMethods.Add(MethodTemplates.GetNotImplementedMethod(methodInformation.DescriptionWithOverride));
					}

					requiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!baseMethod.IsPrivate && baseMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(baseMethod.IsFamily, baseMethod.IsFamilyOrAssembly);

					generatedMethods.Add(baseMethod.ReturnType != typeof(void) ?
						MethodTemplates.GetNonPublicFunctionImplementation(visibility, methodInformation.Description,
							outInitializers, baseMethod.ReturnType,
							method.RequiresNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty,
							baseMethod.ReturnParameter.GetAttributes(true, namespaces)) :
						MethodTemplates.GetNonPublicActionImplementation(visibility, methodInformation.Description,
							outInitializers, method.RequiresNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty));

					requiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return new GenerateResults(string.Join(Environment.NewLine, generatedMethods),
				requiresObsoleteSuppression, false);
		}

		private static string GenerateMethodWithNoRefOutParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList,
			string outInitializers, string methodDescriptionWithOverride,
			string visibility, RequiresIsNewImplementation requiresIsNewImplementation,
			SortedSet<string> namespaces, bool isMake, bool hasEvents)
		{
			var expectationChecks = baseMethod.GetExpectationChecks();
			var expectationExceptionMessage = baseMethod.GetExpectationExceptionMessage();
			var requiresNew = requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty;
			var returnTypeName = $"{baseMethod.ReturnType.GetFullName(baseMethod.ReturnParameter)}";

			if (baseMethod.ReturnType != typeof(void))
			{
				var returnTypeAttributes = baseMethod.ReturnParameter.GetAttributes(true, namespaces);
				return isMake ?
					MethodTemplates.GetFunctionForMake(outInitializers, methodDescriptionWithOverride,
						visibility, requiresNew, returnTypeAttributes, baseMethod.ReturnType) :
					baseMethod.ReturnType.RequiresExplicitCast() ?
						MethodTemplates.GetFunctionWithValueTypeReturnValue(
							baseMethod.MetadataToken, argumentNameList, returnTypeName,
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility, requiresNew, returnTypeAttributes, hasEvents) :
						MethodTemplates.GetFunctionWithReferenceTypeReturnValue(
							baseMethod.MetadataToken, argumentNameList, returnTypeName,
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility, requiresNew, returnTypeAttributes, hasEvents);
			}
			else
			{
				return isMake ?
					MethodTemplates.GetActionMethodForMake(outInitializers, methodDescriptionWithOverride, visibility) :
					MethodTemplates.GetActionMethod(baseMethod.MetadataToken, argumentNameList, expectationChecks,
						delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride, visibility, hasEvents);
			}
		}

		private static string GenerateMethodWithRefOutOrNoParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList,
			string outInitializers, string methodDescriptionWithOverride, string visibility,
			RequiresIsNewImplementation requiresIsNewImplementation,
			SortedSet<string> namespaces, bool isMake, bool hasEvents)
		{
			var requiresNew = requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty;
			var returnTypeName = $"{baseMethod.ReturnType.GetFullName(namespaces, baseMethod.ReturnParameter)}";

			if (baseMethod.ReturnType != typeof(void))
			{
				var returnTypeAttributes = baseMethod.ReturnParameter.GetAttributes(true, namespaces);
				return isMake ?
					MethodTemplates.GetFunctionForMake(outInitializers, methodDescriptionWithOverride,
						visibility, requiresNew, returnTypeAttributes, baseMethod.ReturnType) :
						baseMethod.ReturnType.RequiresExplicitCast() ?
							MethodTemplates.GetFunctionWithValueTypeReturnValueAndNoArguments(
								baseMethod.MetadataToken, argumentNameList, returnTypeName,
								delegateCast, outInitializers, methodDescriptionWithOverride, visibility,
								requiresNew, returnTypeAttributes, hasEvents) :
							MethodTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArguments(
								baseMethod.MetadataToken, argumentNameList, returnTypeName,
								delegateCast, outInitializers, methodDescriptionWithOverride, visibility,
								requiresNew, returnTypeAttributes, hasEvents);
			}
			else
			{
				return isMake ?
					MethodTemplates.GetActionMethodWithNoArgumentsForMake(outInitializers, methodDescriptionWithOverride, visibility) :
					MethodTemplates.GetActionMethodWithNoArguments(baseMethod.MetadataToken, argumentNameList, delegateCast,
						outInitializers, methodDescriptionWithOverride, visibility, hasEvents);
			}
		}
	}
}
