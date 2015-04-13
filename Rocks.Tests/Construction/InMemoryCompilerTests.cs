using NUnit.Framework;
using Rocks.Construction;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class InMemoryCompilerTests
	{
		[Test]
		public void Compile()
		{
			var baseType = typeof(IBuilderTest);
			var handlers = new ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>>(
				new Dictionary<string, ReadOnlyCollection<HandlerInformation>>());
			var namespaces = new SortedSet<string> { baseType.Namespace };
			var options = new Options();

			var builder = new InMemoryBuilder(baseType, handlers, namespaces, options);
			builder.Build();

			var trees = new[] { builder.Tree };
			var compiler = new InMemoryCompiler(trees, options.Level, 
				new List<Assembly> { baseType.Assembly }.AsReadOnly());
			compiler.Compile();

			Assert.AreEqual(options.Level, compiler.Level, nameof(compiler.Level));
			Assert.AreSame(trees, compiler.Trees, nameof(compiler.Trees));
			Assert.IsNotNull(compiler.Result, nameof(compiler.Result));
			Assert.IsNotNull(
				(from type in compiler.Result.GetTypes()
				where baseType.IsAssignableFrom(type)
				select type).Single());
		}
	}

	public interface ICompilerTest { }
}
