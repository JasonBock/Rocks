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

			Assert.That(result.Arguments, Is.EqualTo(arguments), nameof(result.Arguments));
			Assert.That(result.Constraints, Is.EqualTo(constraints), nameof(result.Constraints));
		}
	}
}
