using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Linq.Expressions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ArgumentExpectationOfTTests
	{
		[Test]
		public void Create()
		{
			var expectation = new ArgumentExpectation<int>();
			Assert.IsTrue(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));

			expectation.Validate(1, "a");
		}

		[Test]
		public void CreateWithNullEvaluation()
		{
			Assert.Throws<ArgumentNullException>(() => new ArgumentExpectation<int>(null as Func<int, bool>));
		}

		[Test]
		public void CreateWithEvaluation()
		{
			var expectation = new ArgumentExpectation<int>(_ => _ % 2 == 0);
			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsTrue(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNotNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(int), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void CreateWithInvalidEvaluation()
		{
			var expectation = new ArgumentExpectation<int>(_ => _ % 2 == 0);
			Assert.Throws<ExpectationException>(() => expectation.Validate(1, "a"), nameof(expectation.Validate));
		}

		[Test]
		public void CreateWithValidEvaluation()
		{
			var expectation = new ArgumentExpectation<int>(_ => _ % 2 == 0);
			expectation.Validate(2, "a");
		}

		[Test]
		public void CreateWithValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsFalse(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsTrue(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual("a", expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void CreateWithNullValueAndComparedToNull()
		{
			var expectation = new ArgumentExpectation<string>(null as string);
			expectation.Validate(null, "a");
		}

		[Test]
		public void CreateWithNullValueAndComparedToNotNull()
		{
			var expectation = new ArgumentExpectation<string>(null as string);
			Assert.Throws<ExpectationException>(() => expectation.Validate("a", "a"), nameof(expectation.Validate));
		}

		[Test]
		public void CreateWithInvalidValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.Throws<ExpectationException>(() => expectation.Validate("b", "a"), nameof(expectation.Validate));
		}

		[Test]
		public void CreateWithInvalidNullValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.Throws<ExpectationException>(() => expectation.Validate(null, "a"), nameof(expectation.Validate));
		}

		[Test]
		public void CreateWithValidValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			expectation.Validate("a", "a");
		}

		[Test]
		public void CreateWithExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			Assert.IsFalse(expectation.IsAny, nameof(expectation.IsAny));
			Assert.IsFalse(expectation.IsEvaluation, nameof(expectation.IsEvaluation));
			Assert.IsTrue(expectation.IsExpression, nameof(expectation.IsExpression));
			Assert.IsFalse(expectation.IsValue, nameof(expectation.IsValue));
			Assert.IsNull(expectation.Evaluation, nameof(expectation.Evaluation));
			Assert.IsNotNull(expectation.Expression, nameof(expectation.Expression));
			Assert.AreEqual(default(string), expectation.Value, nameof(expectation.Value));
		}

		[Test]
		public void CreateWithNullExpression()
		{
			Assert.Throws<ArgumentNullException>(() => new ArgumentExpectation<string>(null as Expression));
		}

		[Test]
		public void CreateWithInvalidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			Assert.Throws<ExpectationException>(() => expectation.Validate("b", "a"), nameof(expectation.Validate));
		}

		[Test]
		public void CreateWithValidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			expectation.Validate("a", "a");
		}

		public static string GetValue()
		{
			return "a";
		}
	}
}
