namespace Rocks.Templates
{
	internal static class ClassTemplates
	{
		internal static string GetRaiseImplementation() =>
		{{
			var thisType = this.GetType();

			var eventDelegate = (S.MulticastDelegate)thisType.GetField(eventName, 
				SR.BindingFlags.Instance | SR.BindingFlags.NonPublic).GetValue(this);

			if (eventDelegate != null)
			{{
				foreach (var handler in eventDelegate.GetInvocationList())
				{{
					handler.Method.Invoke(handler.Target, new object[] {{ this, args }});
				}}
			}}
		}}
";

		internal static string GetClassWithObsoleteSuppression(string classDefinition) =>
$@"#pragma warning disable CS0618
#pragma warning disable CS0672
{classDefinition}
#pragma warning restore CS0672
#pragma warning restore CS0618";

#if !NETCOREAPP1_1
		internal static string GetClass(string usingStatements, string mockTypeName, string baseType, 
			string implementedMethods, string implementedProperties, string implementedEvents, string generatedConstructors, string baseTypeNamespace,
			string classAttributes, string noArgumentConstructor, string additionalCode, bool isUnsafe, string baseTypeConstraints,
			string mockType, string raiseImplementation) =>
$@"#pragma warning disable CS8019
using R = Rocks;
using RE = Rocks.Exceptions;
using S = System;
using SCG = System.Collections.Generic;
using SCO = System.Collections.ObjectModel;
using SR = System.Reflection;
using STT = System.Threading.Tasks;
{usingStatements}
#pragma warning restore CS8019

namespace {baseTypeNamespace}
{{
	{classAttributes}
	public {CodeTemplates.GetIsUnsafe(isUnsafe)} sealed class {mockTypeName}
	{{
		private SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlers;

		{noArgumentConstructor}

		{generatedConstructors}

		{implementedMethods}

		{implementedProperties}

		{implementedEvents}

		SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> R.IMock.Handlers => this.handlers;

		{raiseImplementation}
		
		{additionalCode}
	}}
}}";
	}
#else
		internal static string GetClass(string usingStatements, string mockTypeName, string baseType,
			string implementedMethods, string implementedProperties, string implementedEvents, string generatedConstructors, string baseTypeNamespace,
			string classAttributes, string noArgumentConstructor, string additionalCode, bool isUnsafe, string baseTypeConstraints,
			string mockType, string raiseImplementation) =>
		$@"#pragma warning disable CS8019
using R = Rocks;
using RE = Rocks.Exceptions;
using S = System;
using SCG = System.Collections.Generic;
using SCO = System.Collections.ObjectModel;
using SR = System.Reflection;
using STT = System.Threading.Tasks;
{usingStatements}
using System.Reflection;
#pragma warning restore CS8019

namespace {baseTypeNamespace}
{{
	{classAttributes}
	public {CodeTemplates.GetIsUnsafe(isUnsafe)} sealed class {mockTypeName}
	{{
		private SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlers;

		{noArgumentConstructor}

		{generatedConstructors}

		{implementedMethods}

		{implementedProperties}

		{implementedEvents}

		SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> R.IMock.Handlers => this.handlers;

		{raiseImplementation}

		{additionalCode}
	}}
}}";
	}
#endif
}
