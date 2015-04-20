using Microsoft.CodeAnalysis;
using Rocks.Construction;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	public sealed class RockAssembly
	{
		private readonly Assembly assembly;
		private readonly Options options;

		public RockAssembly(Assembly assembly)
			: this(assembly, new Options())
		{ }

		public RockAssembly(Assembly assembly, Options options)
		{
			this.assembly = assembly;
			this.options = options;
			this.Result = this.Generate();
		}

		private Assembly Generate()
		{
			var assemblyPath = Path.GetDirectoryName(this.assembly.Location);
			var assemblyName = $"{this.assembly.GetName().Name}.Rocks";
			var trees = new ConcurrentBag<SyntaxTree>();

         Parallel.ForEach(assembly.GetExportedTypes().Where(_ => string.IsNullOrWhiteSpace(_.Validate())), _ =>
				{
					var builder = new AssemblyBuilder(_, 
						new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
							new Dictionary<int, ReadOnlyCollection<HandlerInformation>>()), 
						new SortedSet<string>(), this.options);
					builder.Build();
					trees.Add(builder.Tree);
            });

			var referencedAssemblies = this.assembly.GetReferencedAssemblies().Select(_ => Assembly.Load(_)).ToList();
			referencedAssemblies.Add(this.assembly);

         var compiler = new AssemblyCompiler(trees, this.options.Level, assemblyName, 
				referencedAssemblies.AsReadOnly(), assemblyPath);
			compiler.Compile();
			return compiler.Result;
      }

		public Assembly Result { get; }
	}
}
