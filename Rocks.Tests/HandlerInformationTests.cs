using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandlerInformationTests
	{
		[Test]
		public void Create()
		{
			var information = new HandlerInformation();
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.Null, nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public void CreateWithMethod()
		{
			var method = new Action(() => { });
			var information = new HandlerInformation(method);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.Null, nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public void CreateWithMethodAndExpectations()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(
				new Dictionary<string, ArgumentExpectation>
				{
					{ "a", new ArgumentExpectation<int>() }
				});
			var method = new Action(() => { });
			var information = new HandlerInformation(method, expectations);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.Null, nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(1), nameof(information.Expectations.Count));
		}

		[Test]
		public void CreateWithExpectedCallCount()
		{
			var information = new HandlerInformation(2);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public void CreateWithExpectedCallCountAndExpectations()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(
				new Dictionary<string, ArgumentExpectation>
				{
					{ "a", new ArgumentExpectation<int>() }
				});
			var information = new HandlerInformation(2, expectations);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(1), nameof(information.Expectations.Count));
		}

		[Test]
		public void CreateWithMethodAndExpectedCallCount()
		{
			var method = new Action(() => { });
			var information = new HandlerInformation(method, 2);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public void CreateWithMethodAndExpectedCallCountAndExpectations()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(
				new Dictionary<string, ArgumentExpectation>
				{
					{ "a", new ArgumentExpectation<int>() }
				});
			var method = new Action(() => { });
			var information = new HandlerInformation(method, 2, expectations);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(1), nameof(information.Expectations.Count));
		}

		[Test]
		public void IncrementCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			information.IncrementCallCount();

			Assert.That(information.CallCount, Is.EqualTo(1), nameof(information.CallCount));
		}

		[Test]
		public void VerifyWhenExpectedCallCountIsNotSpecified()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			information.IncrementCallCount();
			var result = information.Verify();

			Assert.That(result.Count, Is.EqualTo(0), nameof(result.Count));
		}

		[Test]
		public void VerifyWhenExpectedCallCountIsNotSpecifiedAndIncrementCallCountIsNotCalled()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			var result = information.Verify();

			Assert.That(result.Count, Is.EqualTo(1), nameof(result.Count));
		}

		[Test]
		public void VerifyWhenExpectedCallCountIsIncorrect()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(2, expectations);
			information.IncrementCallCount();
			var result = information.Verify();

			Assert.That(result.Count, Is.EqualTo(1), nameof(result.Count));
		}
	}
}
