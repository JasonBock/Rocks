using NUnit.Framework;
using Rocks.Extensions;
using System;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsMatchTests
	{
		[Test]
		public void MatchWhenExact()
		{
			Assert.AreEqual(MethodMatch.Exact, typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithExact).GetMethod(nameof(OtherMatchWithExact.GetString))));
		}

		[Test]
		public void MatchWhenDifferentName()
		{
			Assert.AreEqual(MethodMatch.None, typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentName).GetMethod(nameof(OtherMatchWithDifferentName.GetAString))));
		}

		[Test]
		public void MatchWhenDifferentArgumentCount()
		{
			Assert.AreEqual(MethodMatch.None, typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentArgumentCount).GetMethod(nameof(OtherMatchWithDifferentArgumentCount.GetString))));
		}

		[Test]
		public void MatchWhenDifferentArgumentType()
		{
			Assert.AreEqual(MethodMatch.None, typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentArgumentType).GetMethod(nameof(OtherMatchWithDifferentArgumentType.GetString))));
		}

		[Test]
		public void MatchWhenDifferentModifier()
		{
			Assert.AreEqual(MethodMatch.None, typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentModifier).GetMethod(nameof(OtherMatchWithDifferentModifier.GetString))));
		}

		[Test]
		public void MatchWhenDifferentReturnType()
		{
			Assert.AreEqual(MethodMatch.DifferByReturnTypeOnly, typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentReturnType).GetMethod(nameof(OtherMatchWithDifferentReturnType.GetString))));
		}
	}

	public class TargetMatch
	{
		public string GetString(int a) { return null; }
	}

	public class OtherMatchWithExact
	{
		public string GetString(int a) { return null; }
	}

	public class OtherMatchWithDifferentArgumentCount
	{
		public string GetString(int a, Guid b) { return null; }
	}

	public class OtherMatchWithDifferentName
	{
		public string GetAString(int a) { return null; }
	}

	public class OtherMatchWithDifferentArgumentType
	{
		public string GetString(Guid a) { return null; }
	}

	public class OtherMatchWithDifferentModifier
	{
		public string GetString(ref int a) { return null; }
	}

	public class OtherMatchWithDifferentReturnType
	{
		public Guid GetString(int a) { return default(Guid); }
	}
}
