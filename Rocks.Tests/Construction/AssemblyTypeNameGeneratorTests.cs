using NUnit.Framework;
using Rocks.Construction;
using System.Collections.Generic;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class AssemblyTypeNameGeneratorTests
	{
		[Test]
		public void GenerateWhenTypeIsGenericTypeDefinition()
		{
			var generator = new AssemblyTypeNameGenerator(new SortedSet<string>());
			Assert.AreEqual("RockAssemblyTypeNameGeneratorTestsIAmNotGeneric", 
				generator.Generate(typeof(IAmNotGeneric)));
		}

		[Test]
		public void GenerateWhenTypeIsNotGenericTypeDefinition()
		{
			var generator = new AssemblyTypeNameGenerator(new SortedSet<string>());
			Assert.AreEqual("RockAssemblyTypeNameGeneratorTestsIAmGeneric<T>", 
				generator.Generate(typeof(IAmGeneric<>)));
		}

		public interface IAmNotGeneric { }

		public interface IAmGeneric<T> { }
	}
}
