namespace Rocks
{
	public static class CodeTemplates
	{
		public static string GetAssemblyDelegateTemplate(string returnType, string delegateName, string arguments) => 
			$"public delegate {returnType} {delegateName}({arguments});";
		
		public static string GetPropertyTemplate(string returnType, string name, string getSet) => 
			$"public {returnType} {name} {{ {getSet} }}";

		public static string GetPropertyIndexerTemplate(string returnType, string indexerArguments, string getSet) => 
			$"public {returnType} this[{indexerArguments}] {{ {getSet} }}";

		public static string GetPropertyGetWithReferenceTypeReturnValueTemplate(int methodHandle, string argumentNames, string returnTypeName, 
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues) =>
$@"get
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

		public static string GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate(int methodHandle, string argumentNames, string returnType, string delegateCast) =>
$@"get
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
			string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues) =>
$@"get
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

		public static string GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate(int methodHandle, string argumentNames, string returnType, string delegateCast) =>
$@"get
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

		public static string GetPropertySetTemplate(int methodHandle, string argumentNames, string expectationTemplateInstances, string delegateCast, string methodWithArgumentValues) =>
$@"set
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

		public static string GetPropertySetAndNoIndexersTemplate(int methodHandle, string argumentNames, string delegateCast) =>
$@"set
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

		public static string GetEventTemplate(string eventType, string eventName) => $"public event {eventType} {eventName};";

		public static string GetExpectationTemplate(string parameterName, string parameterTypeName) => 
			$"(methodHandler.Expectations[\"{parameterName}\"] as ArgumentExpectation<{parameterTypeName}>).IsValid({parameterName}, \"{parameterName}\")";

		public static string GetRefOutNotImplementedMethodTemplate(string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
{{
	throw new NotImplementedException();
}}";

		public static string GetConstructorTemplate(string typeName, string argumentNames, string argumentNamesWithTypes) =>
$@"public {typeName}(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers, {argumentNamesWithTypes})
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
			string outInitializers, string methodWithArgumentValues, string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
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

		public static string GetActionMethodWithNoArgumentsTemplate(int methodHandle, string argumentNames, string delegateCast, string outInitializers, string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
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

		public static string GetClassTemplate(string usingStatements, string mockTypeName, string baseType, string implementedMethods,
			string implementedProperties, string implementedEvents, string generatedConstructors, string baseTypeNamespace, 
			string classAttributes, string noArgumentConstructor, string constructorName, string additionalCode) =>
$@"{usingStatements}

namespace {baseTypeNamespace}
{{
	{classAttributes}
	public sealed class {mockTypeName}
		: {baseType}, IMock
	{{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;

		{noArgumentConstructor}

		public {constructorName}(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers)
		{{
			this.handlers = handlers;
		}}

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
	}}

	{additionalCode}
}}";

		public static string GetFunctionWithReferenceTypeReturnValueMethodTemplate(int methodHandle, string argumentNames, string returnTypeName, string expectationTemplateInstances,
			string delegateCast, string outInitializers, string methodWithArgumentValues, string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
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
			string delegateCast, string outInitializers, string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
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
			string delegateCast, string outInitializers, string methodWithArgumentValues, string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
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
			string delegateCast, string outInitializers, string methodNameWithOverride) =>
$@"public {methodNameWithOverride}
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
