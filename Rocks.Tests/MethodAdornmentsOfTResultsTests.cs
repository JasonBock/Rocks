using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class MethodAdornmentsOfTResultsTests
	{
		[Test]
		public void Create()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var handler = new HandlerInformation<string>(expectations);
			var returnValue = new MethodAdornments<string>(handler);
			Assert.That(handler.ReturnValue, Is.EqualTo(default(string)), nameof(handler.ReturnValue));
		}

		[Test]
		public void SetReturnValue()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var newReturnValue = "a";
			var handler = new HandlerInformation<string>(expectations);
			var returnValue = new MethodAdornments<string>(handler);
			returnValue.Returns(newReturnValue);
			Assert.That(handler.ReturnValue, Is.EqualTo(newReturnValue), nameof(handler.ReturnValue));
		}
	}
}
