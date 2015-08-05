namespace Rocks.Templates
{
	public static class CodeTemplates
	{
		public const string Internal = "internal";
		public const string Public = "public";
 		public const string Protected = "protected";
		public const string ProtectedAndInternal = "internal protected";

		public static string GetVisibility(bool isMemberFamily, bool isMemberFamilyOrAssembly) =>
			isMemberFamily || isMemberFamilyOrAssembly ? CodeTemplates.Protected : CodeTemplates.Internal;
		
		public static string GetExpectation(string parameterName, string parameterTypeName) => 
			$"(methodHandler.Expectations[\"{parameterName}\"] as ArgumentExpectation<{parameterTypeName}>).IsValid({parameterName}, \"{parameterName}\")";

		internal static string GetIsUnsafe(bool isUnsafe) => isUnsafe ? "unsafe" : string.Empty;

		public static string GetClass(string usingStatements, string mockTypeName, string baseType, 
			string implementedMethods, string implementedProperties, string implementedEvents, string generatedConstructors, string baseTypeNamespace, 
			string classAttributes, string noArgumentConstructor, string additionalCode, bool isUnsafe, string baseTypeConstraints) =>
$@"#pragma warning disable CS8019
{usingStatements}
#pragma warning restore CS8019

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

#pragma warning disable CS0067
		{implementedEvents}
#pragma warning restore CS0067

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
	}
}
