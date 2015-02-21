using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandlerInformationOfTTests
	{
		[Test]
		public void Create()
		{
			var information = new HandlerInformation<int>();
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithMethod()
		{
			var method = new Action(() => { });
         var information = new HandlerInformation<int>(method);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithExpectedCallCount()
		{
			var information = new HandlerInformation<int>(2);
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void CreateWithMethodAndExpectedCallCount()
		{
			var method = new Action(() => { });
			var information = new HandlerInformation<int>(method, 2);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
			Assert.AreEqual(default(int), information.ReturnValue, nameof(information.ReturnValue));
		}

		[Test]
		public void SetReturnValue()
		{
			var information = new HandlerInformation<int>();
			information.ReturnValue = 1;

			Assert.AreEqual(1, information.ReturnValue, nameof(information.ReturnValue));
		}
	}
}
