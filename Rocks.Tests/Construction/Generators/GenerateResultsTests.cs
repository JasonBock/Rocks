using NUnit.Framework;
using Rocks.Construction.Generators;
using System;

namespace Rocks.Tests.Construction.Generators
{
	[TestFixture]
	public sealed class GenerateResultsTests
	{
		[Test]
		public void Create()
		{
			var result = Guid.NewGuid().ToString();
			var requires = true;
			var isUnsafe = true;

			var results = new GenerateResults(result, requires, isUnsafe);

			Assert.That(results.Result, Is.SameAs(result), nameof(results.Result));
			Assert.That(results.RequiresObsoleteSuppression, Is.EqualTo(requires),
				nameof(results.RequiresObsoleteSuppression));
			Assert.That(results.IsUnsafe, Is.EqualTo(isUnsafe),
				nameof(results.IsUnsafe));
		}
	}
}
