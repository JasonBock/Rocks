using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public enum DefaultValue { One, Two, Three }

public static class ObjectExtensionsTests
{
	[TestCase("b", "\"b\"")]
	[TestCase(true, "true")]
	[TestCase(false, "false")]
	[TestCase(byte.MaxValue, "byte.MaxValue")]
	[TestCase(byte.MinValue, "byte.MinValue")]
	[TestCase((byte)22, "22")]
	[TestCase(sbyte.MaxValue, "sbyte.MaxValue")]
	[TestCase(sbyte.MinValue, "sbyte.MinValue")]
	[TestCase((sbyte)22, "22")]
	[TestCase(char.MaxValue, "char.MaxValue")]
	[TestCase(char.MinValue, "char.MinValue")]
	[TestCase('A', "A")]
	[TestCase(double.PositiveInfinity, "double.PositiveInfinity")]
	[TestCase(double.NegativeInfinity, "double.NegativeInfinity")]
	[TestCase(double.MaxValue, "double.MaxValue")]
	[TestCase(double.MinValue, "double.MinValue")]
	[TestCase(double.NaN, "double.NaN")]
	[TestCase(double.Epsilon, "double.Epsilon")]
	[TestCase((double)22.473, "22.473")]
	[TestCase(float.PositiveInfinity, "float.PositiveInfinity")]
	[TestCase(float.NegativeInfinity, "float.NegativeInfinity")]
	[TestCase(float.MaxValue, "float.MaxValue")]
	[TestCase(float.MinValue, "float.MinValue")]
	[TestCase(float.NaN, "float.NaN")]
	[TestCase(float.Epsilon, "float.Epsilon")]
	[TestCase((float)22.473, "22.473")]
	[TestCase(int.MaxValue, "int.MaxValue")]
	[TestCase(int.MinValue, "int.MinValue")]
	[TestCase((int)22, "22")]
	[TestCase(uint.MaxValue, "uint.MaxValue")]
	[TestCase(uint.MinValue, "uint.MinValue")]
	[TestCase((uint)22, "22")]
	[TestCase(long.MaxValue, "long.MaxValue")]
	[TestCase(long.MinValue, "long.MinValue")]
	[TestCase((long)22, "22")]
	[TestCase(ulong.MaxValue, "ulong.MaxValue")]
	[TestCase(ulong.MinValue, "ulong.MinValue")]
	[TestCase((ulong)22, "22")]
	[TestCase(short.MaxValue, "short.MaxValue")]
	[TestCase(short.MinValue, "short.MinValue")]
	[TestCase((short)22, "22")]
	[TestCase(ushort.MaxValue, "ushort.MaxValue")]
	[TestCase(ushort.MinValue, "ushort.MinValue")]
	[TestCase((ushort)22, "22")]
	[TestCase(DefaultValue.Two, "global::Rocks.Tests.Extensions.DefaultValue.Two")]
	[TestCase(null, "null")]
	[TestCase(null, "default", true)]
	public static void GetDefaultValue(object value, string expectedResult, bool isValueType = false) =>
		Assert.That(value.GetDefaultValue(isValueType), Is.EqualTo(expectedResult));

	[Test]
	public static void GetDecimalDefaultValue() =>
		Assert.Multiple(() =>
		{
			Assert.That(decimal.MaxValue.GetDefaultValue(), Is.EqualTo("decimal.MaxValue"));
			Assert.That(decimal.MinusOne.GetDefaultValue(), Is.EqualTo("decimal.MinusOne"));
			Assert.That(decimal.MinValue.GetDefaultValue(), Is.EqualTo("decimal.MinValue"));
			Assert.That(decimal.One.GetDefaultValue(), Is.EqualTo("decimal.One"));
			Assert.That(decimal.Zero.GetDefaultValue(), Is.EqualTo("decimal.Zero"));
			Assert.That(((decimal)22).GetDefaultValue(), Is.EqualTo("22"));
		});
}