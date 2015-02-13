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
	System.Delegate handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		handler.DynamicInvoke({1});
	}}
	else
	{{
		throw new System.NotImplementedException();
	}}
}}";

			// 0 = mangled name
			// 1 = interface name
			public const string ClassTemplate = @"
public sealed class {0}
	: {1}
{{
	private System.Collections.ObjectModel.ReadOnlyDictionary<System.String, System.Delegate> handlers;

	public {0}(System.Collections.ObjectModel.ReadOnlyDictionary<System.String, System.Delegate> handlers)
	{{
		this.handlers = handlers;
	}}

	{2}
}}";
			// 0 = method name
			// 1 = comma-separate list of argument names
			// 2 = return type name
			public const string FunctionMethodTemplate = @"
public {0}
{{
	System.Delegate handler = null;

	if (this.handlers.TryGetValue(""{0}"", out handler))
	{{
		return ({2})handler.DynamicInvoke({1});
	}}
	else
	{{
		throw new System.NotImplementedException();
	}}
}}";

		}
	}
}
