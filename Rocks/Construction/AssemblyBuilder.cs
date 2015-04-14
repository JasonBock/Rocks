using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal sealed class AssemblyBuilder
		: Builder
	{
		//public delegate string MyDelegate(string a);
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
			// TODO: Need to change what the name of the delegate is here
			// and stuff it into generatedDelegates;
			var delegateCast = !containsRefAndOrOutParameters ?
				baseMethod.GetDelegateCast() :
				(this.Handlers.ContainsKey(description) ?
					this.Handlers[description][0].Method.GetType().GetSafeName(baseMethod, this.Namespaces) : string.Empty);

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

		protected override void HandleRefOutMethod(MethodInfo baseMethod, string methodDescription, string delegateCast)
		{
			base.HandleRefOutMethod(baseMethod, methodDescription, delegateCast);
		}
	}
}
