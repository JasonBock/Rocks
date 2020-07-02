using System;
using System.Globalization;
using System.Threading.Tasks;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Templates
{
	internal static class MethodTemplates
	{
		internal static string GetDefaultReturnValue(Type returnType)
		{
			if (typeof(Task).IsAssignableFrom(returnType))
			{
				if (returnType.IsGenericType && typeof(Task<>).IsAssignableFrom(returnType.GetGenericTypeDefinition()))
				{
					var taskReturnType = returnType.GetGenericArguments()[0].GetFullName();
					return $"STT.Task.FromResult<{taskReturnType}>(default!)";
				}
				else
				{
					return "STT.Task.CompletedTask";
				}
			}
			else
			{
				return $"default!";
			}
		}

		internal static string GetAssemblyDelegate(string returnType, string delegateName, string arguments, bool isUnsafe) =>
			$"public {CodeTemplates.GetIsUnsafe(isUnsafe)} delegate {returnType} {delegateName}({arguments});";

		internal static string GetNonPublicActionImplementation(string visibility, string methodName, string outInitializers, string requiresNew) =>
$@"{visibility} {requiresNew} override {methodName}
{{
	{outInitializers}	
}}";

		internal static string GetNonPublicFunctionImplementation(string visibility, string methodName, string outInitializers, Type returnType, string requiresNew, string returnTypeAttributes) =>
$@"{returnTypeAttributes}{visibility} {requiresNew} override {methodName}
{{
	{outInitializers}	
	
	return {MethodTemplates.GetDefaultReturnValue(returnType)};
}}";
		internal static string GetNotImplementedMethod(string methodNameWithOverride) =>
$@"public {methodNameWithOverride} => throw new S.NotImplementedException();";

		internal static string GetActionMethod(int methodHandle, string argumentNames, string expectationTemplateInstances, string delegateCast,
			string outInitializers, string methodWithArgumentValues, string methodNameWithOverride, string visibility, bool hasEvents) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}

	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		var foundMatch = false;
				
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				foundMatch = true;

				if(methodHandler.Method != null)
				{{
#pragma warning disable CS8604
					(({delegateCast})methodHandler.Method)({argumentNames});
#pragma warning restore CS8604
				}}

				{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
				methodHandler.IncrementCallCount();
				break;
			}}
		}}

		if(!foundMatch)
		{{
			throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
		}}
	}}
	else
	{{
		throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
	}}
}}";

		internal static string GetActionMethodForMake(string outInitializers, string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
}}";

		internal static string GetActionMethodWithNoArguments(int methodHandle, string argumentNames, string delegateCast, string outInitializers,
			string methodNameWithOverride, string visibility, bool hasEvents) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}

	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		if(methodHandler.Method != null)
		{{
#pragma warning disable CS8604
			(({delegateCast})methodHandler.Method)({argumentNames});
#pragma warning restore CS8604
		}}

		{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
		methodHandler.IncrementCallCount();
	}}
	else
	{{
		throw new RE.ExpectationException($""No handlers were found for {methodNameWithOverride.Replace("override ", string.Empty, StringComparison.Ordinal)}"");
	}}
}}";

		internal static string GetActionMethodWithNoArgumentsForMake(string outInitializers, string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
}}";

		internal static string GetFunctionWithReferenceTypeReturnValue(int methodHandle, string argumentNames, string returnTypeName, string expectationTemplateInstances,
			string delegateCast, string outInitializers, string methodWithArgumentValues, string methodNameWithOverride, string visibility, string requiresNew, string returnTypeAttributes, bool hasEvents) =>
$@"{returnTypeAttributes}{visibility} {requiresNew} {methodNameWithOverride}
{{
	{outInitializers}

	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
#pragma warning disable CS8604
				var result = methodHandler.Method != null ?
					({returnTypeName})(({delegateCast})methodHandler.Method)({argumentNames}) :
					((R.HandlerInformation<{returnTypeName}>)methodHandler).ReturnValue;
#pragma warning restore CS8604
				{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
				methodHandler.IncrementCallCount();

#pragma warning disable CS8762
				return result;
#pragma warning restore CS8762
			}}
		}}
	}}

	throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
}}";

		internal static string GetFunctionWithReferenceTypeReturnValueAndNoArguments(int methodHandle, string argumentNames, string returnTypeName,
			string delegateCast, string outInitializers, string methodNameWithOverride, string visibility, string requiresNew, string returnTypeAttributes, bool hasEvents) =>
$@"{returnTypeAttributes}{visibility} {requiresNew} {methodNameWithOverride}
{{
	{outInitializers}

	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
#pragma warning disable CS8604
		var result = methodHandler.Method != null ?
			({returnTypeName})(({delegateCast})methodHandler.Method)({argumentNames}) :
			((R.HandlerInformation<{returnTypeName}>)methodHandler).ReturnValue;
#pragma warning restore CS8604
		{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
		methodHandler.IncrementCallCount();

#pragma warning disable CS8762
		return result;
#pragma warning restore CS8762
	}}
	else
	{{
		throw new RE.ExpectationException($""No handlers were found for {methodNameWithOverride.Replace("override ", string.Empty, StringComparison.Ordinal)}"");
	}}
}}";

		internal static string GetFunctionWithValueTypeReturnValue(int methodHandle, string argumentNames, string returnTypeName, string expectationTemplateInstances,
			string delegateCast, string outInitializers, string methodWithArgumentValues, string methodNameWithOverride, string visibility, string requiresNew, string returnTypeAttributes, bool hasEvents) =>
$@"{returnTypeAttributes}{visibility} {requiresNew} {methodNameWithOverride}
{{
	{outInitializers}

	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
#pragma warning disable CS8604
				var result = methodHandler.Method != null ?
					({returnTypeName})(({delegateCast})methodHandler.Method)({argumentNames}) :
					((R.HandlerInformation<{returnTypeName}>)methodHandler).ReturnValue;
#pragma warning restore CS8604
				{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
				methodHandler.IncrementCallCount();

#pragma warning disable CS8762
				return result;
#pragma warning restore CS8762
			}}
		}}
	}}

	throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
}}";

		internal static string GetFunctionWithValueTypeReturnValueAndNoArguments(int methodHandle, string argumentNames, string returnTypeName,
			string delegateCast, string outInitializers, string methodNameWithOverride, string visibility, string requiresNew, string returnTypeAttributes, bool hasEvents) =>
$@"{returnTypeAttributes}{visibility} {requiresNew} {methodNameWithOverride}
{{
	{outInitializers}

	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
#pragma warning disable CS8604
		var result = methodHandler.Method != null ?
			({returnTypeName})(({delegateCast})methodHandler.Method)({argumentNames}) :
			((R.HandlerInformation<{returnTypeName}>)methodHandler).ReturnValue;
#pragma warning restore CS8604
		{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
		methodHandler.IncrementCallCount();

#pragma warning disable CS8762
		return result;
#pragma warning restore CS8762
	}}

	throw new RE.ExpectationException($""No handlers were found for {methodNameWithOverride.Replace("override ", string.Empty, StringComparison.Ordinal)}"");
}}";

		internal static string GetFunctionForMake(string outInitializers, string methodNameWithOverride, string visibility,
			string requiresNew, string returnTypeAttributes, Type returnType) =>
$@"{returnTypeAttributes}{visibility} {requiresNew} {methodNameWithOverride}
{{
	{outInitializers}

#pragma warning disable CS8762
	return {MethodTemplates.GetDefaultReturnValue(returnType)};
#pragma warning restore CS8762
}}";
	}
}