using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

internal static class HashSetExtensionsTests
{
	[Test]
	public static void AddRange()
	{
		var data = new HashSet<int> { 3, 5, 7 };
		data.AddRange([4, 6]);
		Assert.That(data, Is.EquivalentTo([3, 4, 5, 6, 7]));
	}

	[Test]
	public static void AddRangeWithDuplicates()
	{
		var data = new HashSet<int> { 3, 5, 7 };
		data.AddRange([4, 7]);
		Assert.That(data, Is.EquivalentTo([3, 4, 5, 7]));
	}
}