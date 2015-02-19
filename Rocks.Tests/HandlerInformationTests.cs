using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandlerInformationTests
	{
		[Test]
		public void Create()
		{
			var information = new HandlerInformation();
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void CreateWithMethod()
		{
			var method = new Action(() => { });
         var information = new HandlerInformation(method);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(1, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void CreateWithExpectedCallCount()
		{
			var information = new HandlerInformation(2);
			Assert.IsNull(information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void CreateWithMethodAndExpectedCallCount()
		{
			var method = new Action(() => { });
			var information = new HandlerInformation(method, 2);
			Assert.AreSame(method, information.Method, nameof(information.Method));
			Assert.AreEqual(2, information.ExpectedCallCount, nameof(information.ExpectedCallCount));
			Assert.AreEqual(0, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void IncrementCallCount()
		{
			var information = new HandlerInformation();
			information.IncrementCallCount();

			Assert.AreEqual(1, information.CallCount, nameof(information.CallCount));
		}

		[Test]
		public void Verify()
		{
			var information = new HandlerInformation();
			information.IncrementCallCount();
			var result = information.Verify();

			Assert.AreEqual(0, result.Count, nameof(result.Count));
		}

		[Test]
		public void VerifyWhenExpectedCallCountIsIncorrect()
		{
			var information = new HandlerInformation();
			var result = information.Verify();

         Assert.AreEqual(1, result.Count, nameof(result.Count));
		}
	}
}
