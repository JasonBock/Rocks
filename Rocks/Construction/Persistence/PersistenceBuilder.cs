using Rocks.Extensions;
using Rocks.Options;
using Rocks.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

		protected override string GetDirectoryForFile() =>
			Path.Combine(this.Options.CodeFileDirectory, this.BaseType.Namespace.Replace(".", "\\"));

		protected override string GetAdditionNamespaceCode() =>
			string.Join(Environment.NewLine, this.generatedDelegates);

		protected override void HandleRefOutMethod(MethodInfo baseMethod, MethodInformation methodDescription)
		{
			var returnType = baseMethod.ReturnType;
			this.generatedDelegates.Add(MethodTemplates.GetAssemblyDelegate(
				baseMethod.ReturnType == typeof(void) ? "void" : TypeDissector.Create(returnType).SafeName,
				methodDescription.DelegateCast,
				baseMethod.GetParameters(this.Namespaces), baseMethod.IsUnsafeToMock()));
			this.Namespaces.Add(returnType.Namespace);
		}
	}
}