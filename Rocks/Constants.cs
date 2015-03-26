using System.Reflection;

namespace Rocks
{
	public static class Constants
	{
		public static class Reflection
		{
			public const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
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
			public const string PropertyGetWithReferenceTypeReturnValueTemplate =
@"get
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
			public const string PropertyGetWithValueTypeReturnValueTemplate =
@"get
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
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = instances of the ExpectationTemplate
			// 3 = delegate cast
			public const string PropertySetTemplate =
@"set
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
			// 0 = event type
			// 1 = event name
			public const string EventTemplate = "public event {0} {1};";
			// 0 = parameter name
			public const string ExpectationTemplate = "(handler.Expectations[\"{0}\"] as ArgumentExpectation<{1}>).Validate({0}, \"{0}\");";

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
@"public {0}(ReadOnlyDictionary<string, HandlerInformation> handlers, {2})
	: base({1})
{{
	this.handlers = handlers;
}}";

			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = instances of the ExpectationTemplate
			// 3 = delegate cast
			// 4 = out initializers
			public const string ActionMethodTemplate =
@"public {0}
{{
	{4}
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
			// 6 = generated constructors
			// 7 = base type's namespace
			public const string ClassTemplate =
@"{0}

namespace {7}
{{
	public sealed class {1}
		: {2}, IRock
	{{
		private ReadOnlyDictionary<string, HandlerInformation> handlers;

		public {1}(ReadOnlyDictionary<string, HandlerInformation> handlers)
		{{
			this.handlers = handlers;
		}}

		{6}

		{3}

		{4}

		{5}

		ReadOnlyDictionary<string, HandlerInformation> IRock.Handlers
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
			public const string FunctionWithReferenceTypeReturnValueMethodTemplate =
@"public {0}
{{
	{5}
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
			// 5 = out initializers
			public const string FunctionWithValueTypeReturnValueMethodTemplate =
@"public {0}
{{
	{5}
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
