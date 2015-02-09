using System;
using System.IO;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Extensions;

namespace Rocks
{
	internal static class RockMaker
	{
		internal static T Make<T>(Dictionary<string, Delegate> handlers)
			where T : class
		{
			var x = handlers.ToImmutableDictionary();

			if (typeof(T).IsInterface)
			{
				return RockMaker.MakeInterfaceMock<T>();
			}

			return default(T);
		}

		private static T MakeInterfaceMock<T>()
			where T : class
		{
			var tType = typeof(T);
			var rockMangledName = string.Format("Rock{0}", Guid.NewGuid().ToString("N"));

			var generatedMethods = new List<string>();

			foreach (var tMethod in tType.GetMethods())
			{
				if (tMethod.ReturnType != typeof(void))
				{
					generatedMethods.Add(string.Format(Constants.CodeTemplates.FunctionMethodTemplate,
						tMethod.GetMethodDescription(), RockMaker.GetArgumentNameList(tMethod), tMethod.ReturnType.FullName));
				}
				else
				{
					generatedMethods.Add(string.Format(Constants.CodeTemplates.ActionMethodTemplate,
						tMethod.GetMethodDescription(), RockMaker.GetArgumentNameList(tMethod)));
				}
			}

			var classCode = string.Format(Constants.CodeTemplates.ClassTemplate,
				rockMangledName, tType.FullName,
				string.Join(Environment.NewLine, generatedMethods));

			// Now, compile with assembly references from T, and ImmutableDictionary, and mscorlib
			var tree = SyntaxFactory.ParseSyntaxTree(classCode);
			var compilation = CSharpCompilation.Create(
				"RockQuarry.dll",
				options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
				syntaxTrees: new[] { tree },
				references: new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
					MetadataReference.CreateFromAssembly(typeof(ImmutableArray).Assembly),
					MetadataReference.CreateFromAssembly(tType.Assembly)
				});

			// New up
			Assembly assembly;
			using (var stream = new MemoryStream())
			{
				var results = compilation.Emit(stream);
				assembly = Assembly.Load(stream.GetBuffer());
			}

			// And return.
			return Activator.CreateInstance(assembly.GetType(rockMangledName)) as T;
      }

		private static string GetArgumentNameList(MethodInfo method)
		{
			return string.Join(", ", method.GetParameters().Select(_ => _.Name));
		}
	}
}
