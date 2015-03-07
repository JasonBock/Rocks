using NUnit.Framework;
using Rocks.Construction;
using Rocks.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class InMemoryBuilderTests
	{
		[Test]
		public void Build()
		{
			var baseType = typeof(IBuilderTest);
			var handlers = new ReadOnlyDictionary<string, HandlerInformation>(new Dictionary<string, HandlerInformation>());
			var namespaces = new SortedSet<string> { baseType.Namespace };
			var options = new Options();

			var builder = new InMemoryBuilder(baseType, handlers, namespaces, options.ShouldCreateCodeFile);
			builder.Build();

			Assert.AreSame(baseType, builder.BaseType, nameof(builder.BaseType));
			Assert.AreSame(handlers, builder.Handlers, nameof(builder.Handlers));
			Assert.AreSame(namespaces, builder.Namespaces, nameof(builder.Namespaces));
			Assert.AreEqual(5, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(Rock).Namespace), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains(typeof(CompilationException).Namespace), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains(typeof(IBuilderTest).Namespace), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains(typeof(ReadOnlyDictionary<,>).Namespace), nameof(namespaces));
			Assert.AreEqual(options.ShouldCreateCodeFile, builder.ShouldCreateCodeFile, nameof(builder.ShouldCreateCodeFile));
			Assert.IsNotNull(builder.Tree, nameof(builder.Tree));
			Assert.IsTrue(!string.IsNullOrWhiteSpace(builder.TypeName), nameof(builder.TypeName));
		}
	}

	public interface IBuilderTest { }
}
