namespace Rocks
{
	public static class Constants
	{
		public static class ErrorMessages
		{
			public const string CannotMockSealedType = "Cannot mock the sealed type {0}.";
			public const string VerificationFailed = "Type: {0}, method: {1}, message: {2}.";
		}

		public static class CodeTemplates
		{
			// 0 = type
			// 1 = name
			// 2 = get and/or set
			public const string PropertyTemplate = "public {0} {1} {{ {2} }}";
			// 0 = type
			// 1 = indexer type
			// 2 = indexer name
			// 3 = get and/or set
			public const string PropertyIndexerTemplate = "public {0} this[{1} {2}] {{ {3} }}";
			// 0 = event type
			// 1 = event name
			public const string EventTemplate = "public event {0} {1};";
			// 0 = parameter name
			public const string ExpectationTemplate = "(handler.Expectations[\"{0}\"] as ArgumentExpectation<{1}>).Validate({0}, \"{0}\");";

			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = instances of the ExpectationTemplate
			// 3 = delegate cast
			public const string ActionMethodTemplate = 
@"public {0}
{{
	HandlerInformation handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		{2}
		if(handler.Method != null)
		{{
			(handler.Method as {3})({1});
		}}
	
		handler.IncrementCallCount();
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
			public const string ClassTemplate = 
@"{0}

public sealed class {1}
	: {2}, IRock
{{
	private ReadOnlyDictionary<string, HandlerInformation> handlers;

	public {1}(ReadOnlyDictionary<string, HandlerInformation> handlers)
	{{
		this.handlers = handlers;
	}}

	{3}

	{4}

	{5}

	ReadOnlyDictionary<string, HandlerInformation> IRock.Handlers
	{{
		get {{ return this.handlers; }}
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			// 3 = instances of the ExpectationTemplate
			// 4 = delegate cast
			public const string FunctionWithReferenceTypeReturnValueMethodTemplate = 
@"public {0}
{{
	HandlerInformation handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		{3}
		var result = handler.Method != null ?
			(handler.Method as {4})({1}) as {2} :
			(handler as HandlerInformation<{2}>).ReturnValue;
		handler.IncrementCallCount();
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
			public const string FunctionWithValueTypeReturnValueMethodTemplate = 
@"public {0}
{{
	HandlerInformation handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		{3}
		var result = handler.Method != null ?
			({2})(handler.Method as {4})({1}) :
			(handler as HandlerInformation<{2}>).ReturnValue;
		handler.IncrementCallCount();
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
