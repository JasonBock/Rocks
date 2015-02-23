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
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void CreateWithMethod()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var method = new Action(() => { });
         var information = new HandlerInformation(method, expectations);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void CreateWithExpectedCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(2, expectations);
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void CreateWithMethodAndExpectedCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var method = new Action(() => { });
			var information = new HandlerInformation(method, 2, expectations);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void IncrementCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			information.IncrementCallCount();

			Assert.AreEqual(1, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void Verify()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			information.IncrementCallCount();
			var result = information.Verify();

			Assert.AreEqual(0, result.Count, nameof(result.Count));
		}

		[Test]
		public void VerifyWhenExpectedCallCountIsIncorrect()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation(expectations);
			var result = information.Verify();

         Assert.AreEqual(1, result.Count, nameof(result.Count));
		}
	}
}
