using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
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
			$"{this.GetTypeNameWithGenericsAndNoTextFormatting()}_{baseMethod.Name}{this.GetMethodIdentifier(baseMethod)}Delegate{baseMethod.GetGenericArguments(this.Namespaces).arguments}";

		private string GetMethodIdentifier(MethodInfo baseMethod)
		{
			var methodCount = this.BaseType.GetMethods(ReflectionValues.PublicInstance)
				.Where(_ => _.Name == baseMethod.Name && !_.IsSpecialName && _.IsVirtual).Count();

			return methodCount > 1 ? baseMethod.MetadataToken.ToString(CultureInfo.CurrentCulture) : string.Empty;
		}

		private string GetTypeNameWithGenericsAndNoTextFormatting() => 
			$"{new PersistenceTypeNameGenerator(this.Namespaces).Generate(this.BaseType).Replace("<", string.Empty, StringComparison.Ordinal).Replace(">", string.Empty, StringComparison.Ordinal).Replace(", ", string.Empty, StringComparison.Ordinal)}";

		internal Type BaseType { get; }
	}
}
