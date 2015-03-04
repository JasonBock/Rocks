using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class BuilderTests
	{
		[Test]
		public void Build()
		{
			var baseType = typeof(IBuilderTest);
			var handlers = new ReadOnlyDictionary<string, HandlerInformation>(new Dictionary<string, HandlerInformation>());
			var namespaces = new SortedSet<string> { baseType.Namespace };
			var options = new Options();

			var builder = new Builder(baseType, handlers, namespaces, options);

			Assert.AreSame(baseType, builder.BaseType, nameof(builder.BaseType));
			Assert.AreSame(handlers, builder.Handlers, nameof(builder.Handlers));
			Assert.AreSame(namespaces, builder.Namespaces, nameof(builder.Namespaces));
			Assert.AreEqual(5, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("Rocks"), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains("Rocks.Exceptions"), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains("Rocks.Tests"), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces));
			Assert.IsTrue(namespaces.Contains("System.Collections.ObjectModel"), nameof(namespaces));
			Assert.AreSame(options, builder.Options, nameof(builder.Options));
			Assert.IsNotNull(builder.Tree, nameof(builder.Tree));
			Assert.IsTrue(!string.IsNullOrWhiteSpace(builder.TypeName), nameof(builder.TypeName));
		}
	}

	public interface IBuilderTest { }
}
