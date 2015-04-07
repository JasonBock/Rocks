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

			Assert.AreEqual(7, results.Count, nameof(results.Count));

			Assert.AreEqual(typeof(int), results[0].Type, "results[0].Type");
			Assert.AreEqual(typeof(string), results[1].Type, "results[1].Type");
			Assert.AreEqual(typeof(string), results[2].Type, "results[2].Type");
			Assert.AreEqual(typeof(Guid), results[3].Type, "results[3].Type");
			Assert.AreEqual(typeof(string), results[4].Type, "results[4].Type");
			Assert.AreEqual(typeof(Guid), results[5].Type, "results[5].Type");
			Assert.AreEqual(typeof(string), results[6].Type, "results[6].Type");

			Assert.IsTrue(typeof(ConstantExpression).IsAssignableFrom(results[0].GetType()), "results[0].GetType()");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results[1].GetType()), "results[1].GetType()");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results[2].GetType()), "results[2].GetType()");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results[3].GetType()), "results[3].GetType()");
			Assert.IsTrue(typeof(ConstantExpression).IsAssignableFrom(results[4].GetType()), "results[4].GetType()");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results[5].GetType()), "results[5].GetType()");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results[6].GetType()), "results[6].GetType()");
		}

		[Test]
		public void GetExpectationForSetter()
		{
			Expression<Func<string>> setter = () => "44";

			var expectation = setter.GetExpectationForSetter();

			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsTrue(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual("44", expectation.Value, nameof(expectation.Value));
		}

		private static string GetValue()
		{
			return "44";
		}
   }
}
