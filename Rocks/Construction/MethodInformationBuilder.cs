using Rocks.Extensions;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Construction
{
	internal abstract class MethodInformationBuilder
	{
		internal MethodInformationBuilder(SortedSet<string> namespaces)
		{
			this.Namespaces = namespaces;
		}

		internal MethodInformation Build(MockableResult<MethodInfo> method)
		{
			var baseMethod = method.Value;
			var description = baseMethod.GetMethodDescription(this.Namespaces);
			var descriptionWithOverride = baseMethod.GetMethodDescription(
				this.Namespaces, true, method.RequiresExplicitInterfaceImplementation);
			var containsDelegateConditions = baseMethod.ContainsDelegateConditions();

			var delegateCast = !containsDelegateConditions ? baseMethod.GetDelegateCast() :
				this.GetDelegateCast(baseMethod);

			return new MethodInformation(containsDelegateConditions, delegateCast,
				description, descriptionWithOverride);
		}

		protected abstract string GetDelegateCast(MethodInfo baseMethod);
		internal SortedSet<string> Namespaces { get; private set; }
	}
}
