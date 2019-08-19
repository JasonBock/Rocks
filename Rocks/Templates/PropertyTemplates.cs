using System.Globalization;

namespace Rocks.Templates
{
	public static class PropertyTemplates
	{
		public static string GetProperty(string returnType, string name, string getSet, string visibility, string explicitInterfaceName) =>
			$"{visibility} {returnType} {explicitInterfaceName}{name} {{ {getSet} }}";

		public static string GetPropertyIndexer(string returnType, string indexerArguments, string getSet, string visibility, string explicitInterfaceName) =>
			$"{visibility} {returnType} {explicitInterfaceName}this[{indexerArguments}] {{ {getSet} }}";

		public static string GetPropertyGetWithReferenceTypeReturnValue(int methodHandle, string argumentNames, string returnTypeName,
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues, string visibility, bool hasEvents) =>
$@"{visibility} get
{{
	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					({returnTypeName})(({delegateCast})methodHandler.Method)({argumentNames}) :
					((R.HandlerInformation<{returnTypeName}>)methodHandler).ReturnValue;
				{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}
	}}

	throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
}}";

		public static string GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers(int methodHandle, string argumentNames, string returnType,
			string delegateCast, string visibility, bool hasEvents) =>
$@"{visibility} get
{{
	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			({returnType})(({delegateCast})methodHandler.Method)({argumentNames}) :
			((R.HandlerInformation<{returnType}>)methodHandler).ReturnValue;
		{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
		methodHandler.IncrementCallCount();
		return result;
	}}

	throw new RE.ExpectationException(""No handlers were found."");
}}";

		public static string GetPropertyGetWithValueTypeReturnValue(int methodHandle, string argumentNames, string returnTypeName,
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues, string visibility, bool hasEvents) =>
$@"{visibility} get
{{
	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					({returnTypeName})(({delegateCast})methodHandler.Method)({argumentNames}) :
					((R.HandlerInformation<{returnTypeName}>)methodHandler).ReturnValue;
				{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}
	}}

	throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
}}";

		public static string GetPropertyGetWithValueTypeReturnValueAndNoIndexers(int methodHandle, string argumentNames, string returnType,
			string delegateCast, string visibility, bool hasEvents) =>
$@"{visibility} get
{{
	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			({returnType})(({delegateCast})methodHandler.Method)({argumentNames}) :
			((R.HandlerInformation<{returnType}>)methodHandler).ReturnValue;
		{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
		methodHandler.IncrementCallCount();
		return result;
	}}

	throw new RE.ExpectationException(""No handlers were found."");
}}";

		public static string GetPropertyGetForMake(string visibility) =>
$@"{visibility} get => default;";

		public static string GetPropertyGetForSpanLike(string visibility) =>
$@"{visibility} get => throw new S.NotImplementedException();";

		public static string GetPropertySet(int methodHandle, string argumentNames, string expectationTemplateInstances, string delegateCast,
			string methodWithArgumentValues, string visibility, bool hasEvents) =>
$@"{visibility} set
{{
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
					(({delegateCast})methodHandler.Method)({argumentNames});
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

		public static string GetPropertySetAndNoIndexers(int methodHandle, string argumentNames,
			string delegateCast, string visibility, bool hasEvents) =>
$@"{visibility} set
{{
	if (this.handlers.TryGetValue({methodHandle.ToString(CultureInfo.CurrentCulture)}, out var methodHandlers))
	{{
		var methodHandler = methodHandlers[0];

		if(methodHandler.Method != null)
		{{
			(({delegateCast})methodHandler.Method)({argumentNames});
		}}
	
		{(hasEvents ? "methodHandler.RaiseEvents(this);" : string.Empty)}
		methodHandler.IncrementCallCount();
	}}
	else
	{{
		throw new RE.ExpectationException(""No handlers were found."");
	}}
}}";

		public static string GetPropertySetForMake(string visibility) =>
$@"{visibility} set {{ }}";

		public static string GetPropertySetForSpanLike(string visibility) =>
$@"{visibility} set => throw new S.NotImplementedException();";

		public static string GetNonPublicProperty(string visibility, string returnType, string name, string getSet, string explicitInterfaceName) =>
			$"{visibility} override {returnType} {explicitInterfaceName}{name} {{ {getSet} }}";

		public static string GetNonPublicPropertyIndexer(string visibility, string returnType, string indexerArguments, string getSet, string explicitInterfaceName) =>
			$"{visibility} override {returnType} {explicitInterfaceName}this[{indexerArguments}] {{ {getSet} }}";

		public static string GetNonPublicPropertyGet(string visibility) => $"{visibility} get;";

		public static string GetNonPublicPropertySet(string visibility) => $"{visibility} set;";
	}
}