namespace Rocks.Templates
{
	internal static class ClassTemplates
	{
		// Lifted from: http://code.fitness/post/2019/02/nullableattribute.html
		// TODO: This should go away with https://github.com/dotnet/corefx/issues/36222 ... hopefully.
		internal static string GetNullableAttribute() =>
@"namespace System.Runtime.CompilerServices
{
	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Event | AttributeTargets.Field |
		AttributeTargets.GenericParameter | AttributeTargets.Module | AttributeTargets.Parameter |
		AttributeTargets.Property | AttributeTargets.ReturnValue,
		AllowMultiple = false)]
	public class NullableAttribute : Attribute
	{
		public byte Mode { get; }

		public NullableAttribute(byte mode) => this.Mode = mode;

		public NullableAttribute(byte[] _) => throw new System.NotImplementedException();
	}
}";

		internal static string GetRaiseImplementation() =>
$@"void R.IMockWithEvents.Raise(string eventName, S.EventArgs args)
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

{(!string.IsNullOrWhiteSpace(baseTypeNamespace) ? $"namespace {baseTypeNamespace}" : string.Empty)}
{(!string.IsNullOrWhiteSpace(baseTypeNamespace) ? "{" : string.Empty)}
	{classAttributes}
	public {CodeTemplates.GetIsUnsafe(isUnsafe)} sealed class {mockTypeName}
		: {baseType}, {mockType} {baseTypeConstraints}
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
{(!string.IsNullOrWhiteSpace(baseTypeNamespace) ? "}" : string.Empty)}";
	}
}
