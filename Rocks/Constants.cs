namespace Rocks
{
	public static class Constants
	{
		public static class ErrorMessages
		{
			public const string CannotMockSealedType = "Cannot mock the sealed type {0}.";
		}

		public static class CodeTemplates
		{
			// 0 = method name
			// 1 = comma-separate list of argument names
			public const string ActionMethodTemplate = @"
public {0}
{{
	Delegate handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		handler.DynamicInvoke({1});
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

			// 0 = mangled name
			// 1 = interface name
			public const string ClassTemplate = @"
{0}

public sealed class {1}
	: {2}
{{
	private ReadOnlyDictionary<string, Delegate> handlers;

	public {1}(ReadOnlyDictionary<string, Delegate> handlers)
	{{
		this.handlers = handlers;
	}}

	{3}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			public const string FunctionMethodTemplate = @"
public {0}
{{
	Delegate handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		return ({2})handler.DynamicInvoke({1});
	}}
	else
	{{
		throw new NotImplementedException();
	}}
}}";

		}
	}
}
