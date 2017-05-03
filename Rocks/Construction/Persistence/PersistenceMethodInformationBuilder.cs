using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Construction.Persistence
{
	internal sealed class PersistenceMethodInformationBuilder
		: MethodInformationBuilder
	{
		internal PersistenceMethodInformationBuilder(SortedSet<string> namespaces, Type baseType)
			: base(namespaces) => this.BaseType = baseType;

		protected override string GetDelegateCast(MethodInfo baseMethod) =>
			$"{this.GetTypeNameWithGenericsAndNoTextFormatting()}_{baseMethod.Name}{this.GetMethodIdentifier(baseMethod)}Delegate{baseMethod.GetGenericArguments(this.Namespaces).Arguments}";

		private string GetMethodIdentifier(MethodInfo baseMethod)
		{
			var methodCount = this.BaseType.GetMethods(ReflectionValues.PublicInstance)
				.Where(_ => _.Name == baseMethod.Name && !_.IsSpecialName && _.IsVirtual).Count();

			return methodCount > 1 ? baseMethod.MetadataToken.ToString() : string.Empty;
		}

		private string GetTypeNameWithGenericsAndNoTextFormatting() => 
			$"{new PersistenceTypeNameGenerator(this.Namespaces).Generate(this.BaseType).Replace("<", string.Empty).Replace(">", string.Empty).Replace(", ", string.Empty)}";

		internal Type BaseType { get; }
	}
}
