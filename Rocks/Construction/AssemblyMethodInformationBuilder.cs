using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Rocks.Extensions;

namespace Rocks.Construction
{
	internal sealed class AssemblyMethodInformationBuilder
		: MethodInformationBuilder
	{
		internal AssemblyMethodInformationBuilder(SortedSet<string> namespaces,
			Type baseType)
			: base(namespaces)
		{
			this.BaseType = baseType;
			var name = this.BaseType.IsGenericTypeDefinition ?
				$"{baseType.GetFullName(namespaces)}" : baseType.GetSafeName();
			this.TypeName = $"Rock{name}";
		}

		protected override string GetDelegateCast(MethodInfo baseMethod)
		{
			return $"{this.GetTypeNameWithGenericsAndNoTextFormatting()}_{baseMethod.Name}{this.GetMethodIdentifier(baseMethod)}Delegate{baseMethod.GetGenericArguments(this.Namespaces).Arguments}";
		}

		private string GetMethodIdentifier(MethodInfo baseMethod)
		{
			var methodCount = this.BaseType.GetMethods(ReflectionValues.PublicInstance)
				.Where(_ => _.Name == baseMethod.Name && !_.IsSpecialName && _.IsVirtual).Count();

			return methodCount > 1 ? baseMethod.MethodHandle.Value.ToString() : string.Empty;
		}

		private string GetTypeNameWithGenericsAndNoTextFormatting() => $"{this.TypeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(", ", string.Empty)}";

		internal string TypeName { get; private set; }
		internal Type BaseType { get; private set; }
	}
}
