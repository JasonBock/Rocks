using NUnit.Framework;
using static Rocks.Extensions.ExpressionOfTExtensions;
using System;
using System.Linq.Expressions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class ExpressionOfTExtensionsTests
	{
		[Test]
		public void ParseForPropertyIndexers()
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
		public void GetExpectationForSetter()
		{
			Expression<Func<string>> setter = () => "44";

			var expectation = setter.GetExpectationForSetter();

			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.True, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo("44"), nameof(expectation.Value));
		}

		private static string GetValue()
		{
			return "44";
		}
   }
}
