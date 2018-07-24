using NUnit.Framework;
using Rocks.Construction.InMemory;
using System;
using System.Collections.Generic;

namespace Rocks.Tests.Construction.InMemory
{
	[TestFixture]
	public sealed class InMemoryTypeNameGeneratorTests
	{
		[Test]
		public void GenerateWhenTypeIsGenericTypeDefinition()
		{
			var generator = new InMemoryTypeNameGenerator(new SortedSet<string>());
			var name = generator.Generate(typeof(IAmNotGeneric));

			Assert.That(name.StartsWith("Rock"), Is.True);

			var restOfName = name.Substring(4);

			Assert.That(() => int.Parse(restOfName), Is.EqualTo(typeof(IAmNotGeneric).MetadataToken));
		}

		[Test]
		public void GenerateWhenTypeIsNotGenericTypeDefinition()
		{
			var generator = new InMemoryTypeNameGenerator(new SortedSet<string>());

			var name = generator.Generate(typeof(IAmGeneric<>));

			Assert.That(name.StartsWith("Rock"), Is.True);
			Assert.That(name.EndsWith("<T>"), Is.True);

			var restOfName = name.Substring(4);
			restOfName = restOfName.Substring(0, restOfName.Length - 3);

			Assert.That(() => int.Parse(restOfName), Is.EqualTo(typeof(IAmGeneric<>).MetadataToken));
		}

		public interface IAmNotGeneric { }

		public interface IAmGeneric<T> { }
	}
}
