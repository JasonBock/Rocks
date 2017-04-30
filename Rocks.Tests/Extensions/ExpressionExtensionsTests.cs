using NUnit.Framework;
using static Rocks.Extensions.ExpressionExtensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class ExpressionExtensionsTests
	{
		[Test]
		public void CreateForCall()
		{
			var expectation = Expression.Call(this.GetType().GetTypeInfo().GetMethod(nameof(ExpressionExtensionsTests.Create)))
				.Create() as ArgumentExpectation<int>;

			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.True, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		[Test]
		public void CreateForCallToArgIs()
		{
			Expression<Func<int>> argument = () => Arg.Is<int>(_ => false);
			var expectation = argument.Body.Create() as ArgumentExpectation<int>;

			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.True, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Not.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		[Test]
		public void CreateForCallToArgIsAny()
		{
			Expression<Func<int>> argument = () => Arg.IsAny<int>();
			var expectation = argument.Body.Create() as ArgumentExpectation<int>;

			Assert.That(expectation.IsAny, Is.True, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		[Test]
		public void CreateForConstant()
		{
			var expectation = Expression.Constant(44).Create() as ArgumentExpectation<int>;

			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.True, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(44), nameof(expectation.Value));
		}

		[Test]
		public void CreateForOtherNodeType()
		{
			var expectation = Expression.Add(
				Expression.Call(this.GetType().GetMethod(nameof(ExpressionExtensionsTests.Create))),
				Expression.Constant(1))
				.Create() as ArgumentExpectation<int>;

			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.True, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		public static int Create() { return 44; }
	}
}
