using Microsoft.CodeAnalysis;
using Rocks.Construction;
using Rocks.Construction.Persistence;
using Rocks.Options;
using System;
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
		private readonly string currentDirectory;
		private readonly RockOptions options;

		public RockAssembly(Assembly assembly)
			: this(assembly, new RockOptions())
		{ }

		public RockAssembly(Assembly assembly, RockOptions options)
			: this(assembly, options, Environment.CurrentDirectory)
		{ }

		public RockAssembly(Assembly assembly, string currentDirectory)
			: this(assembly, new RockOptions(), currentDirectory)
		{ }

		public RockAssembly(Assembly assembly, RockOptions options, string currentDirectory)
		{
			this.assembly = assembly;
			this.options = options;
			this.currentDirectory = currentDirectory;
			this.Result = this.Generate();
		}

		private Assembly Generate()
		{
			var assemblyPath = Path.GetDirectoryName(this.assembly.Location);
			var trees = new ConcurrentBag<SyntaxTree>();
			var allowUnsafe = false;

         Parallel.ForEach(assembly.GetExportedTypes()
				.Where(_ => string.IsNullOrWhiteSpace(_.Validate(this.options.Serialization, new PersistenceNameGenerator(_))) && !typeof(Array).IsAssignableFrom(_) &&
					!typeof(Enum).IsAssignableFrom(_) && !typeof(ValueType).IsAssignableFrom(_) && 
					!typeof(Delegate).IsAssignableFrom(_)), _ =>
				{
					var builder = new PersistenceBuilder(_, 
						new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
							new Dictionary<int, ReadOnlyCollection<HandlerInformation>>()), 
						new SortedSet<string>(), this.options);
					builder.Build();
					trees.Add(builder.Tree);
					allowUnsafe |= builder.IsUnsafe;
            });

			var referencedAssemblies = this.assembly.GetReferencedAssemblies().Select(_ => Assembly.Load(_)).ToList();
			referencedAssemblies.Add(this.assembly);

         var compiler = new PersistenceCompiler(trees, this.options.Optimization, 
				new PersistenceNameGenerator(this.assembly).AssemblyName, 
				referencedAssemblies.AsReadOnly(), currentDirectory, allowUnsafe, this.options.AllowWarnings);
			compiler.Compile();
			return compiler.Result;
      }

		public Assembly Result { get; }
	}
}
