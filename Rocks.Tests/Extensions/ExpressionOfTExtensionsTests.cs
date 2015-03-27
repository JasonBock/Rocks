using NUnit.Framework;
using Rocks.Extensions;
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

			Assert.AreEqual(7, results.Item1.Count, nameof(results.Item1.Count));
			Assert.AreEqual(7, results.Item2.Count, nameof(results.Item2.Count));

			Assert.AreEqual(typeof(int), results.Item1[0], "results.Item1[0]");
			Assert.AreEqual(typeof(string), results.Item1[1], "results.Item1[1]");
			Assert.AreEqual(typeof(string), results.Item1[2], "results.Item1[2]");
			Assert.AreEqual(typeof(Guid), results.Item1[3], "results.Item1[3]");
			Assert.AreEqual(typeof(string), results.Item1[4], "results.Item1[4]");
			Assert.AreEqual(typeof(Guid), results.Item1[5], "results.Item1[5]");
			Assert.AreEqual(typeof(string), results.Item1[6], "results.Item1[6]");

			Assert.IsTrue(typeof(ConstantExpression).IsAssignableFrom(results.Item2[0].GetType()), "results.Item2[0]");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results.Item2[1].GetType()), "results.Item2[1]");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results.Item2[2].GetType()), "results.Item2[2]");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results.Item2[3].GetType()), "results.Item2[3]");
			Assert.IsTrue(typeof(ConstantExpression).IsAssignableFrom(results.Item2[4].GetType()), "results.Item2[4]");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results.Item2[5].GetType()), "results.Item2[5]");
			Assert.IsTrue(typeof(MethodCallExpression).IsAssignableFrom(results.Item2[6].GetType()), "results.Item2[6]");
		}

		[Test]
		public void GetExpectationForSetter()
		{
			Expression<Func<string>> setter = () => "44";

			var expectation = setter.GetExpectationForSetter();


		}

		private static string GetValue()
		{
			return "44";
		}
   }
}
