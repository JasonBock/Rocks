namespace Rocks
{
	public static class CodeTemplates
	{
		public const string Internal = "internal";
		public const string Public = "public";
 		public const string Protected = "protected";
		public const string ProtectedAndInternal = "internal protected";

		public static string GetVisibility(bool isMemberFamily, bool isMemberFamilyOrAssembly) =>
			isMemberFamily || isMemberFamilyOrAssembly ? CodeTemplates.Protected : CodeTemplates.Internal;

		public static string GetAssemblyDelegateTemplate(string returnType, string delegateName, string arguments, bool isUnsafe) => 
			$"public {CodeTemplates.GetIsUnsafe(isUnsafe)} delegate {returnType} {delegateName}({arguments});";
		
		public static string GetPropertyTemplate(string returnType, string name, string getSet, string visibility, string explicitInterfaceName) => 
			$"{visibility} {returnType} {explicitInterfaceName}{name} {{ {getSet} }}";

		public static string GetPropertyIndexerTemplate(string returnType, string indexerArguments, string getSet, string visibility, string explicitInterfaceName) => 
			$"{visibility} {returnType} {explicitInterfaceName}this[{indexerArguments}] {{ {getSet} }}";

		public static string GetPropertyGetWithReferenceTypeReturnValueTemplate(int methodHandle, string argumentNames, string returnTypeName, 
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues, string visibility) =>
$@"{visibility} get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as {delegateCast})({argumentNames}) as {returnTypeName} :
					(methodHandler as HandlerInformation<{returnTypeName}>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate(int methodHandle, string argumentNames, string returnType, 
			string delegateCast, string visibility) =>
$@"{visibility} get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as {delegateCast})({argumentNames}) as {returnType} :
			(methodHandler as HandlerInformation<{returnType}>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetPropertyGetWithValueTypeReturnValueTemplate(int methodHandle, string argumentNames, string returnTypeName,
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues, string visibility) =>
$@"{visibility} get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					({returnTypeName})(methodHandler.Method as {delegateCast})({argumentNames}) :
					(methodHandler as HandlerInformation<{returnTypeName}>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate(int methodHandle, string argumentNames, string returnType, 
			string delegateCast, string visibility) =>
$@"{visibility} get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			({returnType})(methodHandler.Method as {delegateCast})({argumentNames}) :
			(methodHandler as HandlerInformation<{returnType}>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetPropertySetTemplate(int methodHandle, string argumentNames, string expectationTemplateInstances, string delegateCast, 
			string methodWithArgumentValues, string visibility) =>
$@"{visibility} set
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

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
			throw new ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
		}}
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetPropertySetAndNoIndexersTemplate(int methodHandle, string argumentNames, 
			string delegateCast, string visibility) =>
$@"{visibility} set
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

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
		throw new NotImplementedException();
	}}
}}";

		public static string GetNonPublicEventTemplate(string visibility, string eventType, string eventName) => $"{visibility} override event {eventType} {eventName};";

		public static string GetEventTemplate(string @override, string eventType, string eventName) => $"public {@override} event {eventType} {eventName};";

		public static string GetExpectationTemplate(string parameterName, string parameterTypeName) => 
			$"(methodHandler.Expectations[\"{parameterName}\"] as ArgumentExpectation<{parameterTypeName}>).IsValid({parameterName}, \"{parameterName}\")";

		public static string GetNonPublicActionImplementationTemplate(string visibility, string methodName, string outInitializers) =>
$@"{visibility} override {methodName}
{{
	{outInitializers}	
}}";

		public static string GetNonPublicFunctionImplementationTemplate(string visibility, string methodName, string outInitializers, string returnTypeName) =>
$@"{visibility} override {methodName}
{{
	{outInitializers}	
	
	return default({returnTypeName});
}}";

		public static string GetNonPublicPropertyTemplate(string visibility, string returnType, string name, string getSet, string explicitInterfaceName) =>
			$"{visibility} override {returnType} {explicitInterfaceName}{name} {{ {getSet} }}";

		public static string GetNonPublicPropertyIndexerTemplate(string visibility, string returnType, string indexerArguments, string getSet, string explicitInterfaceName) =>
			$"{visibility} override {returnType} {explicitInterfaceName}this[{indexerArguments}] {{ {getSet} }}";

		public static string GetNonPublicPropertyGetTemplate(string visibility) => $"{visibility} get;";

		public static string GetNonPublicPropertySetTemplate(string visibility) => $"{visibility} set;";

		public static string GetRefOutNotImplementedMethodTemplate(string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
{{
	throw new NotImplementedException();
}}";

		public static string GetConstructorTemplate(string typeName, string argumentNames, string argumentNamesWithTypes) =>
$@"public {typeName}(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers{argumentNamesWithTypes})
	: base({argumentNames})
{{
	this.handlers = handlers;
}}";

		public static string GetConstructorNoArgumentsTemplate(string mockTypeName) =>
$@"public {mockTypeName}() 
{{ 
	this.handlers = new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
		new System.Collections.Generic.Dictionary<int, ReadOnlyCollection<HandlerInformation>>());
}}";

		public static string GetActionMethodTemplate(int methodHandle, string argumentNames, string expectationTemplateInstances, string delegateCast,
			string outInitializers, string methodWithArgumentValues, string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

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
			throw new ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
		}}
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetActionMethodWithNoArgumentsTemplate(int methodHandle, string argumentNames, string delegateCast, string outInitializers, 
			string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

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
		throw new NotImplementedException();
	}}
}}";

		private static string GetIsUnsafe(bool isUnsafe) => isUnsafe ? "unsafe" : string.Empty;

		public static string GetClassTemplate(string usingStatements, string mockTypeName, string baseType, 
			string implementedMethods, string implementedProperties, string implementedEvents, string generatedConstructors, string baseTypeNamespace, 
			string classAttributes, string noArgumentConstructor, string additionalCode, bool isUnsafe, string baseTypeConstraints) =>
$@"{usingStatements}

namespace {baseTypeNamespace}
{{
	{classAttributes}
	public {CodeTemplates.GetIsUnsafe(isUnsafe)} sealed class {mockTypeName}
		: {baseType}, IMock {baseTypeConstraints}
	{{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;

		{noArgumentConstructor}

		{generatedConstructors}

		{implementedMethods}

		{implementedProperties}

		{implementedEvents}

		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> IMock.Handlers
		{{
			get {{ return this.handlers; }}
		}}

		void IMock.Raise(string eventName, EventArgs args)
		{{
			var thisType = this.GetType();

			var eventDelegate = (MulticastDelegate)thisType.GetField(eventName, 
				BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

			if (eventDelegate != null)
			{{
				foreach (var handler in eventDelegate.GetInvocationList())
				{{
					handler.Method.Invoke(handler.Target, new object[] {{ this, args }});
				}}
			}}
		}}

		{additionalCode}
	}}
}}";

		public static string GetFunctionWithReferenceTypeReturnValueMethodTemplate(int methodHandle, string argumentNames, string returnTypeName, string expectationTemplateInstances,
			string delegateCast, string outInitializers, string methodWithArgumentValues, string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as {delegateCast})({argumentNames}) as {returnTypeName} :
					(methodHandler as HandlerInformation<{returnTypeName}>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate(int methodHandle, string argumentNames, string returnTypeName, 
			string delegateCast, string outInitializers, string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as {delegateCast})({argumentNames}) as {returnTypeName} :
			(methodHandler as HandlerInformation<{returnTypeName}>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetFunctionWithValueTypeReturnValueMethodTemplate(int methodHandle, string argumentNames, string returnTypeName, string expectationTemplateInstances,
			string delegateCast, string outInitializers, string methodWithArgumentValues, string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({expectationTemplateInstances})
			{{
				var result = methodHandler.Method != null ?
					({returnTypeName})(methodHandler.Method as {delegateCast})({argumentNames}) :
					(methodHandler as HandlerInformation<{returnTypeName}>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {methodWithArgumentValues}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		public static string GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate(int methodHandle, string argumentNames, string returnTypeName,
			string delegateCast, string outInitializers, string methodNameWithOverride, string visibility) =>
$@"{visibility} {methodNameWithOverride}
{{
	{outInitializers}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue({methodHandle}, out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			({returnTypeName})(methodHandler.Method as {delegateCast})({argumentNames}) :
			(methodHandler as HandlerInformation<{returnTypeName}>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
	}
}
