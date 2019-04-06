using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace Rocks.Tests
{
	public sealed class ArgumentExpectationOfTTests
	{
		[Test]
		public void Create()
		{
			var expectation = new ArgumentIsAnyExpectation<int>();
			Assert.That(expectation.IsValid(2), Is.True);
		}

		[Test]
		public void CreateWithNullEvaluation() =>
			Assert.That(() => new ArgumentIsEvaluationExpectation<int>(null as Func<int, bool>),
				Throws.TypeOf<ArgumentNullException>());

		[Test]
		public void CreateWithEvaluation()
		{
			var expectation = new ArgumentIsEvaluationExpectation<int>(_ => _ % 2 == 0);
			Assert.That(expectation.Evaluation, Is.Not.Null, nameof(expectation.Evaluation));
		}

		[Test]
		public void CreateWithInvalidEvaluation()
		{
			var expectation = new ArgumentIsEvaluationExpectation<int>(_ => _ % 2 == 0);
			Assert.That(expectation.IsValid(1), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidEvaluation()
		{
			var expectation = new ArgumentIsEvaluationExpectation<int>(_ => _ % 2 == 0);
			Assert.That(expectation.IsValid(2), Is.True);
		}

		[Test]
		public void CreateWithValue()
		{
			var expectation = new ArgumentIsValueExpectation<string>("a");
			Assert.That(expectation.Value, Is.EqualTo("a"), nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullValueAndComparedToNull()
		{
			var expectation = new ArgumentIsValueExpectation<string>(null as string);
			Assert.That(expectation.IsValid(null), Is.True, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullValueAndComparedToNotNull()
		{
			var expectation = new ArgumentIsValueExpectation<string>(null as string);
			Assert.That(expectation.IsValid("a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidValue()
		{
			var expectation = new ArgumentIsValueExpectation<string>("a");
			Assert.That(expectation.IsValid("b"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidNullValue()
		{
			var expectation = new ArgumentIsValueExpectation<string>("a");
			Assert.That(expectation.IsValid(null), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidValue()
		{
			var expectation = new ArgumentIsValueExpectation<string>("a");
			Assert.That(expectation.IsValid("a"), Is.True, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidArrayValueDueToDifferentArrayLengths()
		{
			var expectation = new ArgumentIsValueExpectation<string[]>(new[] { "a" });
			Assert.That(expectation.IsValid(new[] { "a", "a" }), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidArrayValueDueToDifferentValues()
		{
			var expectation = new ArgumentIsValueExpectation<string[]>(new[] { "a" });
			Assert.That(expectation.IsValid(new[] { "b" }), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidArrayValue()
		{
			var expectation = new ArgumentIsValueExpectation<string[]>(new[] { "a" });
			Assert.That(expectation.IsValid(new[] { "a" }), Is.True, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentIsExpressionExpectation<string>(expression);
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
		}

		[Test]
		public void CreateWithNullExpression() =>
			Assert.That(() => new ArgumentIsExpressionExpectation<string>(null as Expression), Throws.TypeOf<ArgumentNullException>());

		[Test]
		public void CreateWithInvalidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentIsExpressionExpectation<string>(expression);
			Assert.That(expectation.IsValid("b"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentIsExpressionExpectation<string>(expression);
			Assert.That(expectation.IsValid("a"), Is.True, nameof(expectation.IsValid));
		}

		public static string GetValue() => "a";
	}
}