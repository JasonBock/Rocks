using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options)
			: base(baseType, handlers, namespaces, options, new AssemblyNameGenerator(baseType))
		{
			var name = this.BaseType.IsGenericTypeDefinition ?
				$"{baseType.GetSafeName()}{this.BaseType.GetGenericArguments(this.Namespaces).Arguments}" : baseType.GetSafeName();
			this.TypeName = $"Rock{name}";
		}

		protected override string GetDirectoryForFile()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), this.BaseType.Namespace.Replace(".", "\\"));
		}

		protected override MethodInformation GetMethodInformation(MockableResult<MethodInfo> method)
		{
			var baseMethod = method.Value;
			var description = baseMethod.GetMethodDescription(this.Namespaces);
			var descriptionWithOverride = baseMethod.GetMethodDescription(this.Namespaces, true, method.RequiresExplicitInterfaceImplementation);
			var containsRefAndOrOutParametersOrPointerTypes = baseMethod.ContainsRefAndOrOutParametersOrPointerTypes();
			string delegateCast = null;

			if(!containsRefAndOrOutParametersOrPointerTypes)
			{
				delegateCast = baseMethod.GetDelegateCast();
         }
			else
			{
				delegateCast = $"{this.GetTypeNameWithGenericsAndNoTextFormatting()}_{baseMethod.Name}{this.GetMethodIdentifier(baseMethod)}Delegate{baseMethod.GetGenericArguments(this.Namespaces).Arguments}";
			}

			return new MethodInformation
			{
				ContainsRefAndOrOutParametersOrPointerTypes = containsRefAndOrOutParametersOrPointerTypes,
				DelegateCast = delegateCast,
				Description = description,
				DescriptionWithOverride = descriptionWithOverride
			};
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
			this.generatedDelegates.Add(CodeTemplates.GetAssemblyDelegateTemplate(
				baseMethod.ReturnType == typeof(void) ? "void" : baseMethod.ReturnType.GetSafeName(null, this.Namespaces),
				methodDescription.DelegateCast,
				baseMethod.GetParameters(this.Namespaces), baseMethod.IsUnsafeToMock()));
		}
	}
}
