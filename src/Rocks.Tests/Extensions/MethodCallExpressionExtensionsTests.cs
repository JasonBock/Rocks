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
			var expectation = (ArgumentIsValueExpectation<int>)expectations["a"];
			Assert.That(expectation.Value, Is.EqualTo(1), nameof(expectation.Value));
		}

		[Test]
		public void GetExpectationsForCall()
		{
			Expression<Action<int>> expression = _ => this.Target(this.Create());
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = (ArgumentIsExpressionExpectation<int>)expectations["a"];
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
		}

		[Test]
		public void GetExpectationsForCallToArgIs()
		{
			Expression<Action<int>> expression = _ => this.Target(Arg.Is<int>(a => a % 2 == 0));
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = (ArgumentIsEvaluationExpectation<int>)expectations["a"];
			Assert.That(expectation.Evaluation, Is.Not.Null, nameof(expectation.Evaluation));
		}

		[Test]
		public void GetExpectationsForCallToArgIsAny()
		{
			Expression<Action<int>> expression = _ => this.Target(Arg.IsAny<int>());
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			Assert.That(() => (ArgumentIsAnyExpectation<int>)expectations["a"], Throws.Nothing);
		}

		[Test]
		public void GetExpectationsForOtherNodeType()
		{
			Expression<Action<int>> expression = _ => this.Target(this.Create() + 1);
			var method = ((MethodCallExpression)expression.Body);
			var expectations = method.GetArgumentExpectations();

			Assert.That(expectations.Count, Is.EqualTo(1), nameof(expectations.Count));
			var expectation = (ArgumentIsExpressionExpectation<int>)expectations["a"];
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
		}

		public int Create() => 1; 

		public void Target(int a) { }
	}
}