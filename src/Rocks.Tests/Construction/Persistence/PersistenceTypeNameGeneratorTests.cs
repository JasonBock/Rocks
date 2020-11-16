using NUnit.Framework;
using Rocks.Construction.Persistence;
using System.Collections.Generic;

namespace Rocks.Tests.Construction
{
	public static class PersistenceTypeNameGeneratorTests
	{
		[Test]
		public static void GenerateWhenTypeIsGenericTypeDefinition()
		{
			var generator = new PersistenceTypeNameGenerator(new SortedSet<string>());
			Assert.That(generator.Generate(typeof(IAmNotGeneric)), 
				Is.EqualTo($"Rock{nameof(PersistenceTypeNameGeneratorTests)}IAmNotGeneric"));
		}

		[Test]
		public static void GenerateWhenTypeIsNotGenericTypeDefinition()
		{
			var generator = new PersistenceTypeNameGenerator(new SortedSet<string>());
			Assert.That(generator.Generate(typeof(IAmGeneric<>)),
				Is.EqualTo($"Rock{nameof(PersistenceTypeNameGeneratorTests)}IAmGeneric<T>"));
		}

		public interface IAmNotGeneric { }

		public interface IAmGeneric<T> { }
	}
}
