using System.Reflection;

namespace Rocks
{
	public static class Constants
	{
		public static class Values
		{
			public const string PropertySetterArgumentName = "value";
		}

		public static class Reflection
		{
			public const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
			public const BindingFlags PublicNonPublicInstance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		}

		public static class ErrorMessages
		{
			public const string CannotMockSealedType = "Cannot mock the sealed type {0}.";
			public const string NoVirtualMembers = "No public virtual members found on type {0}.";
			public const string VerificationFailed = "Type: {0}, method: {1}, message: {2}.";
		}

		public static class CodeTemplates
		{
			// 0 = type
			// 1 = name
			// 2 = get and/or set
			public const string PropertyTemplate = "public {0} {1} {{ {2} }}";
			// 0 = type
			// 1 = indexer arguments
			// 2 = get and/or set
			public const string PropertyIndexerTemplate = "public {0} this[{1}] {{ {2} }}";

			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = instances of the ExpectationTemplate
			// 4 = delegate cast
			// 5 = method with argument values
			public const string PropertyGetWithReferenceTypeReturnValueTemplate =
@"get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({3})
			{{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as {4})({1}) as {2} :
					(methodHandler as HandlerInformation<{2}>).ReturnValue;
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {5}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = delegate cast
			public const string PropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate =
@"get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as {3})({1}) as {2} :
			(methodHandler as HandlerInformation<{2}>).ReturnValue;
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = instances of the ExpectationTemplate
			// 4 = delegate cast
			// 5 = method with argument values
			public const string PropertyGetWithValueTypeReturnValueTemplate =
@"get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({3})
			{{
				var result = methodHandler.Method != null ?
					({2})(methodHandler.Method as {4})({1}) :
					(methodHandler as HandlerInformation<{2}>).ReturnValue;
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {5}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = delegate cast
			public const string PropertyGetWithValueTypeReturnValueAndNoIndexersTemplate =
@"get
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			({2})(methodHandler.Method as {3})({1}) :
			(methodHandler as HandlerInformation<{2}>).ReturnValue;
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = instances of the ExpectationTemplate
			// 3 = delegate cast
			// 4 = method with argument values
			public const string PropertySetTemplate =
@"set
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var foundMatch = false;

		foreach(var methodHandler in methodHandlers)
		{{
			if({2})
			{{
				foundMatch = true;

				if(methodHandler.Method != null)
				{{
					(methodHandler.Method as {3})({1});
				}}
	
				methodHandler.IncrementCallCount();
				break;
			}}
		}}

		if(!foundMatch)
		{{
			throw new ExpectationException($""No handlers were found for {4}"");
		}}
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = delegate cast
			public const string PropertySetAndNoIndexersTemplate =
@"set
{{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];

		if(methodHandler.Method != null)
		{{
			(methodHandler.Method as {2})({1});
		}}
	
		methodHandler.IncrementCallCount();
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = event type
			// 1 = event name
			public const string EventTemplate = "public event {0} {1};";
			// 0 = parameter name
			public const string ExpectationTemplate = "(methodHandler.Expectations[\"{0}\"] as ArgumentExpectation<{1}>).IsValid({0}, \"{0}\")";

			// 0 = method name
			public const string RefOutNotImplementedMethodTemplate =
@"public {0}
{{
	throw new NotImplementedException();
}}";

			// 0 = type
			// 1 = comma-separate list of argument names
			// 2 = comma-separate list of argument names with types
			public const string ConstructorTemplate =
@"public {0}(ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> handlers, {2})
	: base({1})
{{
	this.handlers = handlers;
}}";

			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = instances of the ExpectationTemplate
			// 3 = delegate cast
			// 4 = out initializers
			// 5 = method with argument values
			public const string ActionMethodTemplate =
@"public {0}
{{
	{4}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var foundMatch = false;
				
		foreach(var methodHandler in methodHandlers)
		{{
			if({2})
			{{
				foundMatch = true;

				if(methodHandler.Method != null)
				{{
					(methodHandler.Method as {3})({1});
				}}
	
				methodHandler.IncrementCallCount();
				break;
			}}
		}}

		if(!foundMatch)
		{{
			throw new ExpectationException($""No handlers were found for {5}"");
		}}
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = delegate cast
			// 3 = out initializers
			public const string ActionMethodWithNoArgumentsTemplate =
@"public {0}
{{
	{3}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		if(methodHandler.Method != null)
		{{
			(methodHandler.Method as {2})({1});
		}}
	
		methodHandler.IncrementCallCount();
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

			// 0 = using statements
			// 1 = mangled name
			// 2 = base type
			// 3 = implemented methods
			// 4 = implemented properties
			// 5 = implemented events
			// 6 = generated constructors
			// 7 = base type's namespace
			// 8 = class attributes
			public const string ClassTemplate =
@"{0}

namespace {7}
{{
	{8}
	public sealed class {1}
		: {2}, IRock
	{{
		private ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> handlers;

		public {1}(ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> handlers)
		{{
			this.handlers = handlers;
		}}

		{6}

		{3}

		{4}

		{5}

		ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> IRock.Handlers
		{{
			get {{ return this.handlers; }}
		}}
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = instances of the ExpectationTemplate
			// 4 = delegate cast
			// 5 = out initializers
			// 6 = method with argument values
			public const string FunctionWithReferenceTypeReturnValueMethodTemplate =
@"public {0}
{{
	{5}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({3})
			{{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as {4})({1}) as {2} :
					(methodHandler as HandlerInformation<{2}>).ReturnValue;
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {6}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = delegate cast
			// 4 = out initializers
			public const string FunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate =
@"public {0}
{{
	{4}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as {3})({1}) as {2} :
			(methodHandler as HandlerInformation<{2}>).ReturnValue;
		methodHandler.IncrementCallCount();
		return result;
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = instances of the ExpectationTemplate
			// 4 = delegate cast
			// 5 = out initializers
			// 6 = method with argument values
			public const string FunctionWithValueTypeReturnValueMethodTemplate =
@"public {0}
{{
	{5}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		foreach(var methodHandler in methodHandlers)
		{{
			if({3})
			{{
				var result = methodHandler.Method != null ?
					({2})(methodHandler.Method as {4})({1}) :
					(methodHandler as HandlerInformation<{2}>).ReturnValue;
				methodHandler.IncrementCallCount();
				return result;
			}}
		}}

		throw new ExpectationException($""No handlers were found for {6}"");
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = delegate cast
			// 4 = out initializers
			public const string FunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate =
@"public {0}
{{
	{4}
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""{0}"", out methodHandlers))
	{{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			({2})(methodHandler.Method as {3})({1}) :
			(methodHandler as HandlerInformation<{2}>).ReturnValue;
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
}
