using Microsoft.CodeAnalysis;
using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Rocks.Construction
{
	internal sealed class InMemoryMaker
	{
		internal Type Mock { get; private set; }

		internal InMemoryMaker(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, RockOptions options, bool isMake)
		{
			var builder = new InMemoryBuilder(baseType, handlers, namespaces, options, isMake);
			builder.Build();

			var compiler = new InMemoryCompiler(new List<SyntaxTree> { builder.Tree }, options.Optimization,
				new List<Assembly> { baseType.Assembly }.AsReadOnly(), builder.IsUnsafe, options.AllowWarnings);
			compiler.Compile();

			this.Mock = compiler.Result.GetType($"{baseType.Namespace}.{builder.TypeName}");
		}
	}
}