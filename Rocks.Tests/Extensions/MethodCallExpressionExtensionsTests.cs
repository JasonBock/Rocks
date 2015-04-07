using NUnit.Framework;
using static Rocks.Extensions.MethodCallExpressionExtensions;
using System;
using System.Linq.Expressions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodCallExpressionExtensionsTests
	{
		[Test]
		public void GetExpectationsForConstant()
		{
			Expression<Action<int>> expression = _ => this.Target(1);
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.AreEqual(1, expectations.Count, nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsTrue(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(1, expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForCall()
		{
			Expression<Action<int>> expression = _ => this.Target(this.Create());
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.AreEqual(1, expectations.Count, nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsTrue(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNotNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForCallToArgIs()
		{
			Expression<Action<int>> expression = _ => this.Target(Arg.Is<int>(a => a % 2 == 0));
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.AreEqual(1, expectations.Count, nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsTrue(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNotNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForCallToArgIsAny()
		{
			Expression<Action<int>> expression = _ => this.Target(Arg.IsAny<int>());
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.AreEqual(1, expectations.Count, nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.IsTrue(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForOtherNodeType()
		{
			Expression<Action<int>> expression = _ => this.Target(this.Create() + 1);
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.AreEqual(1, expectations.Count, nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsTrue(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNotNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		public int Create() { return 1; }

		public void Target(int a) { }
	}
}
