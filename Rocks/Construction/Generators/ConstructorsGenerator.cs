using Rocks.Templates;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.ConstructorInfoExtensions;
using static Rocks.Extensions.MethodBaseExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction.Generators
{
	internal static class ConstructorsGenerator
	{
		internal static GenerateResults Generate(Type baseType, SortedSet<string> namespaces,
			NameGenerator generator, string constructorName)
		{
			var isUnsafe = false;
			var requiresObsoleteSuppression = false;

			var generatedConstructors = new List<string>();

			if (baseType.IsInterface)
			{
				generatedConstructors.Add(ConstructorTemplates.GetConstructor(
					constructorName, string.Empty, string.Empty));
			}
			else
			{
				foreach (var constructor in baseType.GetMockableConstructors(generator))
				{
					var baseConstructor = constructor.Value;

					var parameters = baseConstructor.GetParameters(namespaces);

					if (!string.IsNullOrWhiteSpace(parameters))
					{
						parameters = $", {parameters}";
					}

					generatedConstructors.Add(ConstructorTemplates.GetConstructor(
						constructorName, baseConstructor.GetArgumentNameList(), parameters));
					isUnsafe |= baseConstructor.IsUnsafeToMock();
					requiresObsoleteSuppression |= baseConstructor.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return new GenerateResults(string.Join(Environment.NewLine, generatedConstructors),
				requiresObsoleteSuppression, isUnsafe);
		}
	}
}
