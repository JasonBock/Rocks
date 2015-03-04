using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class CompilerTests
	{
		[Test]
		public void Compile()
		{
			var baseType = typeof(IBuilderTest);
			var handlers = new ReadOnlyDictionary<string, HandlerInformation>(new Dictionary<string, HandlerInformation>());
			var namespaces = new SortedSet<string> { baseType.Namespace };
			var options = new Options();

			var builder = new Builder(baseType, handlers, namespaces, options);

			var trees = new[] { builder.Tree };
			var compiler = new Compiler(baseType, trees, options);

			Assert.AreSame(baseType, compiler.BaseType, nameof(compiler.BaseType));
			Assert.AreSame(options, compiler.Options, nameof(compiler.Options));
			Assert.AreSame(trees, compiler.Trees, nameof(compiler.Trees));
			Assert.IsNotNull(compiler.Assembly, nameof(compiler.Assembly));
			Assert.IsNotNull(
				(from type in compiler.Assembly.GetTypes()
				where baseType.IsAssignableFrom(type)
				select type).Single());
		}
	}

	public interface ICompilerTest { }
}
