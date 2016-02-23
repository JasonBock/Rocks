using NUnit.Framework;
using Rocks.Construction.Persistence;
using System.Collections.Generic;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class PersistenceTypeNameGeneratorTests
	{
		[Test]
		public void GenerateWhenTypeIsGenericTypeDefinition()
		{
			var generator = new PersistenceTypeNameGenerator(new SortedSet<string>());
			Assert.AreEqual($"Rock{nameof(PersistenceTypeNameGeneratorTests)}IAmNotGeneric", 
            generator.Generate(typeof(IAmNotGeneric)));
		}

		[Test]
		public void GenerateWhenTypeIsNotGenericTypeDefinition()
		{
			var generator = new PersistenceTypeNameGenerator(new SortedSet<string>());
			Assert.AreEqual($"Rock{nameof(PersistenceTypeNameGeneratorTests)}IAmGeneric<T>", 
				generator.Generate(typeof(IAmGeneric<>)));
		}

		public interface IAmNotGeneric { }

		public interface IAmGeneric<T> { }
	}
}
