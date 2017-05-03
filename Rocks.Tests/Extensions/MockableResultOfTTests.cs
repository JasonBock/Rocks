using NUnit.Framework;
using Rocks.Extensions;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MockableResultOfTTests
	{
		[Test]
		public void Create()
		{
			var method = this.GetType().GetMethod(nameof(Create));
			var requires = RequiresExplicitInterfaceImplementation.Yes;

			var result = new MockableResult<MethodBase>(method, requires);
			Assert.That(result.Value, Is.EqualTo(method), nameof(result.Value));
			Assert.That(result.RequiresExplicitInterfaceImplementation, Is.EqualTo(requires),
				nameof(result.RequiresExplicitInterfaceImplementation));
		}
	}
}
