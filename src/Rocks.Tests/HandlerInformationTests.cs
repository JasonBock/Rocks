using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	public static class HandlerInformationTests
	{
		[Test]
		public static void Create()
		{
			var information = new HandlerInformation();
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.Null, nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public static void CreateWithMethod()
		{
			var method = new Action(() => { });
			var information = new HandlerInformation(method);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.Null, nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public static void CreateWithMethodAndExpectations()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(
				new Dictionary<string, ArgumentExpectation>
				{
					{ "a", new ArgumentIsAnyExpectation<int>() }
				});
			var method = new Action(() => { });
			var information = new HandlerInformation(method, expectations);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.Null, nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(1), nameof(information.Expectations.Count));
		}

		[Test]
		public static void CreateWithExpectedCallCount()
		{
			var information = new HandlerInformation(2);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public static void CreateWithExpectedCallCountAndExpectations()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(
				new Dictionary<string, ArgumentExpectation>
				{
					{ "a", new ArgumentIsAnyExpectation<int>() }
				});
			var information = new HandlerInformation(2, expectations);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(1), nameof(information.Expectations.Count));
		}

		[Test]
		public static void CreateWithMethodAndExpectedCallCount()
		{
			var method = new Action(() => { });
			var information = new HandlerInformation(method, 2);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(0), nameof(information.Expectations.Count));
		}

		[Test]
		public static void CreateWithMethodAndExpectedCallCountAndExpectations()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(
				new Dictionary<string, ArgumentExpectation>
				{
					{ "a", new ArgumentIsAnyExpectation<int>() }
				});
			var method = new Action(() => { });
			var information = new HandlerInformation(method, 2, expectations);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.Expectations.Count, Is.EqualTo(1), nameof(information.Expectations.Count));
		}

		[Test]
		public static void IncrementCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			information.IncrementCallCount();

			Assert.That(information.CallCount, Is.EqualTo(1), nameof(information.CallCount));
		}

		[Test]
		public static void RaiseEvents()
		{
			var name = nameof(RaiseEvents);
			var args = new EventArgs();

			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			information.AddRaiseEvent(new RaiseEventInformation(name, args));

			var mock = Rock.Create<IMockWithEvents>();
			mock.Handle(_ => _.Raise(name, args));

			information.RaiseEvents(mock.Make());

			mock.Verify();
		}

		[Test]
		public static void RaiseEventsWhenTargetIsNull()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);

			Assert.That(() => information.RaiseEvents((null as IMockWithEvents)!), Throws.TypeOf<ArgumentNullException>());
		}

		[Test]
		public static void VerifyWhenExpectedCallCountIsNotSpecified()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			information.IncrementCallCount();
			var result = information.Verify();

			Assert.That(result.Count, Is.EqualTo(0), nameof(result.Count));
		}

		[Test]
		public static void VerifyWhenExpectedCallCountIsNotSpecifiedAndIncrementCallCountIsNotCalled()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			var result = information.Verify();

			Assert.That(result.Count, Is.EqualTo(1), nameof(result.Count));
		}

		[Test]
		public static void VerifyWhenExpectedCallCountIsIncorrect()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(2, expectations);
			information.IncrementCallCount();
			var result = information.Verify();

			Assert.That(result.Count, Is.EqualTo(1), nameof(result.Count));
		}
	}
}
