using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	public static class HandlerInformationOfTTests
	{
		[Test]
		public static void Create()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(expectations);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(1), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.ReturnValue, Is.EqualTo(default(int)), nameof(information.ReturnValue));
		}

		[Test]
		public static void CreateWithMethod()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var method = new Action(() => { });
			var information = new HandlerInformation<int>(method, expectations);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(1), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.ReturnValue, Is.EqualTo(default(int)), nameof(information.ReturnValue));
		}

		[Test]
		public static void CreateWithExpectedCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(2, expectations);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.ReturnValue, Is.EqualTo(default(int)), nameof(information.ReturnValue));
		}

		[Test]
		public static void CreateWithMethodAndExpectedCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var method = new Action(() => { });
			var information = new HandlerInformation<int>(method, 2, expectations);
			Assert.That(information.Method, Is.SameAs(method), nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.ReturnValue, Is.EqualTo(default(int)), nameof(information.ReturnValue));
		}

		[Test]
		public static void SetReturnValue()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(expectations)
			{
				ReturnValue = 1
			};
			Assert.That(information.ReturnValue, Is.EqualTo(1), nameof(information.ReturnValue));
		}
	}
}
