namespace Rocks.Templates
{
	public static class PropertyTemplates
	{
		public static string GetProperty(string returnType, string name, string getSet, string visibility, string explicitInterfaceName) =>
			$"{visibility} {returnType} {explicitInterfaceName}{name} {{ {getSet} }}";

		public static string GetPropertyIndexer(string returnType, string indexerArguments, string getSet, string visibility, string explicitInterfaceName) =>
			$"{visibility} {returnType} {explicitInterfaceName}this[{indexerArguments}] {{ {getSet} }}";

		public static string GetPropertyGetWithReferenceTypeReturnValue(int methodHandle, string argumentNames, string returnTypeName,
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues, string visibility) =>
$@"{visibility} get
{{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as {delegateCast})({argumentNames}) as {returnTypeName} :
					(methodHandler as R.HandlerInformation<{returnTypeName}>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
	}}
	else
	{{
		throw new S.NotImplementedException();
	}}
}}";

		public static string GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers(int methodHandle, string argumentNames, string returnType,
			string delegateCast, string visibility) =>
$@"{visibility} get
{{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as {delegateCast})({argumentNames}) as {returnType} :
			(methodHandler as R.HandlerInformation<{returnType}>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new S.NotImplementedException();
	}}
}}";

		public static string GetPropertyGetWithValueTypeReturnValue(int methodHandle, string argumentNames, string returnTypeName,
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues, string visibility) =>
$@"{visibility} get
{{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					({returnTypeName})(methodHandler.Method as {delegateCast})({argumentNames}) :
					(methodHandler as R.HandlerInformation<{returnTypeName}>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new RE.ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
	}}
	else
	{{
		throw new S.NotImplementedException();
	}}
}}";

		public static string GetPropertyGetWithValueTypeReturnValueAndNoIndexers(int methodHandle, string argumentNames, string returnType,
			string delegateCast, string visibility) =>
$@"{visibility} get
{{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			({returnType})(methodHandler.Method as {delegateCast})({argumentNames}) :
			(methodHandler as R.HandlerInformation<{returnType}>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new S.NotImplementedException();
	}}
}}";

		public static string GetPropertyGetForMake(string visibility, string returnType) =>
$@"{visibility} get
{{
	return default({returnType});
}}";

		public static string GetPropertySet(int methodHandle, string argumentNames, string expectationTemplateInstances, string delegateCast,
			string methodWithArgumentValues, string visibility) =>
$@"{visibility} set
{{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var foundMatch = false;

		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				foundMatch = true;

				if(methodHandler.Method != null)
				{{
					(methodHandler.Method as {delegateCast})({argumentNames});
				}}
	
				methodHandler.RaiseEvents(this);
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
		throw new S.NotImplementedException();
	}}
}}";

		public static string GetPropertySetAndNoIndexers(int methodHandle, string argumentNames,
			string delegateCast, string visibility) =>
$@"{visibility} set
{{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];

		if(methodHandler.Method != null)
		{{
			(methodHandler.Method as {delegateCast})({argumentNames});
		}}
	
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
	}}
	else
	{{
		throw new S.NotImplementedException();
	}}
}}";

		public static string GetPropertySetForMake(string visibility) =>
$@"{visibility} set {{ }}";

		public static string GetNonPublicProperty(string visibility, string returnType, string name, string getSet, string explicitInterfaceName) =>
			$"{visibility} override {returnType} {explicitInterfaceName}{name} {{ {getSet} }}";

		public static string GetNonPublicPropertyIndexer(string visibility, string returnType, string indexerArguments, string getSet, string explicitInterfaceName) =>
			$"{visibility} override {returnType} {explicitInterfaceName}this[{indexerArguments}] {{ {getSet} }}";

		public static string GetNonPublicPropertyGet(string visibility) => $"{visibility} get;";

		public static string GetNonPublicPropertySet(string visibility) => $"{visibility} set;";
	}
}
