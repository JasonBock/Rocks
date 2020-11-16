using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	public static class MethodAdornmentsOfTResultsTests
	{
		[Test]
		public static void Create()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var handler = new HandlerInformation<string>(expectations);
			Assert.That(handler.ReturnValue, Is.EqualTo(default(string)), nameof(handler.ReturnValue));
		}

		[Test]
		public static void SetReturnValue()
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
