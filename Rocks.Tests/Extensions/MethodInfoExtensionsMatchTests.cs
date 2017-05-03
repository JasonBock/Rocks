using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsMatchTests
	{
		[Test]
		public void MatchWhenExact() =>
			Assert.That(typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithExact).GetMethod(nameof(OtherMatchWithExact.GetString))), Is.EqualTo(MethodMatch.Exact));

		[Test]
		public void MatchWhenDifferentName() =>
			Assert.That(typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentName).GetMethod(nameof(OtherMatchWithDifferentName.GetAString))), Is.EqualTo(MethodMatch.None));

		[Test]
		public void MatchWhenDifferentArgumentCount() =>
			Assert.That(typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentArgumentCount).GetMethod(nameof(OtherMatchWithDifferentArgumentCount.GetString))), Is.EqualTo(MethodMatch.None));

		[Test]
		public void MatchWhenDifferentArgumentType() =>
			Assert.That(typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentArgumentType).GetMethod(nameof(OtherMatchWithDifferentArgumentType.GetString))), Is.EqualTo(MethodMatch.None));

		[Test]
		public void MatchWhenDifferentModifier() =>
			Assert.That(typeof(TargetMatch).GetTypeInfo().GetMethod(
				nameof(TargetMatch.GetString)).Match(
					typeof(OtherMatchWithDifferentModifier).GetMethod(nameof(OtherMatchWithDifferentModifier.GetString))), 
				Is.EqualTo(MethodMatch.None));

		[Test]
		public void MatchWhenDifferentReturnType() =>
			Assert.That(typeof(TargetMatch).GetMethod(nameof(TargetMatch.GetString)).Match(
				typeof(OtherMatchWithDifferentReturnType).GetTypeInfo().GetMethod(
					nameof(OtherMatchWithDifferentReturnType.GetString))), 
				Is.EqualTo(MethodMatch.DifferByReturnTypeOnly));
	}

	public class TargetMatch
	{
		public string GetString(int a) => null; 
	}

	public class OtherMatchWithExact
	{
		public string GetString(int a) => null; 
	}

	public class OtherMatchWithDifferentArgumentCount
	{
		public string GetString(int a, Guid b) => null; 
	}

	public class OtherMatchWithDifferentName
	{
		public string GetAString(int a) => null;
	}

	public class OtherMatchWithDifferentArgumentType
	{
		public string GetString(Guid a) => null; 
	}

	public class OtherMatchWithDifferentModifier
	{
		public string GetString(ref int a) => null; 
	}

	public class OtherMatchWithDifferentReturnType
	{
		public Guid GetString(int a) => default(Guid); 
	}
}
