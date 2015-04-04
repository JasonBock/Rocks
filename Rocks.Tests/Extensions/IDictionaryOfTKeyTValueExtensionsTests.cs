using NUnit.Framework;
using System.Collections.Generic;
using Rocks.Extensions;

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

			Assert.AreEqual("b", keyedValues["a"].Data);
		}

		[Test]
		public void Update()
		{
			var keyedValues = new Dictionary<string, Value> { { "a", new Value { Data = "b" } } };
			keyedValues.AddOrUpdate("a", null, _ => _.Data = "c");

			Assert.AreEqual("c", keyedValues["a"].Data);
		}
	}

	public class Value
	{
		public string Data { get; set; }
	}
}
