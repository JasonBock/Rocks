using NUnit.Framework;
using Rocks.Extensions;
using System;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class GenericArgumentsResultTests
	{
		[Test]
		public void Create()
		{
			var arguments = Guid.NewGuid().ToString("N");
			var constraints = Guid.NewGuid().ToString("N");

			var result = new GenericArgumentsResult(arguments, constraints);

			Assert.AreEqual(arguments, result.Arguments, nameof(result.Arguments));
			Assert.AreEqual(constraints, result.Constraints, nameof(result.Constraints));
		}
	}
}
