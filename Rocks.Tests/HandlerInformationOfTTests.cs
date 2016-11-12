using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandlerInformationOfTTests
	{
		[Test]
		public void Create()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(expectations);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(1), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.ReturnValue, Is.EqualTo(default(int)), nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithMethod()
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
		public void CreateWithExpectedCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(2, expectations);
			Assert.That(information.Method, Is.Null, nameof(information.Method));
			Assert.That(information.ExpectedCallCount, Is.EqualTo(2), nameof(information.ExpectedCallCount));
			Assert.That(information.CallCount, Is.EqualTo(0), nameof(information.CallCount));
			Assert.That(information.ReturnValue, Is.EqualTo(default(int)), nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithMethodAndExpectedCallCount()
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
		public void SetReturnValue()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(expectations);
			information.ReturnValue = 1;

			Assert.That(information.ReturnValue, Is.EqualTo(1), nameof(information.ReturnValue));
		}
	}
}
