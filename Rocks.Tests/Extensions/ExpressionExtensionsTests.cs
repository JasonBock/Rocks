using NUnit.Framework;
using System;
using System.Linq.Expressions;
using static Rocks.Extensions.ExpressionExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class ExpressionExtensionsTests
	{
		[Test]
		public void CreateForCall()
		{
			var expectation = (ArgumentIsExpressionExpectation<int>)Expression.Call(
				this.GetType().GetMethod(nameof(ExpressionExtensionsTests.Create))).Create();

			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
		}

		[Test]
		public void CreateForCallToArgIs()
		{
			Expression<Func<int>> argument = () => Arg.Is<int>(_ => false);
			var expectation = (ArgumentIsEvaluationExpectation<int>)argument.Body.Create();

			Assert.That(expectation.Evaluation, Is.Not.Null, nameof(expectation.Evaluation));
		}

		[Test]
		public static void CreateForCallToArgIsAny()
		{
			Expression<Func<int>> argument = () => Arg.IsAny<int>();
			Assert.That(() => (ArgumentIsAnyExpectation<int>)argument.Body.Create(), Throws.Nothing);
		}

		[Test]
		public void CreateForConstant()
		{
			var expectation = (ArgumentIsValueExpectation<int>)Expression.Constant(44).Create();

			Assert.That(expectation.Value, Is.EqualTo(44), nameof(expectation.Value));
		}

		[Test]
		public void CreateForOtherNodeType()
		{
			var expectation = (ArgumentIsExpressionExpectation<int>)Expression.Add(
				Expression.Call(this.GetType().GetMethod(nameof(ExpressionExtensionsTests.Create))),
				Expression.Constant(1))
				.Create();

			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
		}

		public static int Create() => 44; 
	}
}