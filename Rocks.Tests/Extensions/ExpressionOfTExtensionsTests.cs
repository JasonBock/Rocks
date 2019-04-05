using NUnit.Framework;
using System;
using System.Linq.Expressions;
using static Rocks.Extensions.ExpressionOfTExtensions;

namespace Rocks.Tests.Extensions
{
	public static class ExpressionOfTExtensionsTests
	{
		[Test]
		public static void ParseForPropertyIndexers()
		{
			Expression<Func<object[]>> indexers = () => new object[] { 44, ExpressionOfTExtensionsTests.GetValue(), Arg.IsAny<string>(),
				Arg.Is<Guid>(_ => true), "44", Guid.NewGuid(), Arg.Is<string>(_ => false) };

			var results = indexers.ParseForPropertyIndexers();

			Assert.That(results.Count, Is.EqualTo(7), nameof(results.Count));

			Assert.That(results[0].Type, Is.EqualTo(typeof(int)), "results[0].Type");
			Assert.That(results[1].Type, Is.EqualTo(typeof(string)), "results[1].Type");
			Assert.That(results[2].Type, Is.EqualTo(typeof(string)), "results[2].Type");
			Assert.That(results[3].Type, Is.EqualTo(typeof(Guid)), "results[3].Type");
			Assert.That(results[4].Type, Is.EqualTo(typeof(string)), "results[4].Type");
			Assert.That(results[5].Type, Is.EqualTo(typeof(Guid)), "results[5].Type");
			Assert.That(results[6].Type, Is.EqualTo(typeof(string)), "results[6].Type");

			Assert.That(typeof(ConstantExpression).IsAssignableFrom(results[0].GetType()), Is.True, "results[0].GetType()");
			Assert.That(typeof(MethodCallExpression).IsAssignableFrom(results[1].GetType()), Is.True, "results[1].GetType()");
			Assert.That(typeof(MethodCallExpression).IsAssignableFrom(results[2].GetType()), Is.True, "results[2].GetType()");
			Assert.That(typeof(MethodCallExpression).IsAssignableFrom(results[3].GetType()), Is.True, "results[3].GetType()");
			Assert.That(typeof(ConstantExpression).IsAssignableFrom(results[4].GetType()), Is.True, "results[4].GetType()");
			Assert.That(typeof(MethodCallExpression).IsAssignableFrom(results[5].GetType()), Is.True, "results[5].GetType()");
			Assert.That(typeof(MethodCallExpression).IsAssignableFrom(results[6].GetType()), Is.True, "results[6].GetType()");
		}

		[Test]
		public static void GetExpectationForSetter()
		{
			Expression<Func<string>> setter = () => "44";

			var expectation = setter.GetExpectationForSetter();

			Assert.That(expectation.Value, Is.EqualTo("44"), nameof(expectation.Value));
		}

		private static string GetValue() => "44";
	}
}
