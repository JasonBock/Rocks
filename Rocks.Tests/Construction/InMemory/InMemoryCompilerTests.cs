using NUnit.Framework;
using Rocks.Construction.InMemory;
using Rocks.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Rocks.Tests.Construction.InMemory
{
	[TestFixture]
	public sealed class InMemoryCompilerTests
	{
		[Test]
		public void Compile()
		{
			var baseType = typeof(IBuilderTest);
			var handlers = new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
				new Dictionary<int, ReadOnlyCollection<HandlerInformation>>());
			var namespaces = new SortedSet<string> { baseType.Namespace };
			var options = new RockOptions();

			var builder = new InMemoryBuilder(baseType, handlers, namespaces, options, false);
			builder.Build();

			var trees = new[] { builder.Tree };
			var compiler = new InMemoryCompiler(trees, options.Optimization, 
				new List<Assembly> { baseType.Assembly }.AsReadOnly(), builder.IsUnsafe,
				options.AllowWarnings);
			compiler.Compile();

			Assert.That(compiler.Optimization, Is.EqualTo(options.Optimization), nameof(compiler.Optimization));
			Assert.That(compiler.Trees, Is.SameAs(trees), nameof(compiler.Trees));
			Assert.That(compiler.Result, Is.Not.Null, nameof(compiler.Result));
			Assert.That(
				(from type in compiler.Result.GetTypes()
				where baseType.IsAssignableFrom(type)
				select type).Single(), Is.Not.Null);
		}
	}

	public interface ICompilerTest { }
}
