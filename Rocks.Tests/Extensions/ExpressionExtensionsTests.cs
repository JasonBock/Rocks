using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq.Expressions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class ExpressionExtensionsTests
	{
		[Test]
		public void CreateForCall()
		{
			var expectation = Expression.Call(this.GetType().GetMethod(nameof(ExpressionExtensionsTests.Create)))
				.Create() as ArgumentExpectation<int>;

			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsTrue(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNotNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void CreateForCallToArgIs()
		{
			Expression<Func<int>> argument = () => Arg.Is<int>(_ => false);
			var expectation = argument.Body.Create() as ArgumentExpectation<int>;

			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsTrue(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNotNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void CreateForCallToArgIsAny()
		{
			Expression<Func<int>> argument = () => Arg.IsAny<int>();
			var expectation = argument.Body.Create() as ArgumentExpectation<int>;

			Assert.IsTrue(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void CreateForConstant()
		{
			var expectation = Expression.Constant(44).Create() as ArgumentExpectation<int>;

			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsTrue(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(44, expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void CreateForOtherNodeType()
		{
			var expectation = Expression.Add(
				Expression.Call(this.GetType().GetMethod(nameof(ExpressionExtensionsTests.Create))),
				Expression.Constant(1))
				.Create() as ArgumentExpectation<int>;

			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsTrue(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNotNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		public static int Create() { return 44; }
	}
}
