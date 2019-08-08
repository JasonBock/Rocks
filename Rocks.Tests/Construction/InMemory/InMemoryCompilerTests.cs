using NUnit.Framework;
using Rocks.Construction.InMemory;
using Rocks.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Rocks.Tests.Construction.InMemory
{
	public static class InMemoryCompilerTests
	{
		[Test]
		public static void Compile()
		{
			var baseType = typeof(IBuilderTest);
			var handlers = new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
				new Dictionary<int, ReadOnlyCollection<HandlerInformation>>());
			var namespaces = new SortedSet<string> { baseType.Namespace! };
			var options = new RockOptions();

			var builder = new InMemoryBuilder(baseType, handlers, namespaces, options, false);

			var trees = new[] { builder.Tree };
			var compiler = new InMemoryCompiler(trees, options.Optimization, 
				new List<Assembly> { baseType.Assembly }.AsReadOnly(), builder.IsUnsafe,
				options.AllowWarnings);
			var assembly = compiler.Compile();

			Assert.That(compiler.Optimization, Is.EqualTo(options.Optimization), nameof(compiler.Optimization));
			Assert.That(compiler.Trees, Is.SameAs(trees), nameof(compiler.Trees));
			Assert.That(assembly, Is.Not.Null, nameof(compiler.Compile));
			Assert.That(
				(from type in assembly.GetTypes()
				where baseType.IsAssignableFrom(type)
				select type).Single(), Is.Not.Null);
		}
	}

	public interface ICompilerTest { }
}
