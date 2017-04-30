using NUnit.Framework;
using System.Collections.Generic;
using static Rocks.Extensions.IDictionaryOfTKeyTValueExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class IDictionaryOfTKeyTValueExtensionsTests
	{
		[Test]
		public void Add()
		{
			var keyedValues = new Dictionary<string, Value>();
			keyedValues.AddOrUpdate("a", () => new Value { Data = "b" }, null);

			Assert.That(keyedValues["a"].Data, Is.EqualTo("b"));
		}

		[Test]
		public void Update()
		{
			var keyedValues = new Dictionary<string, Value> { { "a", new Value { Data = "b" } } };
			keyedValues.AddOrUpdate("a", null, _ => _.Data = "c");

			Assert.That(keyedValues["a"].Data, Is.EqualTo("c"));
		}
	}

	public class Value
	{
		public string Data { get; set; }
	}
}
