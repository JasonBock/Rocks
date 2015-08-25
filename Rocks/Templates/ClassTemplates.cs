namespace Rocks.Templates
{
	internal static class ClassTemplates
	{
		internal static string GetClassWithObsoleteSuppression(string classDefinition) =>
$@"#pragma warning disable CS0618
#pragma warning disable CS0672
{classDefinition}
#pragma warning restore CS0672
#pragma warning restore CS0618";

		internal static string GetClass(string usingStatements, string mockTypeName, string baseType,
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
	}
}
