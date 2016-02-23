using Rocks.Extensions;
using Rocks.Options;
using Rocks.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using static Rocks.Extensions.MethodBaseExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction.Persistence
{
	internal sealed class PersistenceBuilder
		: Builder<PersistenceMethodInformationBuilder>
	{
		private readonly List<string> generatedDelegates = new List<string>();

		internal PersistenceBuilder(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, RockOptions options)
			: base(baseType, handlers, namespaces, options, new PersistenceNameGenerator(baseType),
				  new PersistenceMethodInformationBuilder(namespaces, baseType), new PersistenceTypeNameGenerator(namespaces), false)
		{ }

		protected override string GetDirectoryForFile()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), this.BaseType.Namespace.Replace(".", "\\"));
		}

		private string GetMethodIdentifier(MethodInfo baseMethod)
		{
			var methodCount = this.BaseType.GetMethods(ReflectionValues.PublicInstance)
				.Where(_ => _.Name == baseMethod.Name && !_.IsSpecialName && _.IsVirtual).Count();

			return methodCount > 1 ? baseMethod.MethodHandle.Value.ToString() : string.Empty;
		}

		protected override string GetAdditionNamespaceCode()
		{
			return string.Join(Environment.NewLine, this.generatedDelegates);
		}

		protected override void HandleRefOutMethod(MethodInfo baseMethod, MethodInformation methodDescription)
		{
			this.generatedDelegates.Add(MethodTemplates.GetAssemblyDelegate(
				baseMethod.ReturnType == typeof(void) ? "void" : baseMethod.ReturnType.GetSafeName(this.Namespaces),
				methodDescription.DelegateCast,
				baseMethod.GetParameters(this.Namespaces), baseMethod.IsUnsafeToMock()));
		}
	}
}
