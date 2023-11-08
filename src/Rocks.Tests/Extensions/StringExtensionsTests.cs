using NUnit.Framework;
using Rocks.Extensions;
using System.Globalization;
using System.Numerics;

namespace Rocks.Tests.Extensions;

public static class StringExtensionsTests
{
	[TestCase("hello", "441101220755863908001377779840592906501715588266")]
	[TestCase("lots of data", "227842025459979035662813979956466430472270772150")]
	[TestCase("", "51555898148515146965744933031213250625354807770")]
	[TestCase(null, "51555898148515146965744933031213250625354807770")]
	public static void CreateHash(string? input, string expectedData) => 
		Assert.That(input.GetHash(), Is.EqualTo(BigInteger.Parse(expectedData, CultureInfo.InvariantCulture)));
}