using NUnit.Framework;
using System;
using System.Linq.Expressions;
using static Rocks.Extensions.MethodCallExpressionExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class MethodCallExpressionExtensionsTests
	{
		[Test]
		public void GetExpectationsForConstant()
		{
			Expression<Action<int>> expression = _ => this.Target(1);
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.True, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(1), nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForCall()
		{
			Expression<Action<int>> expression = _ => this.Target(this.Create());
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.True, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForCallToArgIs()
		{
			Expression<Action<int>> expression = _ => this.Target(Arg.Is<int>(a => a % 2 == 0));
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.True, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Not.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForCallToArgIsAny()
		{
			Expression<Action<int>> expression = _ => this.Target(Arg.IsAny<int>());
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.That(expectation.IsAny, Is.True, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForOtherNodeType()
		{
			Expression<Action<int>> expression = _ => this.Target(this.Create() + 1);
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = expectations["a"] as ArgumentExpectation<int>;
			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.True, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		public int Create() => 1; 

		public void Target(int a) { }
	}
}
