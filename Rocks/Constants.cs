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
			// 0 = method name
			// 1 = comma-separate list of argument names
			public const string ActionMethodTemplate = @"
public {0}
{{
	HandlerInformation handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		handler.Method.DynamicInvoke({1});
		handler.IncrementCallCount();
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

			// 0 = mangled name
			// 1 = interface name
			public const string ClassTemplate = @"{0}

public sealed class {1}
	: {2}, IRock
{{
	private ReadOnlyDictionary<string, HandlerInformation> handlers;

	public {1}(ReadOnlyDictionary<string, HandlerInformation> handlers)
	{{
		this.handlers = handlers;
	}}

	{3}

	ReadOnlyDictionary<string, HandlerInformation> IRock.Handlers
	{{
		get {{ return this.handlers; }}
	}}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			public const string FunctionMethodTemplate = @"
public {0}
{{
	HandlerInformation handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		var result = ({2})handler.Method.DynamicInvoke({1});
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
