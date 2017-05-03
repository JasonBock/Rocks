using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ArgumentExpectationOfTTests
	{
		[Test]
		public void Create()
		{
			var expectation = new ArgumentExpectation<int>();
			Assert.That(expectation.IsAny, Is.True, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
			Assert.That(expectation.IsValid(1, "a"), Is.True, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullEvaluation() =>
			Assert.That(() => new ArgumentExpectation<int>(null as Func<int, bool>),
				Throws.TypeOf<ArgumentNullException>());

		[Test]
		public void CreateWithEvaluation()
		{
			var expectation = new ArgumentExpectation<int>(_ => _ % 2 == 0);
			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.True, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Not.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(int)), nameof(expectation.Value));
		}

		[Test]
		public void CreateWithInvalidEvaluation()
		{
			var expectation = new ArgumentExpectation<int>(_ => _ % 2 == 0);
			Assert.That(expectation.IsValid(1, "a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidEvaluation()
		{
			var expectation = new ArgumentExpectation<int>(_ => _ % 2 == 0);
			Assert.That(expectation.IsValid(2, "a"), Is.True);
		}

		[Test]
		public void CreateWithValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.False, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.True, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo("a"), nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullValueAndComparedToNull()
		{
			var expectation = new ArgumentExpectation<string>(null as string);
			Assert.That(expectation.IsValid(null, "a"), Is.True, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullValueAndComparedToNotNull()
		{
			var expectation = new ArgumentExpectation<string>(null as string);
			Assert.That(expectation.IsValid("a", "a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.That(expectation.IsValid("b", "a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidNullValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.That(expectation.IsValid(null, "a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.That(expectation.IsValid("a", "a"), Is.True, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidArrayValueDueToDifferentArrayLengths()
		{
			var expectation = new ArgumentExpectation<string[]>(new[] { "a" });
			Assert.That(expectation.IsValid(new[] { "a", "a" }, "a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithInvalidArrayValueDueToDifferentValues()
		{
			var expectation = new ArgumentExpectation<string[]>(new[] { "a" });
			Assert.That(expectation.IsValid(new[] { "b" }, "a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidArrayValue()
		{
			var expectation = new ArgumentExpectation<string[]>(new[] { "a" });
			Assert.That(expectation.IsValid(new[] { "a" }, "a"), Is.True, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithExpression()
		{
			var expression = Expression.Call(this.GetType().GetTypeInfo().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			Assert.That(expectation.IsAny, Is.False, nameof(expectation.IsAny));
			Assert.That(expectation.IsEvaluation, Is.False, nameof(expectation.IsEvaluation));
			Assert.That(expectation.IsExpression, Is.True, nameof(expectation.IsExpression));
			Assert.That(expectation.IsValue, Is.False, nameof(expectation.IsValue));
			Assert.That(expectation.Evaluation, Is.Null, nameof(expectation.Evaluation));
			Assert.That(expectation.Expression, Is.Not.Null, nameof(expectation.Expression));
			Assert.That(expectation.Value, Is.EqualTo(default(string)), nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullExpression() =>
			Assert.That(() => new ArgumentExpectation<string>(null as Expression), Throws.TypeOf<ArgumentNullException>());

		[Test]
		public void CreateWithInvalidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			Assert.That(expectation.IsValid("b", "a"), Is.False, nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationOfTTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			Assert.That(expectation.IsValid("a", "a"), Is.True, nameof(expectation.IsValid));
		}

		public static string GetValue() => "a";
	}
}
