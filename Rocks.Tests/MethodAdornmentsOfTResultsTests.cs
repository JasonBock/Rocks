using NUnit.Framework;
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
			Assert.AreEqual(default(string), handler.ReturnValue, nameof(handler.ReturnValue));
		}

		[Test]
		public void SetReturnValue()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var newReturnValue = "a";
			var handler = new HandlerInformation<string>(expectations);
			var returnValue = new MethodAdornments<string>(handler);
			returnValue.Returns(newReturnValue);
			Assert.AreEqual(newReturnValue, handler.ReturnValue, nameof(handler.ReturnValue));
		}
	}
}
