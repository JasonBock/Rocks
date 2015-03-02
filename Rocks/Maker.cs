using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks
{
	internal sealed class Maker
	{
		internal Type Mock { get; private set; }

		internal Maker(Type baseType,
			ReadOnlyDictionary<string, HandlerInformation> handlers,
			SortedSet<string> namespaces, Options options)
		{
			var builder = new Builder(baseType, handlers, namespaces, options);
			var compiler = new Compiler(baseType, new List<SyntaxTree> { builder.Tree }, options);
			this.Mock = compiler.Assembly.GetType(builder.TypeName);
		}
	}
}