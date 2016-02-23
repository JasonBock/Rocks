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

			Assert.AreSame(result, results.Result, nameof(results.Result));
			Assert.AreEqual(requires, results.RequiresObsoleteSuppression,
				nameof(results.RequiresObsoleteSuppression));
			Assert.AreEqual(isUnsafe, results.IsUnsafe,
				nameof(results.IsUnsafe));
		}
	}
}
