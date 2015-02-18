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
	internal sealed class RockMaker
	{
		private string mangledName = string.Format("Rock{0}", Guid.NewGuid().ToString("N"));
		private Type baseType;
		private ReadOnlyDictionary<string, Delegate> handlers;
		private SortedSet<string> namespaces;
		private RockOptions options;

		internal Type Mock { get; private set; }

		internal RockMaker(Type baseType,
			ReadOnlyDictionary<string, Delegate> handlers,
			SortedSet<string> namespaces, RockOptions options)
		{
			this.baseType = baseType;
			this.handlers = handlers;
			this.namespaces = new SortedSet<string>(namespaces);
			this.options = options;

			if (baseType.IsInterface)
			{
				this.Mock = this.MakeInterfaceMock();
			}
		}

		private Type MakeInterfaceMock()
		{
			var generatedMethods = new List<string>();

			foreach (var tMethod in this.baseType.GetMethods())
			{
				if (tMethod.ReturnType != typeof(void))
				{
					generatedMethods.Add(string.Format(Constants.CodeTemplates.FunctionMethodTemplate,
						tMethod.GetMethodDescription(namespaces), tMethod.GetArgumentNameList(),
						tMethod.ReturnType.Name));
				}
				else
				{
					generatedMethods.Add(string.Format(Constants.CodeTemplates.ActionMethodTemplate,
						tMethod.GetMethodDescription(namespaces), tMethod.GetArgumentNameList()));
				}
			}

			this.namespaces.Add(baseType.Namespace);
			this.namespaces.Add("System");
			this.namespaces.Add("System.Collections.ObjectModel");

			var classCode = string.Format(Constants.CodeTemplates.ClassTemplate,
				string.Join(Environment.NewLine,
					(from @namespace in this.namespaces
					 select "using " + @namespace + ";")),
				this.mangledName, this.baseType.Name,
				string.Join(Environment.NewLine, generatedMethods));

			var compilation = this.CreateCompilation(classCode);
			var assembly = this.CreateAssembly(compilation);
			return assembly.GetType(this.mangledName);
		}

		private Assembly CreateAssembly(CSharpCompilation compilation)
		{
			using (MemoryStream assemblyStream = new MemoryStream(),
				pdbStream = new MemoryStream())
			{
				var results = compilation.Emit(assemblyStream,
					pdbStream: pdbStream);

				if (!results.Success)
				{
					throw new RockCompilationException(results.Diagnostics);
				}

				return Assembly.Load(assemblyStream.GetBuffer());
			}
		}

		private CSharpCompilation CreateCompilation(string classCode)
		{
			var tree = SyntaxFactory.SyntaxTree(
				SyntaxFactory.ParseSyntaxTree(classCode).GetCompilationUnitRoot().NormalizeWhitespace());

			if (this.options.ShouldCreateCodeFile)
			{
				File.WriteAllText(this.mangledName + ".cs", tree.GetText().ToString());
			}

			var compilation = CSharpCompilation.Create(
				"RockQuarry.dll",
				options: new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: this.options.Level),
				syntaxTrees: new[] { tree },
				references: new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
					MetadataReference.CreateFromAssembly(this.baseType.Assembly)
				});

			return compilation;
		}
	}
}