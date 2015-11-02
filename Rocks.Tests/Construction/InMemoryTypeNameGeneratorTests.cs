using NUnit.Framework;
using Rocks.Construction;
using System;
using System.Collections.Generic;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class InMemoryTypeNameGeneratorTests
	{
		[Test]
		public void GenerateWhenTypeIsGenericTypeDefinition()
		{
			var generator = new InMemoryTypeNameGenerator(new SortedSet<string>());
			var name = generator.Generate(typeof(IAmNotGeneric));

         Assert.IsTrue(name.StartsWith("Rock"));

			var restOfName = name.Substring(4);

			Assert.DoesNotThrow(() => Guid.Parse(restOfName));
		}

		[Test]
		public void GenerateWhenTypeIsNotGenericTypeDefinition()
		{
			var generator = new InMemoryTypeNameGenerator(new SortedSet<string>());

			var name = generator.Generate(typeof(IAmGeneric<>));

			Assert.IsTrue(name.StartsWith("Rock"));
			Assert.IsTrue(name.EndsWith("<T>"));

			var restOfName = name.Substring(4);
			restOfName = restOfName.Substring(0, restOfName.Length - 3);

			Assert.DoesNotThrow(() => Guid.Parse(restOfName));
		}

		public interface IAmNotGeneric { }

		public interface IAmGeneric<T> { }
	}
}
