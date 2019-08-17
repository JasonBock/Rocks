using Microsoft.CodeAnalysis;
using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryMaker
	{
		internal Type Mock { get; }

		internal InMemoryMaker(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, RockOptions options, bool isMake)
		{
			var builder = new InMemoryBuilder(baseType, handlers, namespaces, options, isMake);

			var compiler = new InMemoryCompiler(new List<SyntaxTree> { builder.Tree }, options.Optimization,
				new List<Assembly> { baseType.Assembly }.AsReadOnly(), builder.IsUnsafe, options.AllowWarning);
			var assembly = compiler.Compile();

			this.Mock = assembly.GetType($"{baseType.Namespace}.{builder.TypeName}");
		}
	}
}