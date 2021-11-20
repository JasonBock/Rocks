using NUnit.Framework;
using static Rocks.Extensions.IDictionaryOfTKeyTValueExtensions;

namespace Rocks.Tests.Extensions;

public static class IDictionaryOfTKeyTValueExtensionsTests
{
	[Test]
	public static void Add()
	{
		var keyedValues = new Dictionary<string, Value>();
		keyedValues.AddOrUpdate("a", () => new Value { Data = "b" }, null);

		Assert.That(keyedValues["a"].Data, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenAddIsNull()
	{
		var keyedValues = new Dictionary<string, Value>();
		Assert.That(() => keyedValues.AddOrUpdate("a", null, null), 
			Throws.TypeOf<ArgumentNullException>().With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("add"));
	}

	[Test]
	public static void Update()
	{
		var keyedValues = new Dictionary<string, Value> { { "a", new Value { Data = "b" } } };
		keyedValues.AddOrUpdate("a", null, _ => _.Data = "c");

		Assert.That(keyedValues["a"].Data, Is.EqualTo("c"));
	}

	[Test]
	public static void UpdateWhenUpdateIsNull()
	{
		var keyedValues = new Dictionary<string, Value> { { "a", new Value { Data = "b" } } };
		Assert.That(() => keyedValues.AddOrUpdate("a", null, null),
			Throws.TypeOf<ArgumentNullException>().With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("update"));
	}
}

public class Value
{
	public string? Data { get; set; }
}