using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ArgumentExpectationTests
	{
		[Test]
		public void Create()
		{
			var expectation = new ArgumentExpectation<int>();
			Assert.IsTrue(expectation.IsValid(1), nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullValue()
		{
			Assert.Throws<ArgumentNullException>(() => new ArgumentExpectation<string>(null as string));
		}

		[Test]
		public void CreateWithInvalidValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.IsFalse(expectation.IsValid("b"), nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidValue()
		{
			var expectation = new ArgumentExpectation<string>("a");
			Assert.IsTrue(expectation.IsValid("a"), nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithNullExpress()
		{
			Assert.Throws<ArgumentNullException>(() => new ArgumentExpectation<string>(null as Expression));
		}

		[Test]
		public void CreateWithInvalidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			Assert.IsFalse(expectation.IsValid("b"), nameof(expectation.IsValid));
		}

		[Test]
		public void CreateWithValidValueFromExpression()
		{
			var expression = Expression.Call(this.GetType().GetMethod(nameof(ArgumentExpectationTests.GetValue)));
			var expectation = new ArgumentExpectation<string>(expression);
			Assert.IsTrue(expectation.IsValid("a"), nameof(expectation.IsValid));
		}

		public static string GetValue()
		{
			return "a";
		}
	}
}
