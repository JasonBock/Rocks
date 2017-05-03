using Microsoft.CodeAnalysis;
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
		private readonly RockOptions options;

		public RockAssembly(Assembly assembly)
			: this(assembly, new RockOptions())
		{ }

		public RockAssembly(Assembly assembly, RockOptions options)
			: base()
		{
			this.assembly = assembly;
			this.options = options;
			this.Result = this.Generate();
		}

		private Assembly Generate()
		{
			var assemblyPath = Path.GetDirectoryName(this.assembly.Location);
			var trees = new ConcurrentBag<SyntaxTree>();
			var allowUnsafe = false;

			Parallel.ForEach(this.assembly.GetExportedTypes()
#if !NETCOREAPP1_1
				.Where(_ => string.IsNullOrWhiteSpace(_.Validate(this.options.Serialization, new PersistenceNameGenerator(_))) && !typeof(Array).IsAssignableFrom(_) &&
#else
				.Where(_ => string.IsNullOrWhiteSpace(_.Validate(new PersistenceNameGenerator(_))) && !typeof(Array).IsAssignableFrom(_) &&
#endif
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
				referencedAssemblies.AsReadOnly(),
				this.options.CodeFileDirectory, allowUnsafe, this.options.AllowWarnings);
			compiler.Compile();
			return compiler.Result;
		}

		public Assembly Result { get; }
	}
}
