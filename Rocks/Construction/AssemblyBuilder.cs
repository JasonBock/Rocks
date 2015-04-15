using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using static Rocks.Extensions.MethodBaseExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal sealed class AssemblyBuilder
		: Builder
	{
		private readonly List<string> generatedDelegates = new List<string>();

		internal AssemblyBuilder(Type baseType,
			ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options)
			: base(baseType, handlers, namespaces, options)
		{
			this.TypeName = $"Rock{baseType.GetSafeName()}";
		}

		protected override string GetDirectoryForFile()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), this.BaseType.Namespace.Replace(".", "\\"));
		}

		protected override MethodDescription GetMethodDescription(MethodInfo baseMethod)
		{
			var description = baseMethod.GetMethodDescription(this.Namespaces);
			var containsRefAndOrOutParameters = baseMethod.ContainsRefAndOrOutParameters();
			string delegateCast = null;

			if(!containsRefAndOrOutParameters)
			{
				delegateCast = baseMethod.GetDelegateCast();
         }
			else
			{
				var delegateName = $"{this.GetTypeNameWithNoGenerics()}_{baseMethod.Name}{baseMethod.MethodHandle.Value.ToInt32()}Delegate";
				delegateCast = delegateName;
				this.generatedDelegates.Add(string.Format(Constants.CodeTemplates.AssemblyDelegateTemplate,
					baseMethod.ReturnType == typeof(void) ? "void" : baseMethod.ReturnType.GetSafeName(null, this.Namespaces),
					delegateName,
					baseMethod.GetParameters(this.Namespaces)));
			}

			return new MethodDescription
			{
				ContainsRefAndOrOutParameters = containsRefAndOrOutParameters,
				DelegateCast = delegateCast,
				Description = description
			};
		}

		protected override string GetAdditionNamespaceCode()
		{
			return string.Join(Environment.NewLine, this.generatedDelegates);
		}

		protected override void HandleRefOutMethod(MethodInfo baseMethod, MethodDescription methodDescription)
		{
			base.HandleRefOutMethod(baseMethod, methodDescription);
		}
	}
}
