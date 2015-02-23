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
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithMethod()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var method = new Action(() => { });
         var information = new HandlerInformation<int>(method, expectations);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithExpectedCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(2, expectations);
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithMethodAndExpectedCallCount()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var method = new Action(() => { });
			var information = new HandlerInformation<int>(method, 2, expectations);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void SetReturnValue()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var information = new HandlerInformation<int>(expectations);
			information.ReturnValue = 1;

			Assert.AreEqual(1, information.ReturnValue, nameof(information.ReturnValue));
		}
	}
}
