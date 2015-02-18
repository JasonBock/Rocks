using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocks
{
	internal static class RockMaker
	{
		internal static Type Make(Type baseType, 
			ReadOnlyDictionary<string, Delegate> handlers,
         SortedSet<string> namespaces)
		{
			if (baseType.IsInterface)
			{
				return RockMaker.MakeInterfaceMock(baseType, handlers, namespaces);
			}

			return null;
		}

		private static Type MakeInterfaceMock(Type baseType,
			ReadOnlyDictionary<string, Delegate> handlers,
			SortedSet<string> namespaces)
		{
			var rockMangledName = string.Format("Rock{0}", Guid.NewGuid().ToString("N"));

			var generatedMethods = new List<string>();

			foreach (var tMethod in baseType.GetMethods())
			{
				if (tMethod.ReturnType != typeof(void))
				{
					generatedMethods.Add(string.Format(Constants.CodeTemplates.FunctionMethodTemplate,
						tMethod.GetMethodDescription(namespaces), tMethod.GetArgumentNameList(), 
						tMethod.ReturnType.FullName));
				}
				else
				{
					generatedMethods.Add(string.Format(Constants.CodeTemplates.ActionMethodTemplate,
						tMethod.GetMethodDescription(namespaces), tMethod.GetArgumentNameList()));
				}
			}

			namespaces.Add(baseType.Namespace);
			namespaces.Add("System");
			namespaces.Add("System.Collections.ObjectModel");

			var classCode = string.Format(Constants.CodeTemplates.ClassTemplate,
				string.Join(Environment.NewLine, 
					(from @namespace in namespaces
					select "using " + @namespace + ";")),
				rockMangledName, baseType.Name,
				string.Join(Environment.NewLine, generatedMethods));

			// Now, compile with assembly references from object and T.
			var tree = SyntaxFactory.ParseSyntaxTree(classCode);
			var compilation = CSharpCompilation.Create(
				"RockQuarry.dll",
				options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
				syntaxTrees: new[] { tree },
				references: new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
					MetadataReference.CreateFromAssembly(baseType.Assembly)
				});

			// New up
			Assembly assembly;
			using (var stream = new MemoryStream())
			{
				var results = compilation.Emit(stream);
				assembly = Assembly.Load(stream.GetBuffer());
			}

			// And return.
			return assembly.GetType(rockMangledName);
      }
	}
}
